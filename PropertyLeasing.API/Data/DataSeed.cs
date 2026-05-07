using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Models;

namespace PropertyLeasing.API.Data
{
    public static class DataSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // ── Staff ──────────────────────────────────────────────────────────────
            modelBuilder.Entity<Staff>().HasData(
                new Staff { StaffId = 1, FullName = "Ahmed Ali",      Email = "manager@test.com",     RoleType = "Property Manager",   HourlyRate = 25m, SkillType = "Management",    IsAvailable = true },
                new Staff { StaffId = 2, FullName = "Ahmed Qusy",     Email = "maintenance@test.com", RoleType = "Maintenance Staff",  HourlyRate = 15m, SkillType = "Plumbing",      IsAvailable = true },
                new Staff { StaffId = 3, FullName = "Khalid Nasser",  Email = "khalid@test.com",      RoleType = "Maintenance Staff",  HourlyRate = 18m, SkillType = "Electrical",    IsAvailable = true },
                new Staff { StaffId = 4, FullName = "Fatima Hassan",  Email = "fatima@test.com",      RoleType = "Maintenance Staff",  HourlyRate = 16m, SkillType = "HVAC",          IsAvailable = false }
            );

            // ── Properties ────────────────────────────────────────────────────────
            modelBuilder.Entity<Property>().HasData(
                new Property { PropertyId = 1, PropertyName = "Bahrain Polytechnic Residences", Address = "Campus A",        City = "Isa Town",   PropertyType = "Residential", ManagerId = 1 },
                new Property { PropertyId = 2, PropertyName = "Seef Pearl Towers",              Address = "Seef District",   City = "Manama",     PropertyType = "Residential", ManagerId = 1 },
                new Property { PropertyId = 3, PropertyName = "Riffa Business Park",            Address = "Riffa Main Road", City = "Riffa",      PropertyType = "Commercial",  ManagerId = 1 }
            );

