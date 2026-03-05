using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EasyStay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HotelType_replaced_by_HotelCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_HotelTypes_TypeId",
                table: "Hotels");

            migrationBuilder.DropTable(
                name: "HotelTypes");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Hotels",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Hotels_TypeId",
                table: "Hotels",
                newName: "IX_Hotels_CategoryId");

            migrationBuilder.CreateTable(
                name: "HotelCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelCategories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_HotelCategories_CategoryId",
                table: "Hotels",
                column: "CategoryId",
                principalTable: "HotelCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_HotelCategories_CategoryId",
                table: "Hotels");

            migrationBuilder.DropTable(
                name: "HotelCategories");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Hotels",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Hotels_CategoryId",
                table: "Hotels",
                newName: "IX_Hotels_TypeId");

            migrationBuilder.CreateTable(
                name: "HotelTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_HotelTypes_TypeId",
                table: "Hotels",
                column: "TypeId",
                principalTable: "HotelTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
