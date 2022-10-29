using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP_MVC_NoAuthentication.Migrations
{
    public partial class UserChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrivingStyle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HighwaySpeed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SummerFactor",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WinterFactor",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DrivingStyle",
                table: "AspNetUsers",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "HighwaySpeed",
                table: "AspNetUsers",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<double>(
                name: "SummerFactor",
                table: "AspNetUsers",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WinterFactor",
                table: "AspNetUsers",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
