using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpsControl.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIncidentResolvedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ResolvedAt",
                table: "Incidents",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResolvedAt",
                table: "Incidents");
        }
    }
}
