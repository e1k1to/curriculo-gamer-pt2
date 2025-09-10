using curriculo_gamer_pt2.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace curriculo_gamer_pt2.Models.Context
{
    public class LogContext : DbContext
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("User");

            modelBuilder.Entity<JogoJogado>()
                .HasOne(jj => jj.User)
                .WithMany(u => u.JogosJogados)
                .HasForeignKey(jj => jj.UserId);

            modelBuilder.Entity<JogoJogado>()
                .HasOne(jj => jj.Jogo)
                .WithMany()
                .HasForeignKey(jj => jj.JogoId);

            modelBuilder.Entity<Jogo>()
                .HasMany(j => j.Categorias)
                .WithMany(c => c.Jogos);
        }

        public LogContext(DbContextOptions<LogContext> options) : base(options) { }
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<JogoJogado> JogosJogados { get; set; }
    }
}
