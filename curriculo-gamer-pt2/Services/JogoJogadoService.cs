using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.Context;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Interfaces;

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
            var jogoJogadoExistente = BuscarPorId(jogoJogado.Id);
            if(jogoJogadoExistente == null)
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
            _context.JogosJogados.Add(jogoJogado);
            _context.SaveChanges();
            return jogoJogado;
        }

        public List<JogoJogado> Listar()
        {
            return _context.JogosJogados.ToList();
        }
    }
}
