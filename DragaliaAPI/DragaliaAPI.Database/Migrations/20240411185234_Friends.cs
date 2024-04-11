using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DragaliaAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class Friends : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    ViewerId1 = table.Column<long>(type: "bigint", nullable: false),
                    ViewerId2 = table.Column<long>(type: "bigint", nullable: false),
                    FriendStatus = table.Column<int>(type: "integer", nullable: false),
                    ViewerID1Viewed = table.Column<bool>(type: "boolean", nullable: false),
                    ViewerID2Viewed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => new { x.ViewerId1, x.ViewerId2 });
                    table.ForeignKey(
                        name: "FK_Friends_Players_ViewerId1",
                        column: x => x.ViewerId1,
                        principalTable: "Players",
                        principalColumn: "ViewerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friends_Players_ViewerId2",
                        column: x => x.ViewerId2,
                        principalTable: "Players",
                        principalColumn: "ViewerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friends_ViewerId2",
                table: "Friends",
                column: "ViewerId2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friends");
        }
    }
}
