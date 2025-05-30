using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExtraCosts2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentUpgradeExtraCostLevel_EquipmentUpgradeImprovmentCostDetail_CostId",
                table: "EquipmentUpgradeExtraCostLevel");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentUpgradeExtraCostLevel_CostId",
                table: "EquipmentUpgradeExtraCostLevel");

            migrationBuilder.DropColumn(
                name: "CostId",
                table: "EquipmentUpgradeExtraCostLevel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CostId",
                table: "EquipmentUpgradeExtraCostLevel",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeExtraCostLevel_CostId",
                table: "EquipmentUpgradeExtraCostLevel",
                column: "CostId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentUpgradeExtraCostLevel_EquipmentUpgradeImprovmentCostDetail_CostId",
                table: "EquipmentUpgradeExtraCostLevel",
                column: "CostId",
                principalTable: "EquipmentUpgradeImprovmentCostDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
