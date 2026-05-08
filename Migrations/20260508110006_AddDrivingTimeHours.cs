using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetLogic.Migrations
{
    /// <inheritdoc />
    public partial class AddDrivingTimeHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DrivingTimeHours",
                table: "TelemetryLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrivingTimeHours",
                table: "TelemetryLogs");
        }
    }
}
