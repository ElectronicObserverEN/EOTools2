using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class Upgrades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeImprovmentCostDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DevmatCost = table.Column<int>(type: "INTEGER", nullable: false),
                    SliderDevmatCost = table.Column<int>(type: "INTEGER", nullable: false),
                    ImproveMatCost = table.Column<int>(type: "INTEGER", nullable: false),
                    SliderImproveMatCost = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeImprovmentCostDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquipmentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgrades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeImprovmentCost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fuel = table.Column<int>(type: "INTEGER", nullable: false),
                    Ammo = table.Column<int>(type: "INTEGER", nullable: false),
                    Steel = table.Column<int>(type: "INTEGER", nullable: false),
                    Bauxite = table.Column<int>(type: "INTEGER", nullable: false),
                    Cost0To5Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Cost6To9Id = table.Column<int>(type: "INTEGER", nullable: false),
                    CostMaxId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeImprovmentCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeImprovmentCost_EquipmentUpgradeImprovmentCostDetail_Cost0To5Id",
                        column: x => x.Cost0To5Id,
                        principalTable: "EquipmentUpgradeImprovmentCostDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeImprovmentCost_EquipmentUpgradeImprovmentCostDetail_Cost6To9Id",
                        column: x => x.Cost6To9Id,
                        principalTable: "EquipmentUpgradeImprovmentCostDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeImprovmentCost_EquipmentUpgradeImprovmentCostDetail_CostMaxId",
                        column: x => x.CostMaxId,
                        principalTable: "EquipmentUpgradeImprovmentCostDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeImprovmentCostItemDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentUpgradeImprovmentCostDetailId = table.Column<int>(type: "INTEGER", nullable: true),
                    EquipmentUpgradeImprovmentCostDetailId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeImprovmentCostItemDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeImprovmentCostDetail_EquipmentUpgradeImprovmentCostDetailId",
                        column: x => x.EquipmentUpgradeImprovmentCostDetailId,
                        principalTable: "EquipmentUpgradeImprovmentCostDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeImprovmentCostDetail_EquipmentUpgradeImprovmentCostDetailId1",
                        column: x => x.EquipmentUpgradeImprovmentCostDetailId1,
                        principalTable: "EquipmentUpgradeImprovmentCostDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Improvments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CostsId = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentUpgradeDataModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Improvments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Improvments_EquipmentUpgradeImprovmentCost_CostsId",
                        column: x => x.CostsId,
                        principalTable: "EquipmentUpgradeImprovmentCost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Improvments_EquipmentUpgrades_EquipmentUpgradeDataModelId",
                        column: x => x.EquipmentUpgradeDataModelId,
                        principalTable: "EquipmentUpgrades",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeConversionModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImprovmentModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    IdEquipmentAfter = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentLevelAfter = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeConversionModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeConversionModel_Improvments_ImprovmentModelId",
                        column: x => x.ImprovmentModelId,
                        principalTable: "Improvments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeHelpersModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquipmentUpgradeImprovmentModelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeHelpersModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeHelpersModel_Improvments_EquipmentUpgradeImprovmentModelId",
                        column: x => x.EquipmentUpgradeImprovmentModelId,
                        principalTable: "Improvments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeHelpersDayModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Day = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentUpgradeHelpersModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeHelpersDayModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeHelpersDayModel_EquipmentUpgradeHelpersModel_EquipmentUpgradeHelpersModelId",
                        column: x => x.EquipmentUpgradeHelpersModelId,
                        principalTable: "EquipmentUpgradeHelpersModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeHelpersShipModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShipId = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentUpgradeHelpersModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeHelpersShipModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeHelpersShipModel_EquipmentUpgradeHelpersModel_EquipmentUpgradeHelpersModelId",
                        column: x => x.EquipmentUpgradeHelpersModelId,
                        principalTable: "EquipmentUpgradeHelpersModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeConversionModel_ImprovmentModelId",
                table: "EquipmentUpgradeConversionModel",
                column: "ImprovmentModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeHelpersDayModel_EquipmentUpgradeHelpersModelId",
                table: "EquipmentUpgradeHelpersDayModel",
                column: "EquipmentUpgradeHelpersModelId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeHelpersModel_EquipmentUpgradeImprovmentModelId",
                table: "EquipmentUpgradeHelpersModel",
                column: "EquipmentUpgradeImprovmentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeHelpersShipModel_EquipmentUpgradeHelpersModelId",
                table: "EquipmentUpgradeHelpersShipModel",
                column: "EquipmentUpgradeHelpersModelId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentCost_Cost0To5Id",
                table: "EquipmentUpgradeImprovmentCost",
                column: "Cost0To5Id");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentCost_Cost6To9Id",
                table: "EquipmentUpgradeImprovmentCost",
                column: "Cost6To9Id");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentCost_CostMaxId",
                table: "EquipmentUpgradeImprovmentCost",
                column: "CostMaxId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeImprovmentCostDetailId",
                table: "EquipmentUpgradeImprovmentCostItemDetail",
                column: "EquipmentUpgradeImprovmentCostDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeImprovmentCostDetailId1",
                table: "EquipmentUpgradeImprovmentCostItemDetail",
                column: "EquipmentUpgradeImprovmentCostDetailId1");

            migrationBuilder.CreateIndex(
                name: "IX_Improvments_CostsId",
                table: "Improvments",
                column: "CostsId");

            migrationBuilder.CreateIndex(
                name: "IX_Improvments_EquipmentUpgradeDataModelId",
                table: "Improvments",
                column: "EquipmentUpgradeDataModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentUpgradeConversionModel");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeHelpersDayModel");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeHelpersShipModel");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeImprovmentCostItemDetail");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeHelpersModel");

            migrationBuilder.DropTable(
                name: "Improvments");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeImprovmentCost");

            migrationBuilder.DropTable(
                name: "EquipmentUpgrades");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeImprovmentCostDetail");
        }
    }
}
