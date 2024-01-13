using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_DAW.Data.Migrations
{
    public partial class FixRaspuns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Raspunsuri",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Raspunsuri_UserId",
                table: "Raspunsuri",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Raspunsuri_AspNetUsers_UserId",
                table: "Raspunsuri",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Raspunsuri_AspNetUsers_UserId",
                table: "Raspunsuri");

            migrationBuilder.DropIndex(
                name: "IX_Raspunsuri_UserId",
                table: "Raspunsuri");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Raspunsuri",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
