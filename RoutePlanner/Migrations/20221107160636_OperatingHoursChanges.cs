using Microsoft.EntityFrameworkCore.Migrations;

namespace RoutePlanner.Migrations
{
    public partial class OperatingHoursChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllTimeOpen",
                table: "ChargingStations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllTimeOpen",
                table: "ChargingStations");
        }
    }
}
