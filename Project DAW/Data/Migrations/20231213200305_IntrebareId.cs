using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_DAW.Data.Migrations
{
    public partial class IntrebareId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarii_Intrebari_IntrebareId",
                table: "Comentarii");

            migrationBuilder.DropColumn(
                name: "Intrebare_ID",
                table: "Comentarii");

            migrationBuilder.AddColumn<int>(
                name: "IntrebareId",
                table: "Intrebari",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarii_Intrebari_IntrebareId",
                table: "Comentarii");

            migrationBuilder.DropColumn(
                name: "IntrebareId",
                table: "Intrebari");

            migrationBuilder.AlterColumn<int>(
                name: "IntrebareId",
                table: "Comentarii",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Intrebare_ID",
                table: "Comentarii",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarii_Intrebari_IntrebareId",
                table: "Comentarii",
                column: "IntrebareId",
                principalTable: "Intrebari",
                principalColumn: "Id");
        }
    }
}
