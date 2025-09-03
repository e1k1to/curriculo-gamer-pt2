using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.Context;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace curriculo_gamer_pt2.Services
{
    public class JogoService : IJogoService
    {
        private readonly LogContext _context;
        public JogoService(LogContext context)
        {
            _context = context;
        }

        public List<Jogo> ListarTodos()
        {
            return _context.Jogos.ToList();
        }

        public Jogo Atualizar(Jogo jogo)
        {
            var jogoExistente = _context.Jogos.Find(jogo.Id);
            if (jogoExistente == null)
                throw new ObjectNotFoundException("Jogo não encontrado");
            jogoExistente.Nome = jogo.Nome;
            jogoExistente.Descricao = jogo.Descricao;
            jogoExistente.AnoLancamento = jogo.AnoLancamento;
            _context.SaveChanges();
            return jogoExistente;
        }

        public Jogo? BuscarPorId(int id)
        {
            return _context.Jogos.Find(id);
        }

        public bool Deletar(int id)
        {
            Jogo? jogoBuscado = BuscarPorId(id);
            if (jogoBuscado == null)
                return false;
            _context.Jogos.Remove(jogoBuscado);
            _context.SaveChanges();
            return true;
        }

        public Jogo Incluir(Jogo jogo)
        {
            _context.Add(jogo);
            _context.SaveChanges();
            return jogo;
        }
    }
}
