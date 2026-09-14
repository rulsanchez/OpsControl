using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpsControl.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIncidetnClassification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Impact",
                table: "Incidents",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Incidents",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "Urgency",
                table: "Incidents",
                type: "int",
                nullable: false,
                defaultValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Impact",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "Urgency",
                table: "Incidents");
        }
    }
}
