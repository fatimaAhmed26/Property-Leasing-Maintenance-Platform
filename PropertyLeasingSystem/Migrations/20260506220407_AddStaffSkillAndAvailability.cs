using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyLeasingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffSkillAndAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "StaffMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SkillType",
                table: "StaffMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "StaffMembers");

            migrationBuilder.DropColumn(
                name: "SkillType",
                table: "StaffMembers");
        }
    }
}
