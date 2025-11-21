using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROGPOE3.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmployeeAddNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "Claims");

            migrationBuilder.AddColumn<string>(
                name: "SupportingDocument",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportingDocument",
                table: "Claims");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
