using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using PropertyLeasingSystem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ILeaseLifecycleService, LeaseLifecycleService>();
builder.Services.AddScoped<IMaintenanceLifecycleService, MaintenanceLifecycleService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddHttpClient("MaintenanceAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5212/");
});

var app = builder.Build();

// ONLY keep Migrate() if you want auto-migration. 
// REMOVE DataSeed.Seed(context) from here.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
