using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExtraEqCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EquipmentUpgradeExtraCostId1",
                table: "EquipmentUpgradeImprovmentCostItemDetail",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeExtraCostId1",
                table: "EquipmentUpgradeImprovmentCostItemDetail",
                column: "EquipmentUpgradeExtraCostId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeExtraCost_EquipmentUpgradeExtraCostId1",
                table: "EquipmentUpgradeImprovmentCostItemDetail",
                column: "EquipmentUpgradeExtraCostId1",
                principalTable: "EquipmentUpgradeExtraCost",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeExtraCost_EquipmentUpgradeExtraCostId1",
                table: "EquipmentUpgradeImprovmentCostItemDetail");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeExtraCostId1",
                table: "EquipmentUpgradeImprovmentCostItemDetail");

            migrationBuilder.DropColumn(
                name: "EquipmentUpgradeExtraCostId1",
                table: "EquipmentUpgradeImprovmentCostItemDetail");
        }
    }
}
