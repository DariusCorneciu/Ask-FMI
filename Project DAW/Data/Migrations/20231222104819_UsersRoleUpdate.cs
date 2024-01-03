using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_DAW.Data.Migrations
{
    public partial class UsersRoleUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Intrebari_SubCategorii_SubCategorieId",
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

            migrationBuilder.AddColumn<bool>(
                name: "Admitere",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "BanTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Licenta",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Master",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Moderator",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Intrebari_SubCategorii_SubCategorieId",
                table: "Intrebari",
                column: "SubCategorieId",
                principalTable: "SubCategorii",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Intrebari_SubCategorii_SubCategorieId",
                table: "Intrebari");

            migrationBuilder.DropColumn(
                name: "Admitere",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BanTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Licenta",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Master",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Moderator",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "SubCategorieId",
                table: "Intrebari",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Intrebari_SubCategorii_SubCategorieId",
                table: "Intrebari",
                column: "SubCategorieId",
                principalTable: "SubCategorii",
                principalColumn: "Id");
        }
    }
}
