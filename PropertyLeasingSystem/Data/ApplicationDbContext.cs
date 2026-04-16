using Microsoft.EntityFrameworkCore;
using PropertyLeasingSystem.Models;
using static System.Net.Mime.MediaTypeNames;

namespace PropertyLeasingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Staff> StaffMembers { get; set; }
        public DbSet<PropertyLeasingSystem.Models.Application> Applications { get; set; }
        public DbSet<Lease> Leases { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
        public DbSet<MaintenanceLog> MaintenanceLogs { get; set; }
        public DbSet<StatusHistory> StatusHistories { get; set; }
        public DbSet<Lease> Lease { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequest { get; set; }
        public DbSet<MaintenanceLog> MaintenanceLog { get; set; }
        public DbSet<StatusHistory> StatusHistorie { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed a Test Property
            modelBuilder.Entity<Property>().HasData(
                new Property { PropertyId = 1, PropertyName = "Polytechnic Towers", Address = "Isa Town", City = "Manama", PropertyType = "Apartment", ManagerId = 1 }
            );

            // Seed a Test Tenant
            modelBuilder.Entity<Tenant>().HasData(
                new Tenant { TenantId = 1, FullName = "Abrar Al-Awadhi", Email = "abrar@example.com", Phone = "12345678" }
            );
        }
    }

    }