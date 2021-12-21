using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP_MVC_NoAuthentication.Migrations
{
    public partial class FixedPaymentAndConnectors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_ChargingStationId",
                table: "PaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_Connectors_CarId",
                table: "Connectors");

            migrationBuilder.DropColumn(
                name: "ChargingStationId",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Connectors");

            migrationBuilder.CreateTable(
                name: "CarConnector",
                columns: table => new
                {
                    CarsId = table.Column<int>(type: "int", nullable: false),
                    ConnectorsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarConnector", x => new { x.CarsId, x.ConnectorsId });
                    table.ForeignKey(
                        name: "FK_CarConnector_Cars_CarsId",
                        column: x => x.CarsId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarConnector_Connectors_ConnectorsId",
                        column: x => x.ConnectorsId,
                        principalTable: "Connectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChargingStationPaymentMethod",
                columns: table => new
                {
                    ChargingStationsId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethodsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargingStationPaymentMethod", x => new { x.ChargingStationsId, x.PaymentMethodsId });
                    table.ForeignKey(
                        name: "FK_ChargingStationPaymentMethod_ChargingStations_ChargingStatio~",
                        column: x => x.ChargingStationsId,
                        principalTable: "ChargingStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChargingStationPaymentMethod_PaymentMethods_PaymentMethodsId",
                        column: x => x.PaymentMethodsId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarConnector_ConnectorsId",
                table: "CarConnector",
                column: "ConnectorsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationPaymentMethod_PaymentMethodsId",
                table: "ChargingStationPaymentMethod",
                column: "PaymentMethodsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarConnector");

            migrationBuilder.DropTable(
                name: "ChargingStationPaymentMethod");

            migrationBuilder.AddColumn<int>(
                name: "ChargingStationId",
                table: "PaymentMethods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Connectors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_ChargingStationId",
                table: "PaymentMethods",
                column: "ChargingStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Connectors_CarId",
                table: "Connectors",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connectors_Cars_CarId",
                table: "Connectors",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_ChargingStations_ChargingStationId",
                table: "PaymentMethods",
                column: "ChargingStationId",
                principalTable: "ChargingStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
