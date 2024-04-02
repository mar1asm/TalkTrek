using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_platform.Migrations
{
    /// <inheritdoc />
    public partial class updateMigration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "User",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "Tutor",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "Tutor",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Reviews",
                table: "Tutor",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "Tutor");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Tutor");

            migrationBuilder.DropColumn(
                name: "Reviews",
                table: "Tutor");
        }
    }
}
