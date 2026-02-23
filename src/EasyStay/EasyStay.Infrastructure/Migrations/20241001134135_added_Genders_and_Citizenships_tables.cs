using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EasyStay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_Genders_and_Citizenships_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Realtors",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CitizenshipId",
                table: "Realtors",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CityId",
                table: "Realtors",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Realtors",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Realtors",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GenderId",
                table: "Realtors",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Citizenships",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizenships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Realtors_CitizenshipId",
                table: "Realtors",
                column: "CitizenshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Realtors_CityId",
                table: "Realtors",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Realtors_GenderId",
                table: "Realtors",
                column: "GenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Realtors_Cities_CityId",
                table: "Realtors",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Realtors_Citizenships_CitizenshipId",
                table: "Realtors",
                column: "CitizenshipId",
                principalTable: "Citizenships",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Realtors_Genders_GenderId",
                table: "Realtors",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Realtors_Cities_CityId",
                table: "Realtors");

            migrationBuilder.DropForeignKey(
                name: "FK_Realtors_Citizenships_CitizenshipId",
                table: "Realtors");

            migrationBuilder.DropForeignKey(
                name: "FK_Realtors_Genders_GenderId",
                table: "Realtors");

            migrationBuilder.DropTable(
                name: "Citizenships");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropIndex(
                name: "IX_Realtors_CitizenshipId",
                table: "Realtors");

            migrationBuilder.DropIndex(
                name: "IX_Realtors_CityId",
                table: "Realtors");

            migrationBuilder.DropIndex(
                name: "IX_Realtors_GenderId",
                table: "Realtors");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Realtors");

            migrationBuilder.DropColumn(
                name: "CitizenshipId",
                table: "Realtors");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Realtors");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Realtors");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Realtors");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "Realtors");
        }
    }
}
