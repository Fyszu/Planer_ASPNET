using Microsoft.EntityFrameworkCore.Migrations;

namespace RoutePlanner.Migrations
{
    public partial class RenameTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarConnectorInterface_Interfaces_ConnectorInterfacesId",
                table: "CarConnectorInterface");

            migrationBuilder.DropForeignKey(
                name: "FK_Connector_ChargingPoints_ChargingPointId",
                table: "Connector");

            migrationBuilder.DropForeignKey(
                name: "FK_ConnectorConnectorInterface_Connector_ConnectorsId",
                table: "ConnectorConnectorInterface");

            migrationBuilder.DropForeignKey(
                name: "FK_ConnectorConnectorInterface_Interfaces_InterfacesId",
                table: "ConnectorConnectorInterface");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interfaces",
                table: "Interfaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Connector",
                table: "Connector");

            migrationBuilder.RenameTable(
                name: "Interfaces",
                newName: "ConnectorInterfaces");

            migrationBuilder.RenameTable(
                name: "Connector",
                newName: "Connectors");

            migrationBuilder.RenameIndex(
                name: "IX_Connector_ChargingPointId",
                table: "Connectors",
                newName: "IX_Connectors_ChargingPointId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConnectorInterfaces",
                table: "ConnectorInterfaces",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Connectors",
                table: "Connectors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarConnectorInterface_ConnectorInterfaces_ConnectorInterface~",
                table: "CarConnectorInterface",
                column: "ConnectorInterfacesId",
                principalTable: "ConnectorInterfaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectorConnectorInterface_ConnectorInterfaces_InterfacesId",
                table: "ConnectorConnectorInterface",
                column: "InterfacesId",
                principalTable: "ConnectorInterfaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectorConnectorInterface_Connectors_ConnectorsId",
                table: "ConnectorConnectorInterface",
                column: "ConnectorsId",
                principalTable: "Connectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Connectors_ChargingPoints_ChargingPointId",
                table: "Connectors",
                column: "ChargingPointId",
                principalTable: "ChargingPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarConnectorInterface_ConnectorInterfaces_ConnectorInterface~",
                table: "CarConnectorInterface");

            migrationBuilder.DropForeignKey(
                name: "FK_ConnectorConnectorInterface_ConnectorInterfaces_InterfacesId",
                table: "ConnectorConnectorInterface");

            migrationBuilder.DropForeignKey(
                name: "FK_ConnectorConnectorInterface_Connectors_ConnectorsId",
                table: "ConnectorConnectorInterface");

            migrationBuilder.DropForeignKey(
                name: "FK_Connectors_ChargingPoints_ChargingPointId",
                table: "Connectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Connectors",
                table: "Connectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConnectorInterfaces",
                table: "ConnectorInterfaces");

            migrationBuilder.RenameTable(
                name: "Connectors",
                newName: "Connector");

            migrationBuilder.RenameTable(
                name: "ConnectorInterfaces",
                newName: "Interfaces");

            migrationBuilder.RenameIndex(
                name: "IX_Connectors_ChargingPointId",
                table: "Connector",
                newName: "IX_Connector_ChargingPointId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Connector",
                table: "Connector",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interfaces",
                table: "Interfaces",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarConnectorInterface_Interfaces_ConnectorInterfacesId",
                table: "CarConnectorInterface",
                column: "ConnectorInterfacesId",
                principalTable: "Interfaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Connector_ChargingPoints_ChargingPointId",
                table: "Connector",
                column: "ChargingPointId",
                principalTable: "ChargingPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectorConnectorInterface_Connector_ConnectorsId",
                table: "ConnectorConnectorInterface",
                column: "ConnectorsId",
                principalTable: "Connector",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectorConnectorInterface_Interfaces_InterfacesId",
                table: "ConnectorConnectorInterface",
                column: "InterfacesId",
                principalTable: "Interfaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
