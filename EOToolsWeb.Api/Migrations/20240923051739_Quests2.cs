using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class Quests2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameJp",
                table: "Quests",
                newName: "NameJP");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Quests",
                newName: "NameEN");

            migrationBuilder.RenameColumn(
                name: "DescJp",
                table: "Quests",
                newName: "DescJP");

            migrationBuilder.RenameColumn(
                name: "DescEn",
                table: "Quests",
                newName: "DescEN");

            migrationBuilder.RenameColumn(
                name: "TrackerData",
                table: "Quests",
                newName: "Tracker");

            migrationBuilder.RenameColumn(
                name: "QuestId",
                table: "Quests",
                newName: "ApiId");

            migrationBuilder.AddColumn<int>(
                name: "AddedOnUpdateId",
                table: "Quests",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemovedOnUpdateId",
                table: "Quests",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeasonId",
                table: "Quests",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quests_Code_ApiId",
                table: "Quests",
                columns: new[] { "Code", "ApiId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quests_Code_ApiId",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "AddedOnUpdateId",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "RemovedOnUpdateId",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "Quests");

            migrationBuilder.RenameColumn(
                name: "NameJP",
                table: "Quests",
                newName: "NameJp");

            migrationBuilder.RenameColumn(
                name: "NameEN",
                table: "Quests",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "DescJP",
                table: "Quests",
                newName: "DescJp");

            migrationBuilder.RenameColumn(
                name: "DescEN",
                table: "Quests",
                newName: "DescEn");

            migrationBuilder.RenameColumn(
                name: "Tracker",
                table: "Quests",
                newName: "TrackerData");

            migrationBuilder.RenameColumn(
                name: "ApiId",
                table: "Quests",
                newName: "QuestId");
        }
    }
}
