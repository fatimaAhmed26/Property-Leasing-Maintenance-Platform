using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PropertyLeasing.API.Data;
using PropertyLeasing.API.Hubs;
using PropertyLeasing.API.Models;
using PropertyLeasing.API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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

var jwt = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SecretKey"]!))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                context.Token = accessToken;
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("PropertyManager", p => p.RequireRole("Property Manager"))
    .AddPolicy("MaintenanceStaff", p => p.RequireRole("Maintenance Staff"))
    .AddPolicy("Tenant", p => p.RequireRole("Tenant"));

builder.Services.AddScoped<ILeaseLifecycleService, LeaseLifecycleService>();
builder.Services.AddScoped<IMaintenanceLifecycleService, MaintenanceLifecycleService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC", policy =>
        policy.WithOrigins("https://localhost:7207", "http://localhost:5209",
                           "https://localhost:7208", "http://localhost:5210")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
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

    async Task EnsureUser(string email, string password, string fullName, string role)
    {
        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new AppUser { UserName = email, Email = email, FullName = fullName, EmailConfirmed = true };
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, role);
        }
    }

    await EnsureUser("manager@test.com", "Pass123", "Ahmed Ali", "Property Manager");
    await EnsureUser("maintenance@test.com", "Pass123", "Ahmed Qusy", "Maintenance Staff");
    await EnsureUser("zahra@test.com", "Pass123", "Zahra Almosawi", "Tenant");
    await EnsureUser("sarah@test.com", "Pass123", "Sarah Ahmed", "Tenant");
}

app.UseCors("AllowMVC");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<MaintenanceHub>("/hubs/maintenance");

app.Run();
