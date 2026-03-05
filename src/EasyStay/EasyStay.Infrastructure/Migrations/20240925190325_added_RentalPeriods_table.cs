using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EasyStay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_RentalPeriods_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Hotels_AddressId",
                table: "Hotels");

            migrationBuilder.CreateTable(
                name: "RentalPeriods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelRentalPeriods",
                columns: table => new
                {
                    HotelId = table.Column<long>(type: "bigint", nullable: false),
                    RentalPeriodId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRentalPeriods", x => new { x.HotelId, x.RentalPeriodId });
                    table.ForeignKey(
                        name: "FK_HotelRentalPeriods_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelRentalPeriods_RentalPeriods_RentalPeriodId",
                        column: x => x.RentalPeriodId,
                        principalTable: "RentalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_AddressId",
                table: "Hotels",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelRentalPeriods_RentalPeriodId",
                table: "HotelRentalPeriods",
                column: "RentalPeriodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelRentalPeriods");

            migrationBuilder.DropTable(
                name: "RentalPeriods");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_AddressId",
                table: "Hotels");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_AddressId",
                table: "Hotels",
                column: "AddressId");
        }
    }
}
