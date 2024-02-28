using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_platform.Migrations
{
    /// <inheritdoc />
    public partial class updateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_AspNetUsers_ApplicationUserId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ApplicationUserId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "User",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_User_ApplicationUserId",
                table: "User",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_AspNetUsers_ApplicationUserId",
                table: "User",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
