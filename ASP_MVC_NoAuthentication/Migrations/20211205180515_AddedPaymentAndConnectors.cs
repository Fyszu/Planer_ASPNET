using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace ASP_MVC_NoAuthentication.Migrations
{
    public partial class AddedPaymentAndConnectors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "ChargingStations");

            migrationBuilder.DropColumn(
                name: "Connectors",
                table: "Cars");

            migrationBuilder.AlterColumn<int>(
                name: "StationId",
                table: "ChargingPoints",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Connectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Connectors_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ChargingStationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_ChargingStations_ChargingStationId",
                        column: x => x.ChargingStationId,
                        principalTable: "ChargingStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connectors_CarId",
                table: "Connectors",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_ChargingStationId",
                table: "PaymentMethods",
                column: "ChargingStationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChargingPoints_ChargingStations_StationId",
                table: "ChargingPoints",
                column: "StationId",
                principalTable: "ChargingStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChargingPoints_ChargingStations_StationId",
                table: "ChargingPoints");

            migrationBuilder.DropTable(
                name: "Connectors");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "ChargingStations",
                type: "text",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "StationId",
                table: "ChargingPoints",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Connectors",
                table: "Cars",
                type: "text",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ChargingPoints_ChargingStations_StationId",
                table: "ChargingPoints",
                column: "StationId",
                principalTable: "ChargingStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
