using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_DAW.Data.Migrations
{
    public partial class FixRaspunsuri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarii_Intrebari_IntrebareId",
                table: "Comentarii");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Raspunsuri",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IntrebareId",
                table: "Comentarii",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarii_Intrebari_IntrebareId",
                table: "Comentarii",
                column: "IntrebareId",
                principalTable: "Intrebari",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarii_Intrebari_IntrebareId",
                table: "Comentarii");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Raspunsuri",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "IntrebareId",
                table: "Comentarii",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarii_Intrebari_IntrebareId",
                table: "Comentarii",
                column: "IntrebareId",
                principalTable: "Intrebari",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
