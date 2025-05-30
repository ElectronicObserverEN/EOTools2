using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExtraCosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EquipmentUpgradeExtraCostId",
                table: "EquipmentUpgradeImprovmentCostItemDetail",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeExtraCost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquipmentUpgradeImprovmentCostId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeExtraCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeExtraCost_EquipmentUpgradeImprovmentCost_EquipmentUpgradeImprovmentCostId",
                        column: x => x.EquipmentUpgradeImprovmentCostId,
                        principalTable: "EquipmentUpgradeImprovmentCost",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeExtraCostLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    CostId = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentUpgradeExtraCostId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeExtraCostLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeExtraCostLevel_EquipmentUpgradeExtraCost_EquipmentUpgradeExtraCostId",
                        column: x => x.EquipmentUpgradeExtraCostId,
                        principalTable: "EquipmentUpgradeExtraCost",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeExtraCostLevel_EquipmentUpgradeImprovmentCostDetail_CostId",
                        column: x => x.CostId,
                        principalTable: "EquipmentUpgradeImprovmentCostDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeExtraCostId",
                table: "EquipmentUpgradeImprovmentCostItemDetail",
                column: "EquipmentUpgradeExtraCostId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeExtraCost_EquipmentUpgradeImprovmentCostId",
                table: "EquipmentUpgradeExtraCost",
                column: "EquipmentUpgradeImprovmentCostId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeExtraCostLevel_CostId",
                table: "EquipmentUpgradeExtraCostLevel",
                column: "CostId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeExtraCostLevel_EquipmentUpgradeExtraCostId",
                table: "EquipmentUpgradeExtraCostLevel",
                column: "EquipmentUpgradeExtraCostId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeExtraCost_EquipmentUpgradeExtraCostId",
                table: "EquipmentUpgradeImprovmentCostItemDetail",
                column: "EquipmentUpgradeExtraCostId",
                principalTable: "EquipmentUpgradeExtraCost",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeExtraCost_EquipmentUpgradeExtraCostId",
                table: "EquipmentUpgradeImprovmentCostItemDetail");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeExtraCostLevel");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeExtraCost");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeExtraCostId",
                table: "EquipmentUpgradeImprovmentCostItemDetail");

            migrationBuilder.DropColumn(
                name: "EquipmentUpgradeExtraCostId",
                table: "EquipmentUpgradeImprovmentCostItemDetail");
        }
    }
}
