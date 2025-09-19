using curriculo_gamer_pt2.Models.Entities;

namespace curriculo_gamer_pt2.Models.Interfaces
{
    public interface IUserService
    {
        List<User> Listar();
        User? BuscarPorId(int id);
        User Atualizar(User user);
        User? BuscarPorEmail(string email);
        User? BuscarPorUsername(string username);
        bool Deletar(int id);
        User Incluir(User user);
        string GerarTokenJwt(User usuario);
    }
}
