using curriculo_gamer_pt2.Models.Entities;

namespace curriculo_gamer_pt2.Models.Interfaces
{
    public interface IJogoService
    {
        List<Jogo> ListarTodos();
        Jogo Incluir(Jogo jogo);
        Jogo? BuscarPorId(int id);
        Jogo Atualizar(Jogo jogo);
        bool Deletar(int id);
        void AtualizarCategorias(Jogo jogo, List<Categoria> categorias);
    }
}