            // ── Units ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<Unit>().HasData(
                // Property 1
                new Unit { UnitId = 1, PropertyId = 1, UnitNumber = "A101", RentAmount = 350m,  IsAvailable = true,  Size = "1 Bedroom", Amenities = "WiFi, Parking" },
                new Unit { UnitId = 2, PropertyId = 1, UnitNumber = "A102", RentAmount = 450m,  IsAvailable = false, Size = "2 Bedroom", Amenities = "WiFi, Parking, Pool" },
                new Unit { UnitId = 3, PropertyId = 1, UnitNumber = "A103", RentAmount = 550m,  IsAvailable = false, Size = "3 Bedroom", Amenities = "WiFi, Parking, Gym" },
                new Unit { UnitId = 4, PropertyId = 1, UnitNumber = "A104", RentAmount = 300m,  IsAvailable = true,  Size = "Studio",    Amenities = "WiFi" },
                // Property 2
                new Unit { UnitId = 5, PropertyId = 2, UnitNumber = "T101", RentAmount = 700m,  IsAvailable = false, Size = "2 Bedroom", Amenities = "WiFi, Sea View, Gym" },
                new Unit { UnitId = 6, PropertyId = 2, UnitNumber = "T102", RentAmount = 900m,  IsAvailable = true,  Size = "3 Bedroom", Amenities = "WiFi, Sea View, Pool, Gym" },
                new Unit { UnitId = 7, PropertyId = 2, UnitNumber = "T103", RentAmount = 500m,  IsAvailable = true,  Size = "1 Bedroom", Amenities = "WiFi, Balcony" },
                // Property 3
                new Unit { UnitId = 8, PropertyId = 3, UnitNumber = "B201", RentAmount = 1200m, IsAvailable = false, Size = "Office",     Amenities = "Parking, Security" },
                new Unit { UnitId = 9, PropertyId = 3, UnitNumber = "B202", RentAmount = 800m,  IsAvailable = true,  Size = "Office",     Amenities = "Parking" }
            );

            // ── Tenants ───────────────────────────────────────────────────────────
            modelBuilder.Entity<Tenant>().HasData(
                new Tenant { TenantId = 1, FullName = "Zahra Almosawi", Email = "zahra@test.com",   Phone = "33334444" },
                new Tenant { TenantId = 2, FullName = "Sarah Ahmed",    Email = "sarah@test.com",   Phone = "35556666" },
                new Tenant { TenantId = 3, FullName = "Omar Khalil",    Email = "omar@test.com",    Phone = "39998888" },
                new Tenant { TenantId = 4, FullName = "Noura Jassim",   Email = "noura@test.com",   Phone = "36667777" }
            );

            // ── Applications ──────────────────────────────────────────────────────
            modelBuilder.Entity<Application>().HasData(
                // Zahra: approved → has active lease on Unit 2
                new Application { ApplicationId = 1, TenantId = 1, UnitId = 2, SubmittedAt = new DateTime(2025, 1, 1), ProcessedAt = new DateTime(2025, 1, 3), Status = "LeaseActive", ApprovedByStaffId = 1 },
                // Sarah: approved → has active lease on Unit 5
                new Application { ApplicationId = 2, TenantId = 2, UnitId = 5, SubmittedAt = new DateTime(2025, 2, 1), ProcessedAt = new DateTime(2025, 2, 4), Status = "LeaseActive", ApprovedByStaffId = 1 },
                // Omar: approved → has active lease on Unit 3
                new Application { ApplicationId = 3, TenantId = 3, UnitId = 3, SubmittedAt = new DateTime(2025, 3, 1), ProcessedAt = new DateTime(2025, 3, 5), Status = "LeaseActive", ApprovedByStaffId = 1 },
                // Noura: approved → has active lease on Unit 8
                new Application { ApplicationId = 4, TenantId = 4, UnitId = 8, SubmittedAt = new DateTime(2025, 4, 1), ProcessedAt = new DateTime(2025, 4, 3), Status = "LeaseActive", ApprovedByStaffId = 1 },
                // New applications in pipeline
                new Application { ApplicationId = 5, TenantId = 2, UnitId = 6, SubmittedAt = new DateTime(2026, 5, 1), Status = "Submitted" },
                new Application { ApplicationId = 6, TenantId = 3, UnitId = 7, SubmittedAt = new DateTime(2026, 5, 3), Status = "Screening" },
                new Application { ApplicationId = 7, TenantId = 1, UnitId = 9, SubmittedAt = new DateTime(2026, 4, 20), ProcessedAt = new DateTime(2026, 4, 25), Status = "Rejected" }
            );

            // ── Leases ────────────────────────────────────────────────────────────
            modelBuilder.Entity<Lease>().HasData(
                new Lease { LeaseId = 1, ApplicationId = 1, UnitId = 2, TenantId = 1, StartDate = new DateTime(2025, 1, 5),  EndDate = new DateTime(2026, 1, 4),  MonthlyRent = 450m,  Status = "LeaseActive", CreatedAt = new DateTime(2025, 1, 5) },
                new Lease { LeaseId = 2, ApplicationId = 2, UnitId = 5, TenantId = 2, StartDate = new DateTime(2025, 2, 5),  EndDate = new DateTime(2026, 2, 4),  MonthlyRent = 700m,  Status = "LeaseActive", CreatedAt = new DateTime(2025, 2, 5) },
                new Lease { LeaseId = 3, ApplicationId = 3, UnitId = 3, TenantId = 3, StartDate = new DateTime(2025, 3, 10), EndDate = new DateTime(2026, 3, 9),  MonthlyRent = 550m,  Status = "LeaseActive", CreatedAt = new DateTime(2025, 3, 10) },
                new Lease { LeaseId = 4, ApplicationId = 4, UnitId = 8, TenantId = 4, StartDate = new DateTime(2025, 4, 5),  EndDate = new DateTime(2026, 4, 4),  MonthlyRent = 1200m, Status = "LeaseActive", CreatedAt = new DateTime(2025, 4, 5) }
            );

            // ── Payments ──────────────────────────────────────────────────────────
            modelBuilder.Entity<Payment>().HasData(
                // Lease 1 — Zahra
                new Payment { PaymentId = 1,  LeaseId = 1, Amount = 450m,  PaymentDate = new DateTime(2025, 2, 1),  TransactionTimestamp = new DateTime(2025, 2, 1),  Method = "Bank Transfer", PaymentType = "Monthly Rent", Status = "Paid" },
                new Payment { PaymentId = 2,  LeaseId = 1, Amount = 450m,  PaymentDate = new DateTime(2025, 3, 1),  TransactionTimestamp = new DateTime(2025, 3, 1),  Method = "Bank Transfer", PaymentType = "Monthly Rent", Status = "Paid" },
                new Payment { PaymentId = 3,  LeaseId = 1, Amount = 450m,  PaymentDate = new DateTime(2025, 4, 1),  TransactionTimestamp = new DateTime(2025, 4, 1),  Method = "Cash",          PaymentType = "Monthly Rent", Status = "Paid" },
                new Payment { PaymentId = 4,  LeaseId = 1, Amount = 450m,  PaymentDate = new DateTime(2026, 5, 1),  TransactionTimestamp = new DateTime(2026, 5, 1),  Method = "Cash",          PaymentType = "Monthly Rent", Status = "Overdue" },
                // Lease 2 — Sarah
                new Payment { PaymentId = 5,  LeaseId = 2, Amount = 700m,  PaymentDate = new DateTime(2025, 3, 5),  TransactionTimestamp = new DateTime(2025, 3, 5),  Method = "Bank Transfer", PaymentType = "Monthly Rent", Status = "Paid" },
                new Payment { PaymentId = 6,  LeaseId = 2, Amount = 700m,  PaymentDate = new DateTime(2025, 4, 5),  TransactionTimestamp = new DateTime(2025, 4, 5),  Method = "Bank Transfer", PaymentType = "Monthly Rent", Status = "Paid" },
                new Payment { PaymentId = 7,  LeaseId = 2, Amount = 700m,  PaymentDate = new DateTime(2026, 5, 5),  TransactionTimestamp = new DateTime(2026, 5, 5),  Method = "Online",        PaymentType = "Monthly Rent", Status = "Paid" },
                // Lease 3 — Omar
                new Payment { PaymentId = 8,  LeaseId = 3, Amount = 550m,  PaymentDate = new DateTime(2025, 4, 10), TransactionTimestamp = new DateTime(2025, 4, 10), Method = "Cheque",        PaymentType = "Monthly Rent", Status = "Paid" },
                new Payment { PaymentId = 9,  LeaseId = 3, Amount = 550m,  PaymentDate = new DateTime(2025, 5, 10), TransactionTimestamp = new DateTime(2025, 5, 10), Method = "Cheque",        PaymentType = "Monthly Rent", Status = "Paid" },
                new Payment { PaymentId = 10, LeaseId = 3, Amount = 550m,  PaymentDate = new DateTime(2026, 5, 10), TransactionTimestamp = new DateTime(2026, 5, 10), Method = "Bank Transfer", PaymentType = "Monthly Rent", Status = "Overdue" },
                // Lease 4 — Noura
                new Payment { PaymentId = 11, LeaseId = 4, Amount = 1200m, PaymentDate = new DateTime(2025, 5, 5),  TransactionTimestamp = new DateTime(2025, 5, 5),  Method = "Bank Transfer", PaymentType = "Monthly Rent", Status = "Paid" },
                new Payment { PaymentId = 12, LeaseId = 4, Amount = 1200m, PaymentDate = new DateTime(2026, 5, 5),  TransactionTimestamp = new DateTime(2026, 5, 5),  Method = "Bank Transfer", PaymentType = "Monthly Rent", Status = "Paid" }
            );

            // ── Maintenance Requests ──────────────────────────────────────────────
            modelBuilder.Entity<MaintenanceRequest>().HasData(
                // Closed
                new MaintenanceRequest { RequestId = 1,  UnitId = 2, TenantId = 1, Description = "Air conditioner not cooling properly",           Priority = "High",   ReportedAt = new DateTime(2026, 4, 10), Status = "Closed",     AssignedStaffId = 4 },
                new MaintenanceRequest { RequestId = 2,  UnitId = 2, TenantId = 1, Description = "Kitchen sink is leaking",                        Priority = "Medium", ReportedAt = new DateTime(2026, 4, 25), Status = "Closed",     AssignedStaffId = 2 },
                new MaintenanceRequest { RequestId = 8,  UnitId = 3, TenantId = 3, Description = "Front door lock is jammed",                      Priority = "High",   ReportedAt = new DateTime(2026, 3, 15), Status = "Closed",     AssignedStaffId = 2 },
                new MaintenanceRequest { RequestId = 9,  UnitId = 8, TenantId = 4, Description = "Office ceiling fan stopped working",             Priority = "Low",    ReportedAt = new DateTime(2026, 3, 20), Status = "Closed",     AssignedStaffId = 3 },
                new MaintenanceRequest { RequestId = 10, UnitId = 5, TenantId = 2, Description = "Water heater tripping the circuit breaker",      Priority = "High",   ReportedAt = new DateTime(2026, 4, 1),  Status = "Closed",     AssignedStaffId = 3 },
                // Resolved
                new MaintenanceRequest { RequestId = 3,  UnitId = 5, TenantId = 2, Description = "Bathroom light flickering",                      Priority = "Low",    ReportedAt = new DateTime(2026, 5, 1),  Status = "Resolved",   AssignedStaffId = 3 },
                new MaintenanceRequest { RequestId = 11, UnitId = 3, TenantId = 3, Description = "Shower drain is blocked",                        Priority = "Medium", ReportedAt = new DateTime(2026, 4, 28), Status = "Resolved",   AssignedStaffId = 2 },
                new MaintenanceRequest { RequestId = 12, UnitId = 8, TenantId = 4, Description = "Broken socket in meeting room",                  Priority = "Medium", ReportedAt = new DateTime(2026, 5, 2),  Status = "Resolved",   AssignedStaffId = 3 },
                // InProgress
                new MaintenanceRequest { RequestId = 4,  UnitId = 3, TenantId = 3, Description = "Broken window latch in bedroom",                 Priority = "Medium", ReportedAt = new DateTime(2026, 5, 3),  Status = "InProgress", AssignedStaffId = 2 },
                new MaintenanceRequest { RequestId = 13, UnitId = 2, TenantId = 1, Description = "Balcony sliding door off its track",             Priority = "Medium", ReportedAt = new DateTime(2026, 5, 4),  Status = "InProgress", AssignedStaffId = 2 },
                new MaintenanceRequest { RequestId = 14, UnitId = 8, TenantId = 4, Description = "Air duct making whistling sound",                Priority = "Low",    ReportedAt = new DateTime(2026, 5, 5),  Status = "InProgress", AssignedStaffId = 4 },
                // Assigned
                new MaintenanceRequest { RequestId = 5,  UnitId = 8, TenantId = 4, Description = "HVAC system making loud noise",                  Priority = "High",   ReportedAt = new DateTime(2026, 5, 5),  Status = "Assigned",   AssignedStaffId = 4 },
                new MaintenanceRequest { RequestId = 15, UnitId = 5, TenantId = 2, Description = "Kitchen exhaust fan not working",                Priority = "Low",    ReportedAt = new DateTime(2026, 5, 6),  Status = "Assigned",   AssignedStaffId = 3 },
                new MaintenanceRequest { RequestId = 16, UnitId = 3, TenantId = 3, Description = "Bedroom wardrobe door hinge broken",             Priority = "Low",    ReportedAt = new DateTime(2026, 5, 6),  Status = "Assigned",   AssignedStaffId = 2 },
                // Submitted
                new MaintenanceRequest { RequestId = 6,  UnitId = 2, TenantId = 1, Description = "Hot water not working in the morning",           Priority = "High",   ReportedAt = new DateTime(2026, 5, 6),  Status = "Submitted",  AssignedStaffId = null },
                new MaintenanceRequest { RequestId = 7,  UnitId = 5, TenantId = 2, Description = "Paint peeling off living room wall",             Priority = "Low",    ReportedAt = new DateTime(2026, 5, 7),  Status = "Submitted",  AssignedStaffId = null },
                new MaintenanceRequest { RequestId = 17, UnitId = 8, TenantId = 4, Description = "Main entrance door closer needs adjustment",     Priority = "Medium", ReportedAt = new DateTime(2026, 5, 7),  Status = "Submitted",  AssignedStaffId = null },
                new MaintenanceRequest { RequestId = 18, UnitId = 3, TenantId = 3, Description = "Cockroach infestation in kitchen area",          Priority = "High",   ReportedAt = new DateTime(2026, 5, 7),  Status = "Submitted",  AssignedStaffId = null },
                new MaintenanceRequest { RequestId = 19, UnitId = 2, TenantId = 1, Description = "Bedroom ceiling has water stain — possible leak", Priority = "High",  ReportedAt = new DateTime(2026, 5, 7),  Status = "Submitted",  AssignedStaffId = null }
            );

            // ── Maintenance Logs ──────────────────────────────────────────────────
            modelBuilder.Entity<MaintenanceLog>().HasData(
                // Request 1 — AC (Closed)
                new MaintenanceLog { LogId = 1,  RequestId = 1,  StaffId = 4, ActionTaken = "Inspected AC unit — refrigerant recharge needed",     WorkStarted = new DateTime(2026, 4, 11), WorkCompleted = new DateTime(2026, 4, 11) },
                new MaintenanceLog { LogId = 2,  RequestId = 1,  StaffId = 4, ActionTaken = "Recharged refrigerant and tested cooling",             WorkStarted = new DateTime(2026, 4, 13), WorkCompleted = new DateTime(2026, 4, 13) },
                // Request 2 — Sink (Closed)
                new MaintenanceLog { LogId = 3,  RequestId = 2,  StaffId = 2, ActionTaken = "Replaced faulty pipe connector under sink",            WorkStarted = new DateTime(2026, 4, 26), WorkCompleted = new DateTime(2026, 4, 26) },
                // Request 3 — Light (Resolved)
                new MaintenanceLog { LogId = 4,  RequestId = 3,  StaffId = 3, ActionTaken = "Replaced flickering bulb and inspected wiring",        WorkStarted = new DateTime(2026, 5, 2),  WorkCompleted = new DateTime(2026, 5, 2) },
                // Request 4 — Window latch (InProgress)
                new MaintenanceLog { LogId = 5,  RequestId = 4,  StaffId = 2, ActionTaken = "Inspected window — replacement latch ordered",         WorkStarted = new DateTime(2026, 5, 4),  WorkCompleted = null },
                // Request 8 — Door lock (Closed)
                new MaintenanceLog { LogId = 6,  RequestId = 8,  StaffId = 2, ActionTaken = "Lubricated and realigned door lock mechanism",         WorkStarted = new DateTime(2026, 3, 16), WorkCompleted = new DateTime(2026, 3, 16) },
                // Request 9 — Ceiling fan (Closed)
                new MaintenanceLog { LogId = 7,  RequestId = 9,  StaffId = 3, ActionTaken = "Replaced capacitor on ceiling fan motor",              WorkStarted = new DateTime(2026, 3, 22), WorkCompleted = new DateTime(2026, 3, 22) },
                // Request 10 — Water heater (Closed)
                new MaintenanceLog { LogId = 8,  RequestId = 10, StaffId = 3, ActionTaken = "Replaced faulty thermostat on water heater",           WorkStarted = new DateTime(2026, 4, 3),  WorkCompleted = new DateTime(2026, 4, 3) },
                // Request 11 — Drain (Resolved)
                new MaintenanceLog { LogId = 9,  RequestId = 11, StaffId = 2, ActionTaken = "Cleared blockage using drain snake tool",              WorkStarted = new DateTime(2026, 4, 29), WorkCompleted = new DateTime(2026, 4, 29) },
                // Request 12 — Socket (Resolved)
                new MaintenanceLog { LogId = 10, RequestId = 12, StaffId = 3, ActionTaken = "Replaced broken double socket and tested circuit",     WorkStarted = new DateTime(2026, 5, 3),  WorkCompleted = new DateTime(2026, 5, 3) },
                // Request 13 — Balcony door (InProgress)
                new MaintenanceLog { LogId = 11, RequestId = 13, StaffId = 2, ActionTaken = "Assessed door track — new roller kit required",        WorkStarted = new DateTime(2026, 5, 5),  WorkCompleted = null },
                // Request 14 — Air duct (InProgress)
                new MaintenanceLog { LogId = 12, RequestId = 14, StaffId = 4, ActionTaken = "Inspected ductwork — damper adjustment in progress",   WorkStarted = new DateTime(2026, 5, 6),  WorkCompleted = null }
            );

            // ── Status History ────────────────────────────────────────────────────
            modelBuilder.Entity<StatusHistory>().HasData(
                new StatusHistory { AuditId = 1,  EntityName = "MaintenanceRequest", EntityId = 1, OldStatus = "Submitted",   NewStatus = "Assigned",    ChangedAt = new DateTime(2026, 4, 10) },
                new StatusHistory { AuditId = 2,  EntityName = "MaintenanceRequest", EntityId = 1, OldStatus = "Assigned",    NewStatus = "InProgress",  ChangedAt = new DateTime(2026, 4, 11) },
                new StatusHistory { AuditId = 3,  EntityName = "MaintenanceRequest", EntityId = 1, OldStatus = "InProgress",  NewStatus = "Resolved",    ChangedAt = new DateTime(2026, 4, 13) },
                new StatusHistory { AuditId = 4,  EntityName = "MaintenanceRequest", EntityId = 1, OldStatus = "Resolved",    NewStatus = "Closed",      ChangedAt = new DateTime(2026, 4, 15) },
                new StatusHistory { AuditId = 5,  EntityName = "MaintenanceRequest", EntityId = 2, OldStatus = "Submitted",   NewStatus = "Assigned",    ChangedAt = new DateTime(2026, 4, 25) },
                new StatusHistory { AuditId = 6,  EntityName = "MaintenanceRequest", EntityId = 2, OldStatus = "Assigned",    NewStatus = "InProgress",  ChangedAt = new DateTime(2026, 4, 26) },
                new StatusHistory { AuditId = 7,  EntityName = "MaintenanceRequest", EntityId = 2, OldStatus = "InProgress",  NewStatus = "Resolved",    ChangedAt = new DateTime(2026, 4, 26) },
                new StatusHistory { AuditId = 8,  EntityName = "MaintenanceRequest", EntityId = 3, OldStatus = "Submitted",   NewStatus = "Assigned",    ChangedAt = new DateTime(2026, 5, 1) },
                new StatusHistory { AuditId = 9,  EntityName = "MaintenanceRequest", EntityId = 3, OldStatus = "Assigned",    NewStatus = "InProgress",  ChangedAt = new DateTime(2026, 5, 2) },
                new StatusHistory { AuditId = 10, EntityName = "MaintenanceRequest", EntityId = 4, OldStatus = "Submitted",   NewStatus = "Assigned",    ChangedAt = new DateTime(2026, 5, 3) },
                // New requests
                new StatusHistory { AuditId = 16, EntityName = "MaintenanceRequest", EntityId = 8,  OldStatus = "Submitted",  NewStatus = "Assigned",   ChangedAt = new DateTime(2026, 3, 15) },
                new StatusHistory { AuditId = 17, EntityName = "MaintenanceRequest", EntityId = 8,  OldStatus = "Assigned",   NewStatus = "InProgress", ChangedAt = new DateTime(2026, 3, 16) },
                new StatusHistory { AuditId = 18, EntityName = "MaintenanceRequest", EntityId = 8,  OldStatus = "InProgress", NewStatus = "Resolved",   ChangedAt = new DateTime(2026, 3, 16) },
                new StatusHistory { AuditId = 19, EntityName = "MaintenanceRequest", EntityId = 8,  OldStatus = "Resolved",   NewStatus = "Closed",     ChangedAt = new DateTime(2026, 3, 17) },
                new StatusHistory { AuditId = 20, EntityName = "MaintenanceRequest", EntityId = 9,  OldStatus = "Submitted",  NewStatus = "Assigned",   ChangedAt = new DateTime(2026, 3, 21) },
                new StatusHistory { AuditId = 21, EntityName = "MaintenanceRequest", EntityId = 9,  OldStatus = "Assigned",   NewStatus = "InProgress", ChangedAt = new DateTime(2026, 3, 22) },
                new StatusHistory { AuditId = 22, EntityName = "MaintenanceRequest", EntityId = 9,  OldStatus = "InProgress", NewStatus = "Resolved",   ChangedAt = new DateTime(2026, 3, 22) },
                new StatusHistory { AuditId = 23, EntityName = "MaintenanceRequest", EntityId = 9,  OldStatus = "Resolved",   NewStatus = "Closed",     ChangedAt = new DateTime(2026, 3, 23) },
                new StatusHistory { AuditId = 24, EntityName = "MaintenanceRequest", EntityId = 10, OldStatus = "Submitted",  NewStatus = "Assigned",   ChangedAt = new DateTime(2026, 4, 2)  },
                new StatusHistory { AuditId = 25, EntityName = "MaintenanceRequest", EntityId = 10, OldStatus = "Assigned",   NewStatus = "InProgress", ChangedAt = new DateTime(2026, 4, 3)  },
                new StatusHistory { AuditId = 26, EntityName = "MaintenanceRequest", EntityId = 10, OldStatus = "InProgress", NewStatus = "Resolved",   ChangedAt = new DateTime(2026, 4, 3)  },
                new StatusHistory { AuditId = 27, EntityName = "MaintenanceRequest", EntityId = 10, OldStatus = "Resolved",   NewStatus = "Closed",     ChangedAt = new DateTime(2026, 4, 4)  },
                new StatusHistory { AuditId = 28, EntityName = "MaintenanceRequest", EntityId = 11, OldStatus = "Submitted",  NewStatus = "Assigned",   ChangedAt = new DateTime(2026, 4, 28) },
                new StatusHistory { AuditId = 29, EntityName = "MaintenanceRequest", EntityId = 11, OldStatus = "Assigned",   NewStatus = "InProgress", ChangedAt = new DateTime(2026, 4, 29) },
                new StatusHistory { AuditId = 30, EntityName = "MaintenanceRequest", EntityId = 11, OldStatus = "InProgress", NewStatus = "Resolved",   ChangedAt = new DateTime(2026, 4, 29) },
                new StatusHistory { AuditId = 31, EntityName = "MaintenanceRequest", EntityId = 12, OldStatus = "Submitted",  NewStatus = "Assigned",   ChangedAt = new DateTime(2026, 5, 2)  },
                new StatusHistory { AuditId = 32, EntityName = "MaintenanceRequest", EntityId = 12, OldStatus = "Assigned",   NewStatus = "InProgress", ChangedAt = new DateTime(2026, 5, 3)  },
                new StatusHistory { AuditId = 33, EntityName = "MaintenanceRequest", EntityId = 12, OldStatus = "InProgress", NewStatus = "Resolved",   ChangedAt = new DateTime(2026, 5, 3)  },
                new StatusHistory { AuditId = 34, EntityName = "MaintenanceRequest", EntityId = 13, OldStatus = "Submitted",  NewStatus = "Assigned",   ChangedAt = new DateTime(2026, 5, 4)  },
                new StatusHistory { AuditId = 35, EntityName = "MaintenanceRequest", EntityId = 13, OldStatus = "Assigned",   NewStatus = "InProgress", ChangedAt = new DateTime(2026, 5, 5)  },
                new StatusHistory { AuditId = 36, EntityName = "MaintenanceRequest", EntityId = 14, OldStatus = "Submitted",  NewStatus = "Assigned",   ChangedAt = new DateTime(2026, 5, 5)  },
                new StatusHistory { AuditId = 37, EntityName = "MaintenanceRequest", EntityId = 14, OldStatus = "Assigned",   NewStatus = "InProgress", ChangedAt = new DateTime(2026, 5, 6)  },
                new StatusHistory { AuditId = 38, EntityName = "MaintenanceRequest", EntityId = 15, OldStatus = "Submitted",  NewStatus = "Assigned",   ChangedAt = new DateTime(2026, 5, 6)  },
                new StatusHistory { AuditId = 39, EntityName = "MaintenanceRequest", EntityId = 16, OldStatus = "Submitted",  NewStatus = "Assigned",   ChangedAt = new DateTime(2026, 5, 6)  },
                // Applications
                new StatusHistory { AuditId = 11, EntityName = "Application",        EntityId = 1, OldStatus = "Submitted",   NewStatus = "Screening",   ChangedAt = new DateTime(2025, 1, 2) },
                new StatusHistory { AuditId = 12, EntityName = "Application",        EntityId = 1, OldStatus = "Screening",   NewStatus = "Approved",    ChangedAt = new DateTime(2025, 1, 3) },
                new StatusHistory { AuditId = 13, EntityName = "Application",        EntityId = 1, OldStatus = "Approved",    NewStatus = "LeaseActive", ChangedAt = new DateTime(2025, 1, 5) },
                new StatusHistory { AuditId = 14, EntityName = "Application",        EntityId = 7, OldStatus = "Submitted",   NewStatus = "Screening",   ChangedAt = new DateTime(2026, 4, 22) },
                new StatusHistory { AuditId = 15, EntityName = "Application",        EntityId = 7, OldStatus = "Screening",   NewStatus = "Rejected",    ChangedAt = new DateTime(2026, 4, 25) }
            );
        }
    }
}
