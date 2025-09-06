using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace curriculo_gamer_pt2.Migrations
{
    /// <inheritdoc />
    public partial class roleseautenticacaoesperoqultima : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Users SET ROLE = 'User' WHERE Role is NULL OR Role = ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
