using curriculo_gamer_pt2.Models.Entities;

namespace curriculo_gamer_pt2.Models.Interfaces
{
    public interface ICategoriaService
    {
        List<Categoria> ListarTodos();
        Categoria Incluir(Categoria categoria);
        Categoria? BuscarPorId(int id);
        bool Deletar(int id);
        List<Categoria> BuscarPorIds(List<int> categoriaIds);
        Categoria? BuscarPorNome(string nome);
        List<Categoria> BuscarPorNomes(List<string> nomes);
    }
}
