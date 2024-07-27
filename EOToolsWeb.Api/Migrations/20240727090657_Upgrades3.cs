using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class Upgrades3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Improvments_EquipmentUpgrades_EquipmentUpgradeModelId",
                table: "Improvments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EquipmentUpgrades",
                table: "EquipmentUpgrades");

            migrationBuilder.RenameTable(
                name: "EquipmentUpgrades",
                newName: "EquipmentUpgradeDataModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EquipmentUpgradeDataModel",
                table: "EquipmentUpgradeDataModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Improvments_EquipmentUpgradeDataModel_EquipmentUpgradeModelId",
                table: "Improvments",
                column: "EquipmentUpgradeModelId",
                principalTable: "EquipmentUpgradeDataModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Improvments_EquipmentUpgradeDataModel_EquipmentUpgradeModelId",
                table: "Improvments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EquipmentUpgradeDataModel",
                table: "EquipmentUpgradeDataModel");

            migrationBuilder.RenameTable(
                name: "EquipmentUpgradeDataModel",
                newName: "EquipmentUpgrades");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EquipmentUpgrades",
                table: "EquipmentUpgrades",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Improvments_EquipmentUpgrades_EquipmentUpgradeModelId",
                table: "Improvments",
                column: "EquipmentUpgradeModelId",
                principalTable: "EquipmentUpgrades",
                principalColumn: "Id");
        }
    }
}
