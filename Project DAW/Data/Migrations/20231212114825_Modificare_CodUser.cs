using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_DAW.Data.Migrations
{
    public partial class Modificare_CodUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Intrebari_SubCategorii_SubCategorieId",
                table: "Intrebari");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "SubCategorieId",
                table: "Intrebari",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Intrebari",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Intrebari_SubCategorii_SubCategorieId",
                table: "Intrebari",
                column: "SubCategorieId",
                principalTable: "SubCategorii",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Intrebari_SubCategorii_SubCategorieId",
                table: "Intrebari");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Intrebari");

            migrationBuilder.AlterColumn<int>(
                name: "SubCategorieId",
                table: "Intrebari",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Intrebari_SubCategorii_SubCategorieId",
                table: "Intrebari",
                column: "SubCategorieId",
                principalTable: "SubCategorii",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
