using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class EntityAnimalColumnsTypesFloat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Weight",
                table: "Animals",
                type: "FLOAT(13,8)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "Length",
                table: "Animals",
                type: "FLOAT(13,8)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "Height",
                table: "Animals",
                type: "FLOAT(13,8)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Weight",
                table: "Animals",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "FLOAT(13,8)");

            migrationBuilder.AlterColumn<float>(
                name: "Length",
                table: "Animals",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "FLOAT(13,8)");

            migrationBuilder.AlterColumn<float>(
                name: "Height",
                table: "Animals",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "FLOAT(13,8)");
        }
    }
}
