using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class ShipTl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShipSuffixTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipSuffixTranslations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShipTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipTranslations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TranslationModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Translation = table.Column<string>(type: "TEXT", nullable: false),
                    Language = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipNameTranslationModelId = table.Column<int>(type: "INTEGER", nullable: true),
                    ShipSuffixTranslationModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranslationModel_ShipSuffixTranslations_ShipSuffixTranslationModelId",
                        column: x => x.ShipSuffixTranslationModelId,
                        principalTable: "ShipSuffixTranslations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TranslationModel_ShipTranslations_ShipNameTranslationModelId",
                        column: x => x.ShipNameTranslationModelId,
                        principalTable: "ShipTranslations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TranslationModel_ShipNameTranslationModelId",
                table: "TranslationModel",
                column: "ShipNameTranslationModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TranslationModel_ShipSuffixTranslationModelId",
                table: "TranslationModel",
                column: "ShipSuffixTranslationModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TranslationModel");

            migrationBuilder.DropTable(
                name: "ShipSuffixTranslations");

            migrationBuilder.DropTable(
                name: "ShipTranslations");
        }
    }
}
