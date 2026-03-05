using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EasyStay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updated_Hotels_and_added_Rooms_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_HotelCategories_CategoryId",
                table: "Hotels");

            migrationBuilder.DropTable(
                name: "HotelRentalPeriods");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "NumberOfRooms",
                table: "Hotels");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Hotels",
                newName: "HotelCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Hotels_CategoryId",
                table: "Hotels",
                newName: "IX_Hotels_HotelCategoryId");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ArrivalTimeUtcFrom",
                table: "Hotels",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ArrivalTimeUtcTo",
                table: "Hotels",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DepartureTimeUtcFrom",
                table: "Hotels",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DepartureTimeUtcTo",
                table: "Hotels",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Area = table.Column<double>(type: "double precision", nullable: false),
                    NumberOfRooms = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    HotelId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomRentalPeriods",
                columns: table => new
                {
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    RentalPeriodId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRentalPeriods", x => new { x.RoomId, x.RentalPeriodId });
                    table.ForeignKey(
                        name: "FK_RoomRentalPeriods_RentalPeriods_RentalPeriodId",
                        column: x => x.RentalPeriodId,
                        principalTable: "RentalPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomRentalPeriods_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomRentalPeriods_RentalPeriodId",
                table: "RoomRentalPeriods",
                column: "RentalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HotelId",
                table: "Rooms",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_HotelCategories_HotelCategoryId",
                table: "Hotels",
                column: "HotelCategoryId",
                principalTable: "HotelCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_HotelCategories_HotelCategoryId",
                table: "Hotels");

            migrationBuilder.DropTable(
                name: "RoomRentalPeriods");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropColumn(
                name: "ArrivalTimeUtcFrom",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ArrivalTimeUtcTo",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "DepartureTimeUtcFrom",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "DepartureTimeUtcTo",
                table: "Hotels");

            migrationBuilder.RenameColumn(
                name: "HotelCategoryId",
                table: "Hotels",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Hotels_HotelCategoryId",
                table: "Hotels",
                newName: "IX_Hotels_CategoryId");

            migrationBuilder.AddColumn<double>(
                name: "Area",
                table: "Hotels",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRooms",
                table: "Hotels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_HotelRentalPeriods_RentalPeriodId",
                table: "HotelRentalPeriods",
                column: "RentalPeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_HotelCategories_CategoryId",
                table: "Hotels",
                column: "CategoryId",
                principalTable: "HotelCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
