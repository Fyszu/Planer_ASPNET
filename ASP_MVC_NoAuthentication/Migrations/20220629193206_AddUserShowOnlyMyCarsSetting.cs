using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP_MVC_NoAuthentication.Migrations
{
    public partial class AddUserShowOnlyMyCarsSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowOnlyMyCars",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowOnlyMyCars",
                table: "AspNetUsers");
        }
    }
}
