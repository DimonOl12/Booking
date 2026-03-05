using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyStay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_personal_info_columns_to_Customers_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Customers",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CitizenshipId",
                table: "Customers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CityId",
                table: "Customers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Customers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GenderId",
                table: "Customers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CitizenshipId",
                table: "Customers",
                column: "CitizenshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CityId",
                table: "Customers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_GenderId",
                table: "Customers",
                column: "GenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Cities_CityId",
                table: "Customers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Citizenships_CitizenshipId",
                table: "Customers",
                column: "CitizenshipId",
                principalTable: "Citizenships",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Genders_GenderId",
                table: "Customers",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Cities_CityId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Citizenships_CitizenshipId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Genders_GenderId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CitizenshipId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CityId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_GenderId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CitizenshipId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "Customers");
        }
    }
}
