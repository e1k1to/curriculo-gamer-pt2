using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace curriculo_gamer_pt2.Migrations
{
    /// <inheritdoc />
    public partial class categoriaserelacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CategoriaJogo",
                columns: table => new
                {
                    CategoriasId = table.Column<int>(type: "int", nullable: false),
                    JogosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaJogo", x => new { x.CategoriasId, x.JogosId });
                    table.ForeignKey(
                        name: "FK_CategoriaJogo_Categorias_CategoriasId",
                        column: x => x.CategoriasId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriaJogo_Jogos_JogosId",
                        column: x => x.JogosId,
                        principalTable: "Jogos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_JogosJogados_JogoId",
                table: "JogosJogados",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_JogosJogados_UserId",
                table: "JogosJogados",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaJogo_JogosId",
                table: "CategoriaJogo",
                column: "JogosId");

            migrationBuilder.AddForeignKey(
                name: "FK_JogosJogados_Jogos_JogoId",
                table: "JogosJogados",
                column: "JogoId",
                principalTable: "Jogos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JogosJogados_Users_UserId",
                table: "JogosJogados",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JogosJogados_Jogos_JogoId",
                table: "JogosJogados");

            migrationBuilder.DropForeignKey(
                name: "FK_JogosJogados_Users_UserId",
                table: "JogosJogados");

            migrationBuilder.DropTable(
                name: "CategoriaJogo");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_JogosJogados_JogoId",
                table: "JogosJogados");

            migrationBuilder.DropIndex(
                name: "IX_JogosJogados_UserId",
                table: "JogosJogados");
        }
    }
}
