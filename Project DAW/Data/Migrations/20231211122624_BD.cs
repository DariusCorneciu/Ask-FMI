using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_DAW.Data.Migrations
{
    public partial class BD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorii",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorii", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCategorii",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategorieId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: null, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategorii", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategorii_Categorii_CategorieId",
                        column: x => x.CategorieId,
                        principalTable: "Categorii",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Intrebari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Continut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubCategorieId = table.Column<int>(type: "int", nullable: false),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intrebari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Intrebari_SubCategorii_SubCategorieId",
                        column: x => x.SubCategorieId,
                        principalTable: "SubCategorii",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentarii",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Intrebare_ID = table.Column<int>(type: "int", nullable: false),
                    IntrebareId = table.Column<int>(type: "int", nullable: true),
                    Continut = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarii", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentarii_Intrebari_IntrebareId",
                        column: x => x.IntrebareId,
                        principalTable: "Intrebari",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Raspunsuri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntrebareId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raspunsuri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Raspunsuri_Intrebari_IntrebareId",
                        column: x => x.IntrebareId,
                        principalTable: "Intrebari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarii_IntrebareId",
                table: "Comentarii",
                column: "IntrebareId");

            migrationBuilder.CreateIndex(
                name: "IX_Intrebari_SubCategorieId",
                table: "Intrebari",
                column: "SubCategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_Raspunsuri_IntrebareId",
                table: "Raspunsuri",
                column: "IntrebareId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategorii_CategorieId",
                table: "SubCategorii",
                column: "CategorieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentarii");

            migrationBuilder.DropTable(
                name: "Raspunsuri");

            migrationBuilder.DropTable(
                name: "Intrebari");

            migrationBuilder.DropTable(
                name: "SubCategorii");

            migrationBuilder.DropTable(
                name: "Categorii");
        }
    }
}
