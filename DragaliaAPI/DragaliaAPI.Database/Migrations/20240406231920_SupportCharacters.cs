using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DragaliaAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class SupportCharacters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerSupportChara",
                columns: table => new
                {
                    ViewerId = table.Column<long>(type: "bigint", nullable: false),
                    CharaId = table.Column<int>(type: "integer", nullable: false),
                    EquipDragonKeyId = table.Column<long>(type: "bigint", nullable: false),
                    EquipWeaponBodyId = table.Column<int>(type: "integer", nullable: false),
                    EquipCrestSlotType1CrestId1 = table.Column<int>(type: "integer", nullable: false),
                    EquipCrestSlotType1CrestId2 = table.Column<int>(type: "integer", nullable: false),
                    EquipCrestSlotType1CrestId3 = table.Column<int>(type: "integer", nullable: false),
                    EquipCrestSlotType2CrestId1 = table.Column<int>(type: "integer", nullable: false),
                    EquipCrestSlotType2CrestId2 = table.Column<int>(type: "integer", nullable: false),
                    EquipCrestSlotType3CrestId1 = table.Column<int>(type: "integer", nullable: false),
                    EquipCrestSlotType3CrestId2 = table.Column<int>(type: "integer", nullable: false),
                    EquipTalismanKeyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSupportChara", x => x.ViewerId);
                    table.ForeignKey(
                        name: "FK_PlayerSupportChara_Players_ViewerId",
                        column: x => x.ViewerId,
                        principalTable: "Players",
                        principalColumn: "ViewerId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerSupportChara");
        }
    }
}
