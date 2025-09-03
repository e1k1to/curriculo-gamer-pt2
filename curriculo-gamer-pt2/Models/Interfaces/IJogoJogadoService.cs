using curriculo_gamer_pt2.Models.Entities;

namespace curriculo_gamer_pt2.Models.Interfaces
{
    public interface IJogoJogadoService
    {
        JogoJogado Incluir(JogoJogado jogoJogado);
        List<JogoJogado>? Listar();
        JogoJogado? BuscarPorId(int id);
        JogoJogado Atualizar(JogoJogado jogoJogado);
        bool Deletar(int id);
    }
}
