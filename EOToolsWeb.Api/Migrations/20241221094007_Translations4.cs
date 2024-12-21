using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class Translations4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TranslationModel_QuestTranslationModel_QuestTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropTable(
                name: "QuestTranslationModel");

            migrationBuilder.RenameColumn(
                name: "QuestTranslationModelId",
                table: "TranslationModel",
                newName: "QuestTitleTranslationModelId");

            migrationBuilder.RenameIndex(
                name: "IX_TranslationModel_QuestTranslationModelId",
                table: "TranslationModel",
                newName: "IX_TranslationModel_QuestTitleTranslationModelId");

            migrationBuilder.AddColumn<int>(
                name: "QuestDescriptionTranslationModelId",
                table: "TranslationModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QuestDescriptionTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestDescriptionTranslations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestTitleTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestTitleTranslations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TranslationModel_QuestDescriptionTranslationModelId",
                table: "TranslationModel",
                column: "QuestDescriptionTranslationModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TranslationModel_QuestDescriptionTranslations_QuestDescriptionTranslationModelId",
                table: "TranslationModel",
                column: "QuestDescriptionTranslationModelId",
                principalTable: "QuestDescriptionTranslations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TranslationModel_QuestTitleTranslations_QuestTitleTranslationModelId",
                table: "TranslationModel",
                column: "QuestTitleTranslationModelId",
                principalTable: "QuestTitleTranslations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TranslationModel_QuestDescriptionTranslations_QuestDescriptionTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropForeignKey(
                name: "FK_TranslationModel_QuestTitleTranslations_QuestTitleTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropTable(
                name: "QuestDescriptionTranslations");

            migrationBuilder.DropTable(
                name: "QuestTitleTranslations");

            migrationBuilder.DropIndex(
                name: "IX_TranslationModel_QuestDescriptionTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.DropColumn(
                name: "QuestDescriptionTranslationModelId",
                table: "TranslationModel");

            migrationBuilder.RenameColumn(
                name: "QuestTitleTranslationModelId",
                table: "TranslationModel",
                newName: "QuestTranslationModelId");

            migrationBuilder.RenameIndex(
                name: "IX_TranslationModel_QuestTitleTranslationModelId",
                table: "TranslationModel",
                newName: "IX_TranslationModel_QuestTranslationModelId");

            migrationBuilder.CreateTable(
                name: "QuestTranslationModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestTranslationModel", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TranslationModel_QuestTranslationModel_QuestTranslationModelId",
                table: "TranslationModel",
                column: "QuestTranslationModelId",
                principalTable: "QuestTranslationModel",
                principalColumn: "Id");
        }
    }
}
