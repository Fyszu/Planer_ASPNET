using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace ASP_MVC_NoAuthentication.Migrations
{
    public partial class update05_12_2021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeOfConnector",
                table: "ChargingPoints");

            migrationBuilder.AddColumn<int>(
                name: "PersonalCarId",
                table: "Connectors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConnectorId",
                table: "ChargingPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PersonalCar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    MaximumDistance = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalCar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalCar_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connectors_PersonalCarId",
                table: "Connectors",
                column: "PersonalCarId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingPoints_ConnectorId",
                table: "ChargingPoints",
                column: "ConnectorId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalCar_UserId",
                table: "PersonalCar",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChargingPoints_Connectors_ConnectorId",
                table: "ChargingPoints",
                column: "ConnectorId",
                principalTable: "Connectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Connectors_PersonalCar_PersonalCarId",
                table: "Connectors",
                column: "PersonalCarId",
                principalTable: "PersonalCar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChargingPoints_Connectors_ConnectorId",
                table: "ChargingPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Connectors_PersonalCar_PersonalCarId",
                table: "Connectors");

            migrationBuilder.DropTable(
                name: "PersonalCar");

            migrationBuilder.DropIndex(
                name: "IX_Connectors_PersonalCarId",
                table: "Connectors");

            migrationBuilder.DropIndex(
                name: "IX_ChargingPoints_ConnectorId",
                table: "ChargingPoints");

            migrationBuilder.DropColumn(
                name: "PersonalCarId",
                table: "Connectors");

            migrationBuilder.DropColumn(
                name: "ConnectorId",
                table: "ChargingPoints");

            migrationBuilder.AddColumn<string>(
                name: "TypeOfConnector",
                table: "ChargingPoints",
                type: "text",
                nullable: false);
        }
    }
}
