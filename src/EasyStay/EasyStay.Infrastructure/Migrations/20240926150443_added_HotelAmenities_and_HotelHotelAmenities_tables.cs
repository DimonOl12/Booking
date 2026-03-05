using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EasyStay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_HotelAmenities_and_HotelHotelAmenities_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "ApartmentNumber",
                table: "Addresses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "Addresses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HotelAmenities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
					Image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
				},
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelAmenities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelHotelAmenities",
                columns: table => new
                {
                    HotelId = table.Column<long>(type: "bigint", nullable: false),
                    HotelAmenityId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelHotelAmenities", x => new { x.HotelId, x.HotelAmenityId });
                    table.ForeignKey(
                        name: "FK_HotelHotelAmenities_HotelAmenities_HotelAmenityId",
                        column: x => x.HotelAmenityId,
                        principalTable: "HotelAmenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelHotelAmenities_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelHotelAmenities_HotelAmenityId",
                table: "HotelHotelAmenities",
                column: "HotelAmenityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelHotelAmenities");

            migrationBuilder.DropTable(
                name: "HotelAmenities");

            migrationBuilder.DropColumn(
                name: "ApartmentNumber",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "Addresses");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Addresses",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Addresses",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
