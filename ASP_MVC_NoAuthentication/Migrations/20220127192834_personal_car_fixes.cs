using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP_MVC_NoAuthentication.Migrations
{
    public partial class personal_car_fixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalCar",
                table: "PersonalCar");

            migrationBuilder.RenameTable(
                name: "PersonalCar",
                newName: "PersonalCars");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalCar_UserId",
                table: "PersonalCars",
                newName: "IX_PersonalCars_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalCars",
                table: "PersonalCars",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectorPersonalCar_PersonalCars_PersonalCarsId",
                table: "ConnectorPersonalCar",
                column: "PersonalCarsId",
                principalTable: "PersonalCars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalCars_AspNetUsers_UserId",
                table: "PersonalCars",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectorPersonalCar_PersonalCars_PersonalCarsId",
                table: "ConnectorPersonalCar");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalCars_AspNetUsers_UserId",
                table: "PersonalCars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalCars",
                table: "PersonalCars");

            migrationBuilder.RenameTable(
                name: "PersonalCars",
                newName: "PersonalCar");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalCars_UserId",
                table: "PersonalCar",
                newName: "IX_PersonalCar_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalCar",
                table: "PersonalCar",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectorPersonalCar_PersonalCar_PersonalCarsId",
                table: "ConnectorPersonalCar",
                column: "PersonalCarsId",
                principalTable: "PersonalCar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalCar_AspNetUsers_UserId",
                table: "PersonalCar",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
