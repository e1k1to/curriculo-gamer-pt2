using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.Context;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace curriculo_gamer_pt2.Services
{
    public class JogoJogadoService : IJogoJogadoService
    {
        private readonly LogContext _context;
        public JogoJogadoService(LogContext context)
        {
            _context = context;
        }
        public JogoJogado Atualizar(JogoJogado jogoJogado)
        {
            var jogoIdValido = _context.Jogos.Find(jogoJogado.JogoId);
            if (jogoIdValido == null)
                throw new ObjectNotFoundException("Jogo não encontrado");

            var userIdValido = _context.Users.Find(jogoJogado.UserId);
            if (userIdValido == null)
                throw new ObjectNotFoundException("Usuário não encontrado");

            var jogoJogadoExistente = BuscarPorId(jogoJogado.Id);

            if (jogoJogadoExistente == null)
                throw new ObjectNotFoundException("Jogo jogado não encontrado");

            jogoJogadoExistente.HorasJogadas = jogoJogado.HorasJogadas;
            jogoJogadoExistente.StatusJogo = jogoJogado.StatusJogo;
            _context.SaveChanges();
            return jogoJogadoExistente;
        }

        public JogoJogado? BuscarPorId(int id)
        {
            return _context.JogosJogados.Find(id);
        }

        public bool Deletar(int id)
        {
            JogoJogado? jogoJogadoBuscado = BuscarPorId(id);
            if (jogoJogadoBuscado == null)
                return false;
            _context.JogosJogados.Remove(jogoJogadoBuscado);
            _context.SaveChanges();
            return true;
        }

        public JogoJogado Incluir(JogoJogado jogoJogado)
        {
            var jogoIdValido = _context.Jogos.Find(jogoJogado.JogoId);
            var userIdValido = _context.Users.Find(jogoJogado.UserId);

            if(jogoIdValido == null)
                throw new ObjectNotFoundException("Jogo não encontrado");

            if(userIdValido == null)
                throw new ObjectNotFoundException("Usuário não encontrado");

            _context.JogosJogados.Add(jogoJogado);
            _context.SaveChanges();
            return jogoJogado;
        }

        public List<JogoJogado> GetJogoJogadoUsuario(int userId)
        {
            return _context.JogosJogados
                .Include(jj => jj.Jogo)
                .Where(jj => jj.UserId == userId)
                .ToList();
        }
    }
}
