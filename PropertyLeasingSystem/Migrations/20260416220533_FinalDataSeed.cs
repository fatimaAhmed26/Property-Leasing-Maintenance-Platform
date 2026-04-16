using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyLeasingSystem.Migrations
{
    /// <inheritdoc />
    public partial class FinalDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "PropertyId", "Address", "City", "ManagerId", "PropertyName", "PropertyType" },
                values: new object[] { 1, "Isa Town", "Manama", 1, "Polytechnic Towers", "Apartment" });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "TenantId", "Email", "FullName", "Phone" },
                values: new object[] { 1, "abrar@example.com", "Abrar Al-Awadhi", "12345678" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "PropertyId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 1);
        }
    }
}
