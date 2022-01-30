using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace ASP_MVC_NoAuthentication.Migrations
{
    public partial class carPolymorph : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectorPersonalCar");

            migrationBuilder.DropTable(
                name: "PersonalCars");

            migrationBuilder.AddColumn<int>(
                name: "ConnectorId",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Cars",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Cars",
                type: "varchar(767)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ConnectorId",
                table: "Cars",
                column: "ConnectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_UserId",
                table: "Cars",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_AspNetUsers_UserId",
                table: "Cars",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Connectors_ConnectorId",
                table: "Cars",
                column: "ConnectorId",
                principalTable: "Connectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_AspNetUsers_UserId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Connectors_ConnectorId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_ConnectorId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_UserId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "ConnectorId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Cars");

            migrationBuilder.CreateTable(
                name: "PersonalCars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    MaximumDistance = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "varchar(767)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalCars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalCars_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_ConnectorPersonalCar_PersonalCars_PersonalCarsId",
                        column: x => x.PersonalCarsId,
                        principalTable: "PersonalCars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorPersonalCar_PersonalCarsId",
                table: "ConnectorPersonalCar",
                column: "PersonalCarsId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalCars_UserId",
                table: "PersonalCars",
                column: "UserId");
        }
    }
}
