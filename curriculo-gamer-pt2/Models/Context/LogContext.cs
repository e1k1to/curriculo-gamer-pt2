using curriculo_gamer_pt2.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace curriculo_gamer_pt2.Models.Context
{
    public class LogContext : DbContext
    {
        public LogContext(DbContextOptions<LogContext> options) : base(options) { }

        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<JogoJogado> JogosJogados { get; set; }
    }
}
