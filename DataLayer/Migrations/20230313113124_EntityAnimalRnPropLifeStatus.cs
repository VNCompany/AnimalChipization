using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class EntityAnimalRnPropLifeStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LifeStatis",
                table: "Animals");

            migrationBuilder.AddColumn<string>(
                name: "LifeStatus",
                table: "Animals",
                type: "longtext",
                nullable: false,
                defaultValue: "ALIVE")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 1L,
                column: "LifeStatus",
                value: "ALIVE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LifeStatus",
                table: "Animals");

            migrationBuilder.AddColumn<string>(
                name: "LifeStatis",
                table: "Animals",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 1L,
                column: "LifeStatis",
                value: "ALIVE");
        }
    }
}
