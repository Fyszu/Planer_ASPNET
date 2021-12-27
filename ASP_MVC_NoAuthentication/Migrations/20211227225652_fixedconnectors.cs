using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP_MVC_NoAuthentication.Migrations
{
    public partial class fixedconnectors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Connectors_PersonalCarId",
                table: "Connectors");

            migrationBuilder.DropColumn(
                name: "PersonalCarId",
                table: "Connectors");

            migrationBuilder.CreateTable(
                name: "ConnectorPersonalCar",
                columns: table => new
                {
                    ConnectorsId = table.Column<int>(type: "int", nullable: false),
                    PersonalCarsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectorPersonalCar", x => new { x.ConnectorsId, x.PersonalCarsId });
                    table.ForeignKey(
                        name: "FK_ConnectorPersonalCar_Connectors_ConnectorsId",
                        column: x => x.ConnectorsId,
                        principalTable: "Connectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectorPersonalCar_PersonalCar_PersonalCarsId",
                        column: x => x.PersonalCarsId,
                        principalTable: "PersonalCar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorPersonalCar_PersonalCarsId",
                table: "ConnectorPersonalCar",
                column: "PersonalCarsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectorPersonalCar");

            migrationBuilder.AddColumn<int>(
                name: "PersonalCarId",
                table: "Connectors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Connectors_PersonalCarId",
                table: "Connectors",
                column: "PersonalCarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connectors_PersonalCar_PersonalCarId",
                table: "Connectors",
                column: "PersonalCarId",
                principalTable: "PersonalCar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
