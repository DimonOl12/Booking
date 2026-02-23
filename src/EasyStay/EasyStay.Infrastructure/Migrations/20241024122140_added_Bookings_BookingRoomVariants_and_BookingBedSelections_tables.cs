using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EasyStay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_Bookings_BookingRoomVariants_and_BookingBedSelections_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    DateTo = table.Column<DateOnly>(type: "date", nullable: false),
                    PersonalWishes = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    EstimatedTimeOfArrivalUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AmountToPay = table.Column<decimal>(type: "numeric", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    BankCardId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_BankCards_BankCardId",
                        column: x => x.BankCardId,
                        principalTable: "BankCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Bookings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingRoomVariants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    RoomVariantId = table.Column<long>(type: "bigint", nullable: false),
                    BookingId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingRoomVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingRoomVariants_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingRoomVariants_RoomVariants_RoomVariantId",
                        column: x => x.RoomVariantId,
                        principalTable: "RoomVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingBedSelections",
                columns: table => new
                {
                    BookingRoomVariantId = table.Column<long>(type: "bigint", nullable: false),
                    IsSingleBed = table.Column<bool>(type: "boolean", nullable: false),
                    IsDoubleBed = table.Column<bool>(type: "boolean", nullable: false),
                    IsExtraBed = table.Column<bool>(type: "boolean", nullable: false),
                    IsSofa = table.Column<bool>(type: "boolean", nullable: false),
                    IsKingsizeBed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingBedSelections", x => x.BookingRoomVariantId);
                    table.ForeignKey(
                        name: "FK_BookingBedSelections_BookingRoomVariants_BookingRoomVariant~",
                        column: x => x.BookingRoomVariantId,
                        principalTable: "BookingRoomVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingRoomVariants_BookingId",
                table: "BookingRoomVariants",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRoomVariants_RoomVariantId",
                table: "BookingRoomVariants",
                column: "RoomVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BankCardId",
                table: "Bookings",
                column: "BankCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingBedSelections");

            migrationBuilder.DropTable(
                name: "BookingRoomVariants");

            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
