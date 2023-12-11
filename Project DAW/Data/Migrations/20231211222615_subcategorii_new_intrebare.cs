using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_DAW.Data.Migrations
{
    public partial class subcategorii_new_intrebare : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarii_AspNetUsers_ApplicationUserId",
                table: "Comentarii");

            migrationBuilder.DropForeignKey(
                name: "FK_Intrebari_AspNetUsers_ApplicationUserId",
                table: "Intrebari");

            migrationBuilder.DropIndex(
                name: "IX_Comentarii_ApplicationUserId",
                table: "Comentarii");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Comentarii");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Intrebari",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Intrebari_ApplicationUserId",
                table: "Intrebari",
                newName: "IX_Intrebari_UserId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Comentarii",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarii_UserId",
                table: "Comentarii",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarii_AspNetUsers_UserId",
                table: "Comentarii",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Intrebari_AspNetUsers_UserId",
                table: "Intrebari",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarii_AspNetUsers_UserId",
                table: "Comentarii");

            migrationBuilder.DropForeignKey(
                name: "FK_Intrebari_AspNetUsers_UserId",
                table: "Intrebari");

            migrationBuilder.DropIndex(
                name: "IX_Comentarii_UserId",
                table: "Comentarii");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comentarii");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Intrebari",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Intrebari_UserId",
                table: "Intrebari",
                newName: "IX_Intrebari_ApplicationUserId");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Comentarii",
                type: "nvarchar(450)",
                nullable: true);

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
    }
}
