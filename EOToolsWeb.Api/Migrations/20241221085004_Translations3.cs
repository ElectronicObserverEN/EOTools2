using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class Translations3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TranslationModel_QuestTranslations_QuestTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestTranslations",
                table: "QuestTranslations");

            migrationBuilder.RenameTable(
                name: "QuestTranslations",
                newName: "QuestTranslationModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestTranslationModel",
                table: "QuestTranslationModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TranslationModel_QuestTranslationModel_QuestTranslationModelId",
                table: "TranslationModel",
                column: "QuestTranslationModelId",
                principalTable: "QuestTranslationModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TranslationModel_QuestTranslationModel_QuestTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestTranslationModel",
                table: "QuestTranslationModel");

            migrationBuilder.RenameTable(
                name: "QuestTranslationModel",
                newName: "QuestTranslations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestTranslations",
                table: "QuestTranslations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TranslationModel_QuestTranslations_QuestTranslationModelId",
                table: "TranslationModel",
                column: "QuestTranslationModelId",
                principalTable: "QuestTranslations",
                principalColumn: "Id");
        }
    }
}
