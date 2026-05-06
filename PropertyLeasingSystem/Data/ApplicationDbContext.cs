using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PropertyLeasing.API.Models;
using PropertyLeasingSystem.Data;

namespace PropertyLeasing.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Lease> Leases { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
        public DbSet<MaintenanceLog> MaintenanceLogs { get; set; }
        public DbSet<StatusHistory> StatusHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // DELETE this line after your database is successfully created
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Data
            DataSeed.Seed(modelBuilder);

            // Relationships (Excellent work here)
            modelBuilder.Entity<Property>()
                .HasMany(p => p.Units)
                .WithOne(u => u.Property)
                .HasForeignKey(u => u.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tenant>().HasMany(t => t.Applications).WithOne(a => a.Tenant).HasForeignKey(a => a.TenantId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Unit>().HasMany(u => u.Applications).WithOne(a => a.Unit).HasForeignKey(a => a.UnitId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Lease>().HasMany(l => l.Payments).WithOne(p => p.Lease).HasForeignKey(p => p.LeaseId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Unit>().HasMany(u => u.MaintenanceRequests).WithOne(m => m.Unit).HasForeignKey(m => m.UnitId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Tenant>().HasMany(t => t.MaintenanceRequests).WithOne(m => m.Tenant).HasForeignKey(m => m.TenantId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MaintenanceRequest>().HasMany(m => m.MaintenanceLogs).WithOne(l => l.MaintenanceRequest).HasForeignKey(l => l.RequestId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Staff>().HasMany(s => s.MaintenanceLogs).WithOne(m => m.Staff).HasForeignKey(m => m.StaffId).OnDelete(DeleteBehavior.Restrict);

            // Precision Settings (Crucial for Financial Data)
            modelBuilder.Entity<Unit>().Property(u => u.RentAmount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Lease>().Property(l => l.MonthlyRent).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Staff>().Property(s => s.HourlyRate).HasColumnType("decimal(18,2)");
        }
    }
}