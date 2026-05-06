using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyLeasingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStaffSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 7, 1, 15, 10, 930, DateTimeKind.Local).AddTicks(3860));

            migrationBuilder.UpdateData(
                table: "MaintenanceLogs",
                keyColumn: "LogId",
                keyValue: 1,
                column: "WorkStarted",
                value: new DateTime(2026, 5, 7, 1, 15, 10, 945, DateTimeKind.Local).AddTicks(3300));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                column: "ReportedAt",
                value: new DateTime(2026, 5, 7, 1, 15, 10, 945, DateTimeKind.Local).AddTicks(2440));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1,
                columns: new[] { "PaymentDate", "TransactionTimestamp" },
                values: new object[] { new DateTime(2026, 5, 7, 1, 15, 10, 945, DateTimeKind.Local).AddTicks(830), new DateTime(2026, 5, 7, 1, 15, 10, 945, DateTimeKind.Local).AddTicks(990) });

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "StaffId",
                keyValue: 1,
                columns: new[] { "IsAvailable", "SkillType" },
                values: new object[] { true, "Management" });

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "StaffId",
                keyValue: 2,
                columns: new[] { "IsAvailable", "SkillType" },
                values: new object[] { true, "Plumbing" });

            migrationBuilder.UpdateData(
                table: "StatusHistories",
                keyColumn: "AuditId",
                keyValue: 1,
                column: "ChangedAt",
                value: new DateTime(2026, 5, 7, 1, 15, 10, 945, DateTimeKind.Local).AddTicks(4370));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 22, 2, 37, 698, DateTimeKind.Local).AddTicks(637));

            migrationBuilder.UpdateData(
                table: "MaintenanceLogs",
                keyColumn: "LogId",
                keyValue: 1,
                column: "WorkStarted",
                value: new DateTime(2026, 5, 6, 22, 2, 37, 699, DateTimeKind.Local).AddTicks(5683));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                column: "ReportedAt",
                value: new DateTime(2026, 5, 6, 22, 2, 37, 699, DateTimeKind.Local).AddTicks(4565));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1,
                columns: new[] { "PaymentDate", "TransactionTimestamp" },
                values: new object[] { new DateTime(2026, 5, 6, 22, 2, 37, 699, DateTimeKind.Local).AddTicks(2604), new DateTime(2026, 5, 6, 22, 2, 37, 699, DateTimeKind.Local).AddTicks(2782) });

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "StaffId",
                keyValue: 1,
                columns: new[] { "IsAvailable", "SkillType" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "StaffId",
                keyValue: 2,
                columns: new[] { "IsAvailable", "SkillType" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "StatusHistories",
                keyColumn: "AuditId",
                keyValue: 1,
                column: "ChangedAt",
                value: new DateTime(2026, 5, 6, 22, 2, 37, 699, DateTimeKind.Local).AddTicks(7090));
        }
    }
}
