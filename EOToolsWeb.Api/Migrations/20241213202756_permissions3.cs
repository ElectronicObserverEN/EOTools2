using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOToolsWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class permissions3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "PermissionsPermissionsModel");

            migrationBuilder.AddColumn<int>(
                name: "Kind",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kind",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "PermissionsPermissionsModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CanAdd = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanDelete = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEdit = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanRead = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionsPermissionsModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PermissionsId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatesId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_PermissionsPermissionsModel_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "PermissionsPermissionsModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_PermissionsPermissionsModel_UpdatesId",
                        column: x => x.UpdatesId,
                        principalTable: "PermissionsPermissionsModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_PermissionsId",
                table: "Permissions",
                column: "PermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UpdatesId",
                table: "Permissions",
                column: "UpdatesId");
        }
    }
}
