using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class ShipsAndEquips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApiId = table.Column<int>(type: "INTEGER", nullable: false),
                    NameJP = table.Column<string>(type: "TEXT", nullable: false),
                    NameEN = table.Column<string>(type: "TEXT", nullable: false),
                    CanBeCrafted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShipClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApiId = table.Column<int>(type: "INTEGER", nullable: false),
                    NameEnglish = table.Column<string>(type: "TEXT", nullable: false),
                    NameJapanese = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameEN = table.Column<string>(type: "TEXT", nullable: false),
                    NameJP = table.Column<string>(type: "TEXT", nullable: false),
                    ApiId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipClassId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ships_ShipClasses_ShipClassId1",
                        column: x => x.ShipClassId1,
                        principalTable: "ShipClasses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ships_ShipClassId1",
                table: "Ships",
                column: "ShipClassId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Ships");

            migrationBuilder.DropTable(
                name: "ShipClasses");
        }
    }
}
