using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasing.API.Models;
using PropertyLeasing.API.Services;
using PropertyLeasingSystem.Filters;
using PropertyLeasingSystem.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<NotificationCountFilter>();
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddScoped<ILeaseLifecycleService, LeaseLifecycleService>();
builder.Services.AddScoped<IMaintenanceLifecycleService, MaintenanceLifecycleService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddSignalR();

builder.Services.AddHttpClient("MaintenanceAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5212/");
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    string[] roles = ["Property Manager", "Maintenance Staff", "Tenant"];
    foreach (var role in roles)
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));

    async Task EnsureUser(string email, string password, string fullName, string role, int? tenantId = null, int? staffId = null)
    {
        var existing = await userManager.FindByEmailAsync(email);
        if (existing == null)
        {
            var user = new AppUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true,
                TenantId = tenantId,
                StaffId = staffId
            };
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, role);
        }
        else if (existing.TenantId == null && tenantId != null)
        {
            existing.TenantId = tenantId;
            await userManager.UpdateAsync(existing);
        }
        else if (existing.StaffId == null && staffId != null)
        {
            existing.StaffId = staffId;
            await userManager.UpdateAsync(existing);
        }
    }

    // Staff accounts  (StaffId matches DataSeed)
    await EnsureUser("manager@test.com",     "Pass123", "Ahmed Ali",     "Property Manager",  staffId: 1);
    await EnsureUser("maintenance@test.com", "Pass123", "Ahmed Qusy",    "Maintenance Staff", staffId: 2);
    await EnsureUser("khalid@test.com",      "Pass123", "Khalid Nasser", "Maintenance Staff", staffId: 3);
    await EnsureUser("fatima@test.com",      "Pass123", "Fatima Hassan", "Maintenance Staff", staffId: 4);

    // Tenant accounts  (TenantId matches DataSeed)
    await EnsureUser("zahra@test.com", "Pass123", "Zahra Almosawi", "Tenant", tenantId: 1);
    await EnsureUser("sarah@test.com", "Pass123", "Sarah Ahmed",    "Tenant", tenantId: 2);
    await EnsureUser("omar@test.com",  "Pass123", "Omar Khalil",    "Tenant", tenantId: 3);
    await EnsureUser("noura@test.com", "Pass123", "Noura Jassim",   "Tenant", tenantId: 4);

    // Seed notifications per user if they have none yet
    async Task SeedNotifications(string email, List<(string Message, bool IsRead, DateTime CreatedAt)> items)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null) return;

        bool hasAny = context.Notifications.Any(n => n.UserId == user.Id);
        if (hasAny) return;

        foreach (var (message, isRead, createdAt) in items)
        {
            context.Notifications.Add(new Notification
            {
                UserId    = user.Id,
                Message   = message,
                IsRead    = isRead,
                CreatedAt = createdAt
            });
        }
        await context.SaveChangesAsync();
    }

    await SeedNotifications("zahra@test.com", [
        ("Your application #1 has been approved!", true,  new DateTime(2025, 1, 3)),
        ("Your lease for unit A102 has been activated.", true,  new DateTime(2025, 1, 5)),
        ("Your maintenance request #1 has been assigned to staff.", true,  new DateTime(2026, 4, 10)),
        ("Your maintenance request #1 status updated to: InProgress.", true,  new DateTime(2026, 4, 11)),
        ("Your maintenance request #1 status updated to: Resolved.", true,  new DateTime(2026, 4, 13)),
        ("Your maintenance request #1 has been closed.", true,  new DateTime(2026, 4, 15)),
        ("Your maintenance request #2 has been assigned to staff.", true,  new DateTime(2026, 4, 25)),
        ("Your maintenance request #2 status updated to: Resolved.", true,  new DateTime(2026, 4, 26)),
        ("Your maintenance request #6 has been submitted. Ticket #6.", false, new DateTime(2026, 5, 6)),
        ("Your maintenance request #19 has been submitted. Ticket #19.", false, new DateTime(2026, 5, 7)),
    ]);

    await SeedNotifications("sarah@test.com", [
        ("Your application #2 has been approved!", true,  new DateTime(2025, 2, 4)),
        ("Your lease for unit T101 has been activated.", true,  new DateTime(2025, 2, 5)),
        ("Your maintenance request #3 has been assigned to staff.", true,  new DateTime(2026, 5, 1)),
        ("Your maintenance request #3 status updated to: InProgress.", true,  new DateTime(2026, 5, 2)),
        ("Your maintenance request #3 status updated to: Resolved.", false, new DateTime(2026, 5, 2)),
        ("Your maintenance request #7 has been submitted. Ticket #7.", false, new DateTime(2026, 5, 7)),
    ]);

    await SeedNotifications("omar@test.com", [
        ("Your application #3 has been approved!", true,  new DateTime(2025, 3, 5)),
        ("Your lease for unit A103 has been activated.", true,  new DateTime(2025, 3, 10)),
        ("Your maintenance request #4 has been assigned to staff.", true,  new DateTime(2026, 5, 3)),
        ("Your maintenance request #4 status updated to: InProgress.", false, new DateTime(2026, 5, 4)),
        ("Your maintenance request #11 status updated to: Resolved.", false, new DateTime(2026, 4, 29)),
        ("Your maintenance request #18 has been submitted. Ticket #18.", false, new DateTime(2026, 5, 7)),
    ]);

    await SeedNotifications("noura@test.com", [
        ("Your application #4 has been approved!", true,  new DateTime(2025, 4, 3)),
        ("Your lease for unit B201 has been activated.", true,  new DateTime(2025, 4, 5)),
        ("Your maintenance request #5 has been assigned to staff.", false, new DateTime(2026, 5, 5)),
        ("Your maintenance request #12 status updated to: Resolved.", false, new DateTime(2026, 5, 3)),
        ("Your maintenance request #17 has been submitted. Ticket #17.", false, new DateTime(2026, 5, 7)),
    ]);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<MaintenanceHub>("/hubs/maintenance");

app.Run();
