using curriculo_gamer_pt2.Models.Entities;

namespace curriculo_gamer_pt2.Models.Interfaces
{
    public interface IUserService
    {
        User Incluir(User user);
        List<User> Listar();
        User? BuscarPorId(int id);
        User Atualizar(User user);
        bool Deletar(int id);
        string GerarTokenJwt(User usuario);
    }
}
