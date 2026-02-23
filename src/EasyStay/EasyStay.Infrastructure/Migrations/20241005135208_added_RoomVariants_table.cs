using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EasyStay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_RoomVariants_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomVariants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    RoomId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomVariants_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BedInfos",
                columns: table => new
                {
                    RoomVariantId = table.Column<long>(type: "bigint", nullable: false),
                    SingleBedCount = table.Column<int>(type: "integer", nullable: false),
                    DoubleBedCount = table.Column<int>(type: "integer", nullable: false),
                    ExtraBedCount = table.Column<int>(type: "integer", nullable: false),
                    SofaCount = table.Column<int>(type: "integer", nullable: false),
                    KingsizeBedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedInfos", x => x.RoomVariantId);
                    table.ForeignKey(
                        name: "FK_BedInfos_RoomVariants_RoomVariantId",
                        column: x => x.RoomVariantId,
                        principalTable: "RoomVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuestInfos",
                columns: table => new
                {
                    RoomVariantId = table.Column<long>(type: "bigint", nullable: false),
                    AdultCount = table.Column<int>(type: "integer", nullable: false),
                    ChildCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestInfos", x => x.RoomVariantId);
                    table.ForeignKey(
                        name: "FK_GuestInfos_RoomVariants_RoomVariantId",
                        column: x => x.RoomVariantId,
                        principalTable: "RoomVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomVariants_RoomId",
                table: "RoomVariants",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BedInfos");

            migrationBuilder.DropTable(
                name: "GuestInfos");

            migrationBuilder.DropTable(
                name: "RoomVariants");
        }
    }
}
