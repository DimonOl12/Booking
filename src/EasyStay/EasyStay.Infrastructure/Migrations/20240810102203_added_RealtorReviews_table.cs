using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EasyStay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_RealtorReviews_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RealtorReviews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: true),
                    AuthorId = table.Column<long>(type: "bigint", nullable: false),
                    RealtorId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealtorReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RealtorReviews_Customers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RealtorReviews_Realtors_RealtorId",
                        column: x => x.RealtorId,
                        principalTable: "Realtors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RealtorReviews_AuthorId",
                table: "RealtorReviews",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_RealtorReviews_RealtorId",
                table: "RealtorReviews",
                column: "RealtorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RealtorReviews");
        }
    }
}
