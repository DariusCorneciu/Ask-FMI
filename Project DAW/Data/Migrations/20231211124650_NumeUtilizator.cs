using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_DAW.Data.Migrations
{
    public partial class NumeUtilizator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Intrebari",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Comentarii",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Intrebari_ApplicationUserId",
                table: "Intrebari",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarii_ApplicationUserId",
                table: "Comentarii",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarii_AspNetUsers_ApplicationUserId",
                table: "Comentarii",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Intrebari_AspNetUsers_ApplicationUserId",
                table: "Intrebari",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarii_AspNetUsers_ApplicationUserId",
                table: "Comentarii");

            migrationBuilder.DropForeignKey(
                name: "FK_Intrebari_AspNetUsers_ApplicationUserId",
                table: "Intrebari");

            migrationBuilder.DropIndex(
                name: "IX_Intrebari_ApplicationUserId",
                table: "Intrebari");

            migrationBuilder.DropIndex(
                name: "IX_Comentarii_ApplicationUserId",
                table: "Comentarii");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Intrebari");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Comentarii");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
