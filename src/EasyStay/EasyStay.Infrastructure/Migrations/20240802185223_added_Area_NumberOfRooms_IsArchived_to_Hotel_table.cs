using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyStay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_Area_NumberOfRooms_IsArchived_to_Hotel_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Area",
                table: "Hotels",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Hotels",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRooms",
                table: "Hotels",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "NumberOfRooms",
                table: "Hotels");
        }
    }
}
