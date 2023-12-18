using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_DAW.Data.Migrations
{
    public partial class Imaginigen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imagine_AspNetUsers_UserId",
                table: "Imagine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Imagine",
                table: "Imagine");

            migrationBuilder.RenameTable(
                name: "Imagine",
                newName: "Imagini");

            migrationBuilder.RenameIndex(
                name: "IX_Imagine_UserId",
                table: "Imagini",
                newName: "IX_Imagini_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Imagini",
                table: "Imagini",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Imagini_AspNetUsers_UserId",
                table: "Imagini",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imagini_AspNetUsers_UserId",
                table: "Imagini");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Imagini",
                table: "Imagini");

            migrationBuilder.RenameTable(
                name: "Imagini",
                newName: "Imagine");

            migrationBuilder.RenameIndex(
                name: "IX_Imagini_UserId",
                table: "Imagine",
                newName: "IX_Imagine_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Imagine",
                table: "Imagine",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Imagine_AspNetUsers_UserId",
                table: "Imagine",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
