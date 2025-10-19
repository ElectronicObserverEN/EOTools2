using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class EqRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentRecipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rank = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipTypes = table.Column<string>(type: "TEXT", nullable: true),
                    Ships = table.Column<string>(type: "TEXT", nullable: true),
                    Fuel = table.Column<int>(type: "INTEGER", nullable: false),
                    Ammo = table.Column<int>(type: "INTEGER", nullable: false),
                    Steel = table.Column<int>(type: "INTEGER", nullable: false),
                    Bauxite = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentRecipes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentRecipes");
        }
    }
}
