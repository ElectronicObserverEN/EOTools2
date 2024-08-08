using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class MapsTl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FleetNameTranslationModelId",
                table: "TranslationModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MapNameTranslationModelId",
                table: "TranslationModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fleets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fleets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TranslationModel_FleetNameTranslationModelId",
                table: "TranslationModel",
                column: "FleetNameTranslationModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TranslationModel_MapNameTranslationModelId",
                table: "TranslationModel",
                column: "MapNameTranslationModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TranslationModel_Fleets_FleetNameTranslationModelId",
                table: "TranslationModel",
                column: "FleetNameTranslationModelId",
                principalTable: "Fleets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TranslationModel_Maps_MapNameTranslationModelId",
                table: "TranslationModel",
                column: "MapNameTranslationModelId",
                principalTable: "Maps",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TranslationModel_Fleets_FleetNameTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropForeignKey(
                name: "FK_TranslationModel_Maps_MapNameTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropTable(
                name: "Fleets");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropIndex(
                name: "IX_TranslationModel_FleetNameTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropIndex(
                name: "IX_TranslationModel_MapNameTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropColumn(
                name: "FleetNameTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropColumn(
                name: "MapNameTranslationModelId",
                table: "TranslationModel");
        }
    }
}
