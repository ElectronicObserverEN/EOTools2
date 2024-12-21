using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class TranslationsPart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EquipmentTranslationModelId",
                table: "TranslationModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestTranslationModelId",
                table: "TranslationModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EquipmentTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquipmentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTranslations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestTranslations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TranslationModel_EquipmentTranslationModelId",
                table: "TranslationModel",
                column: "EquipmentTranslationModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TranslationModel_QuestTranslationModelId",
                table: "TranslationModel",
                column: "QuestTranslationModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TranslationModel_EquipmentTranslations_EquipmentTranslationModelId",
                table: "TranslationModel",
                column: "EquipmentTranslationModelId",
                principalTable: "EquipmentTranslations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TranslationModel_QuestTranslations_QuestTranslationModelId",
                table: "TranslationModel",
                column: "QuestTranslationModelId",
                principalTable: "QuestTranslations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TranslationModel_EquipmentTranslations_EquipmentTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropForeignKey(
                name: "FK_TranslationModel_QuestTranslations_QuestTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropTable(
                name: "EquipmentTranslations");

            migrationBuilder.DropTable(
                name: "QuestTranslations");

            migrationBuilder.DropIndex(
                name: "IX_TranslationModel_EquipmentTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropIndex(
                name: "IX_TranslationModel_QuestTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropColumn(
                name: "EquipmentTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropColumn(
                name: "QuestTranslationModelId",
                table: "TranslationModel");
        }
    }
}
