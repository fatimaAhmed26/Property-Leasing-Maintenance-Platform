using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PropertyLeasing.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SkillType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.StaffId);
                });

            migrationBuilder.CreateTable(
                name: "StatusHistories",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    OldStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusHistories", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    PropertyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.PropertyId);
                    table.ForeignKey(
                        name: "FK_Properties_Staffs_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Staffs",
                        principalColumn: "StaffId");
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    UnitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    UnitNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amenities = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitId);
                    table.ForeignKey(
                        name: "FK_Units_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedByStaffId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_Applications_Staffs_ApprovedByStaffId",
                        column: x => x.ApprovedByStaffId,
                        principalTable: "Staffs",
                        principalColumn: "StaffId");
                    table.ForeignKey(
                        name: "FK_Applications_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedStaffId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Staffs_AssignedStaffId",
                        column: x => x.AssignedStaffId,
                        principalTable: "Staffs",
                        principalColumn: "StaffId");
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Leases",
                columns: table => new
                {
                    LeaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonthlyRent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leases", x => x.LeaseId);
                    table.ForeignKey(
                        name: "FK_Leases_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leases_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leases_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceLogs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    WorkStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkCompleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceLogs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_MaintenanceLogs_MaintenanceRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "MaintenanceRequests",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceLogs_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaseId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Leases_LeaseId",
                        column: x => x.LeaseId,
                        principalTable: "Leases",
                        principalColumn: "LeaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "StaffId", "Email", "FullName", "HourlyRate", "IsAvailable", "RoleType", "SkillType" },
                values: new object[,]
                {
                    { 1, "manager@test.com", "Ahmed Ali", 25m, true, "Property Manager", "Management" },
                    { 2, "maintenance@test.com", "Ahmed Qusy", 15m, true, "Maintenance Staff", "Plumbing" },
                    { 3, "khalid@test.com", "Khalid Nasser", 18m, true, "Maintenance Staff", "Electrical" },
                    { 4, "fatima@test.com", "Fatima Hassan", 16m, false, "Maintenance Staff", "HVAC" }
                });

            migrationBuilder.InsertData(
                table: "StatusHistories",
                columns: new[] { "AuditId", "ChangedAt", "EntityId", "EntityName", "NewStatus", "OldStatus" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "MaintenanceRequest", "Assigned", "Submitted" },
                    { 2, new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "MaintenanceRequest", "InProgress", "Assigned" },
                    { 3, new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "MaintenanceRequest", "Resolved", "InProgress" },
                    { 4, new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "MaintenanceRequest", "Closed", "Resolved" },
                    { 5, new DateTime(2026, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "MaintenanceRequest", "Assigned", "Submitted" },
                    { 6, new DateTime(2026, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "MaintenanceRequest", "InProgress", "Assigned" },
                    { 7, new DateTime(2026, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "MaintenanceRequest", "Resolved", "InProgress" },
                    { 8, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "MaintenanceRequest", "Assigned", "Submitted" },
                    { 9, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "MaintenanceRequest", "InProgress", "Assigned" },
                    { 10, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "MaintenanceRequest", "Assigned", "Submitted" },
                    { 11, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Application", "Screening", "Submitted" },
                    { 12, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Application", "Approved", "Screening" },
                    { 13, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Application", "LeaseActive", "Approved" },
                    { 14, new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Application", "Screening", "Submitted" },
                    { 15, new DateTime(2026, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Application", "Rejected", "Screening" },
                    { 16, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "MaintenanceRequest", "Assigned", "Submitted" },
                    { 17, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "MaintenanceRequest", "InProgress", "Assigned" },
                    { 18, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "MaintenanceRequest", "Resolved", "InProgress" },
                    { 19, new DateTime(2026, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "MaintenanceRequest", "Closed", "Resolved" },
                    { 20, new DateTime(2026, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "MaintenanceRequest", "Assigned", "Submitted" },
                    { 21, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "MaintenanceRequest", "InProgress", "Assigned" },
                    { 22, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "MaintenanceRequest", "Resolved", "InProgress" },
                    { 23, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "MaintenanceRequest", "Closed", "Resolved" },
                    { 24, new DateTime(2026, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "MaintenanceRequest", "Assigned", "Submitted" },
                    { 25, new DateTime(2026, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "MaintenanceRequest", "InProgress", "Assigned" },
                    { 26, new DateTime(2026, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "MaintenanceRequest", "Resolved", "InProgress" },
                    { 27, new DateTime(2026, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "MaintenanceRequest", "Closed", "Resolved" },
                    { 28, new DateTime(2026, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, "MaintenanceRequest", "Assigned", "Submitted" },
                    { 29, new DateTime(2026, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, "MaintenanceRequest", "InProgress", "Assigned" },
                    { 30, new DateTime(2026, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, "MaintenanceRequest", "Resolved", "InProgress" },
                    { 31, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, "MaintenanceRequest", "Assigned", "Submitted" },
                    { 32, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, "MaintenanceRequest", "InProgress", "Assigned" },
                    { 33, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, "MaintenanceRequest", "Resolved", "InProgress" },
                    { 34, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, "MaintenanceRequest", "Assigned", "Submitted" },
                    { 35, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, "MaintenanceRequest", "InProgress", "Assigned" },
                    { 36, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, "MaintenanceRequest", "Assigned", "Submitted" },
                    { 37, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, "MaintenanceRequest", "InProgress", "Assigned" },
                    { 38, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, "MaintenanceRequest", "Assigned", "Submitted" },
                    { 39, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, "MaintenanceRequest", "Assigned", "Submitted" }
                });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "TenantId", "Email", "FullName", "Phone" },
                values: new object[,]
                {
                    { 1, "zahra@test.com", "Zahra Almosawi", "33334444" },
                    { 2, "sarah@test.com", "Sarah Ahmed", "35556666" },
                    { 3, "omar@test.com", "Omar Khalil", "39998888" },
                    { 4, "noura@test.com", "Noura Jassim", "36667777" }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "PropertyId", "Address", "City", "ManagerId", "PropertyName", "PropertyType" },
                values: new object[,]
                {
                    { 1, "Campus A", "Isa Town", 1, "Bahrain Polytechnic Residences", "Residential" },
                    { 2, "Seef District", "Manama", 1, "Seef Pearl Towers", "Residential" },
                    { 3, "Riffa Main Road", "Riffa", 1, "Riffa Business Park", "Commercial" }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "UnitId", "Amenities", "IsAvailable", "PropertyId", "RentAmount", "Size", "UnitNumber" },
                values: new object[,]
                {
                    { 1, "WiFi, Parking", true, 1, 350m, "1 Bedroom", "A101" },
                    { 2, "WiFi, Parking, Pool", false, 1, 450m, "2 Bedroom", "A102" },
                    { 3, "WiFi, Parking, Gym", false, 1, 550m, "3 Bedroom", "A103" },
                    { 4, "WiFi", true, 1, 300m, "Studio", "A104" },
                    { 5, "WiFi, Sea View, Gym", false, 2, 700m, "2 Bedroom", "T101" },
                    { 6, "WiFi, Sea View, Pool, Gym", true, 2, 900m, "3 Bedroom", "T102" },
                    { 7, "WiFi, Balcony", true, 2, 500m, "1 Bedroom", "T103" },
                    { 8, "Parking, Security", false, 3, 1200m, "Office", "B201" },
                    { 9, "Parking", true, 3, 800m, "Office", "B202" }
                });

            migrationBuilder.InsertData(
                table: "Applications",
                columns: new[] { "ApplicationId", "ApprovedByStaffId", "ProcessedAt", "Status", "SubmittedAt", "TenantId", "UnitId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "LeaseActive", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2 },
                    { 2, 1, new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "LeaseActive", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 5 },
                    { 3, 1, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "LeaseActive", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3 },
                    { 4, 1, new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "LeaseActive", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 8 },
                    { 5, null, null, "Submitted", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 6 },
                    { 6, null, null, "Screening", new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 7 },
                    { 7, null, new DateTime(2026, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rejected", new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 9 }
                });

            migrationBuilder.InsertData(
                table: "MaintenanceRequests",
                columns: new[] { "RequestId", "AssignedStaffId", "Description", "Priority", "ReportedAt", "Status", "TenantId", "UnitId" },
                values: new object[,]
                {
                    { 1, 4, "Air conditioner not cooling properly", "High", new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Closed", 1, 2 },
                    { 2, 2, "Kitchen sink is leaking", "Medium", new DateTime(2026, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Closed", 1, 2 },
                    { 3, 3, "Bathroom light flickering", "Low", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resolved", 2, 5 },
                    { 4, 2, "Broken window latch in bedroom", "Medium", new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "InProgress", 3, 3 },
                    { 5, 4, "HVAC system making loud noise", "High", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assigned", 4, 8 },
                    { 6, null, "Hot water not working in the morning", "High", new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Submitted", 1, 2 },
                    { 7, null, "Paint peeling off living room wall", "Low", new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Submitted", 2, 5 },
                    { 8, 2, "Front door lock is jammed", "High", new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Closed", 3, 3 },
                    { 9, 3, "Office ceiling fan stopped working", "Low", new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Closed", 4, 8 },
                    { 10, 3, "Water heater tripping the circuit breaker", "High", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Closed", 2, 5 },
                    { 11, 2, "Shower drain is blocked", "Medium", new DateTime(2026, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resolved", 3, 3 },
                    { 12, 3, "Broken socket in meeting room", "Medium", new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resolved", 4, 8 },
                    { 13, 2, "Balcony sliding door off its track", "Medium", new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "InProgress", 1, 2 },
                    { 14, 4, "Air duct making whistling sound", "Low", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "InProgress", 4, 8 },
                    { 15, 3, "Kitchen exhaust fan not working", "Low", new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assigned", 2, 5 },
                    { 16, 2, "Bedroom wardrobe door hinge broken", "Low", new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assigned", 3, 3 },
                    { 17, null, "Main entrance door closer needs adjustment", "Medium", new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Submitted", 4, 8 },
                    { 18, null, "Cockroach infestation in kitchen area", "High", new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Submitted", 3, 3 },
                    { 19, null, "Bedroom ceiling has water stain — possible leak", "High", new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Submitted", 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "Leases",
                columns: new[] { "LeaseId", "ApplicationId", "CreatedAt", "EndDate", "MonthlyRent", "StartDate", "Status", "TenantId", "UnitId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 450m, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "LeaseActive", 1, 2 },
                    { 2, 2, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 700m, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "LeaseActive", 2, 5 },
                    { 3, 3, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 550m, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "LeaseActive", 3, 3 },
                    { 4, 4, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1200m, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "LeaseActive", 4, 8 }
                });

            migrationBuilder.InsertData(
                table: "MaintenanceLogs",
                columns: new[] { "LogId", "ActionTaken", "RequestId", "StaffId", "WorkCompleted", "WorkStarted" },
                values: new object[,]
                {
                    { 1, "Inspected AC unit — refrigerant recharge needed", 1, 4, new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Recharged refrigerant and tested cooling", 1, 4, new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Replaced faulty pipe connector under sink", 2, 2, new DateTime(2026, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Replaced flickering bulb and inspected wiring", 3, 3, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Inspected window — replacement latch ordered", 4, 2, null, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Lubricated and realigned door lock mechanism", 8, 2, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Replaced capacitor on ceiling fan motor", 9, 3, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "Replaced faulty thermostat on water heater", 10, 3, new DateTime(2026, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "Cleared blockage using drain snake tool", 11, 2, new DateTime(2026, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "Replaced broken double socket and tested circuit", 12, 3, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, "Assessed door track — new roller kit required", 13, 2, null, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, "Inspected ductwork — damper adjustment in progress", 14, 4, null, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "LeaseId", "Method", "PaymentDate", "PaymentType", "Status", "TransactionTimestamp" },
                values: new object[,]
                {
                    { 1, 450m, 1, "Bank Transfer", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Monthly Rent", "Paid", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 450m, 1, "Bank Transfer", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Monthly Rent", "Paid", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 450m, 1, "Cash", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Monthly Rent", "Paid", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 450m, 1, "Cash", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Monthly Rent", "Overdue", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 700m, 2, "Bank Transfer", new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Monthly Rent", "Paid", new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 700m, 2, "Bank Transfer", new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Monthly Rent", "Paid", new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 700m, 2, "Online", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Monthly Rent", "Paid", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 550m, 3, "Cheque", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Monthly Rent", "Paid", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 550m, 3, "Cheque", new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Monthly Rent", "Paid", new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 550m, 3, "Bank Transfer", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Monthly Rent", "Overdue", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 1200m, 4, "Bank Transfer", new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Monthly Rent", "Paid", new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 1200m, 4, "Bank Transfer", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Monthly Rent", "Paid", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApprovedByStaffId",
                table: "Applications",
                column: "ApprovedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_TenantId",
                table: "Applications",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_UnitId",
                table: "Applications",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_ApplicationId",
                table: "Leases",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_TenantId",
                table: "Leases",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_UnitId",
                table: "Leases",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceLogs_RequestId",
                table: "MaintenanceLogs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceLogs_StaffId",
                table: "MaintenanceLogs",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_AssignedStaffId",
                table: "MaintenanceRequests",
                column: "AssignedStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_TenantId",
                table: "MaintenanceRequests",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_UnitId",
                table: "MaintenanceRequests",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_LeaseId",
                table: "Payments",
                column: "LeaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ManagerId",
                table: "Properties",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_PropertyId",
                table: "Units",
                column: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "MaintenanceLogs");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "StatusHistories");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MaintenanceRequests");

            migrationBuilder.DropTable(
                name: "Leases");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Staffs");
        }
    }
}
