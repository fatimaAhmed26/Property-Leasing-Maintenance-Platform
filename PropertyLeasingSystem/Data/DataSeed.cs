using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Models;
using PropertyLeasingSystem.Models; 
using System;

namespace PropertyLeasingSystem.Data
{
    public static class DataSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // 1. Seed Staff
            modelBuilder.Entity<Staff>().HasData(
                new Staff { StaffId = 1, FullName = "Ahmed Ali", Email = "manager@test.com", RoleType = "Property Manager", HourlyRate = 25m, SkillType = "Management", IsAvailable = true },
                new Staff { StaffId = 2, FullName = "Ahmed Qusy", Email = "maintenance@test.com", RoleType = "Maintenance Staff", HourlyRate = 15m, SkillType = "Plumbing", IsAvailable = true }
            );

            // 2. Seed Properties
            modelBuilder.Entity<Property>().HasData(
                new Property { PropertyId = 1, PropertyName = "Bahrain Polytechnic", Address = "Campus A", City = "Isa Town", PropertyType = "Residential", ManagerId = 1 }
            );

            // 3. Seed Units
            modelBuilder.Entity<Unit>().HasData(
                new Unit { UnitId = 1, PropertyId = 1, UnitNumber = "A101", RentAmount = 350m, IsAvailable = true, Size = "2 Bedroom", Amenities = "Parking, WiFi" },
                new Unit { UnitId = 2, PropertyId = 1, UnitNumber = "A102", RentAmount = 450m, IsAvailable = false, Size = "3 Bedroom", Amenities = "Parking, Pool" }
            );

            // 4. Seed Tenants
            modelBuilder.Entity<Tenant>().HasData(
                new Tenant { TenantId = 1, FullName = "Zahra Almosawi", Email = "zahra@test.com", Phone = "33334444" },
                new Tenant { TenantId = 2, FullName = "Sarah Ahmed", Email = "sarah@test.com", Phone = "35556666" }
            );

            // 5. Seed Applications
            modelBuilder.Entity<Application>().HasData(
                new Application { ApplicationId = 1, TenantId = 1, UnitId = 2, SubmittedAt = new DateTime(2026, 5, 6, 10, 0, 0), Status = "Approved", ApprovedByStaffId = 1 }
            );

            // 6. Seed Leases (FIXED: Only one Lease with ID 1)
            modelBuilder.Entity<Lease>().HasData(
                new Lease
                {
                    LeaseId = 1,
                    ApplicationId = 1,
                    UnitId = 2,
                    TenantId = 1,
                    StartDate = new DateTime(2025, 1, 1),
                    EndDate = new DateTime(2025, 12, 31),
                    MonthlyRent = 500.00m,
                    CreatedAt = DateTime.Now,
                    Status = "Active"
                }
            );

            // 7. Seed Payments
            modelBuilder.Entity<Payment>().HasData(
                new Payment { PaymentId = 1, LeaseId = 1, Amount = 450m, PaymentDate = DateTime.Now, TransactionTimestamp = DateTime.Now, Method = "Cash", PaymentType = "Monthly Rent", Status = "Paid" }
            );

            // 8. Seed Maintenance Requests
            modelBuilder.Entity<MaintenanceRequest>().HasData(
                new MaintenanceRequest { RequestId = 1, UnitId = 2, TenantId = 1, Description = "Air conditioner not working", Priority = "High", ReportedAt = DateTime.Now, Status = "Submitted" }
            );

            // 9. Seed Maintenance Logs
            modelBuilder.Entity<MaintenanceLog>().HasData(
                new MaintenanceLog { LogId = 1, RequestId = 1, StaffId = 2, WorkStarted = DateTime.Now, ActionTaken = "Inspection started" }
            );

            // 10. Seed Status Histories
            modelBuilder.Entity<StatusHistory>().HasData(
                new StatusHistory { AuditId = 1, EntityName = "MaintenanceRequest", EntityId = 1, OldStatus = "Submitted", NewStatus = "Assigned", ChangedAt = DateTime.Now }
            );
        }
    }
}