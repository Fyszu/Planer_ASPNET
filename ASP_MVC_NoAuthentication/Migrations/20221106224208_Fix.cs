using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP_MVC_NoAuthentication.Migrations
{
    public partial class Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarConnectorInterface_Cars_CarsId",
                table: "CarConnectorInterface");

            migrationBuilder.DropForeignKey(
                name: "FK_CarConnectorInterface_Interfaces_ConnectorInterfacesId",
                table: "CarConnectorInterface");

            migrationBuilder.AddForeignKey(
                name: "FK_CarConnectorInterface_Cars_CarsId",
                table: "CarConnectorInterface",
                column: "CarsId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarConnectorInterface_Interfaces_ConnectorInterfacesId",
                table: "CarConnectorInterface",
                column: "ConnectorInterfacesId",
                principalTable: "Interfaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarConnectorInterface_Cars_CarsId",
                table: "CarConnectorInterface");

            migrationBuilder.DropForeignKey(
                name: "FK_CarConnectorInterface_Interfaces_ConnectorInterfacesId",
                table: "CarConnectorInterface");

            migrationBuilder.AddForeignKey(
                name: "FK_CarConnectorInterface_Cars_CarsId",
                table: "CarConnectorInterface",
                column: "CarsId",
                principalTable: "Cars",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarConnectorInterface_Interfaces_ConnectorInterfacesId",
                table: "CarConnectorInterface",
                column: "ConnectorInterfacesId",
                principalTable: "Interfaces",
                principalColumn: "Id");
        }
    }
}
