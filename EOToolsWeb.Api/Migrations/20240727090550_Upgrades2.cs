using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class Upgrades2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Improvments_EquipmentUpgrades_EquipmentUpgradeDataModelId",
                table: "Improvments");

            migrationBuilder.RenameColumn(
                name: "EquipmentUpgradeDataModelId",
                table: "Improvments",
                newName: "EquipmentUpgradeModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Improvments_EquipmentUpgradeDataModelId",
                table: "Improvments",
                newName: "IX_Improvments_EquipmentUpgradeModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Improvments_EquipmentUpgrades_EquipmentUpgradeModelId",
                table: "Improvments",
                column: "EquipmentUpgradeModelId",
                principalTable: "EquipmentUpgrades",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Improvments_EquipmentUpgrades_EquipmentUpgradeModelId",
                table: "Improvments");

            migrationBuilder.RenameColumn(
                name: "EquipmentUpgradeModelId",
                table: "Improvments",
                newName: "EquipmentUpgradeDataModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Improvments_EquipmentUpgradeModelId",
                table: "Improvments",
                newName: "IX_Improvments_EquipmentUpgradeDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Improvments_EquipmentUpgrades_EquipmentUpgradeDataModelId",
                table: "Improvments",
                column: "EquipmentUpgradeDataModelId",
                principalTable: "EquipmentUpgrades",
                principalColumn: "Id");
        }
    }
}
