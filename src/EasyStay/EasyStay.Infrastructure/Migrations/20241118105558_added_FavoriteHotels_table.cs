using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyStay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_FavoriteHotels_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteHotels",
                columns: table => new
                {
                    HotelId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteHotels", x => new { x.HotelId, x.CustomerId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteHotels");
        }
    }
}
