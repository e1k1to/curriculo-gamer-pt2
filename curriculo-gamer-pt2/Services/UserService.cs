using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.Context;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Interfaces;

namespace curriculo_gamer_pt2.Services
{
    public class UserService : IUserService
    {
        private readonly LogContext _context;
        public UserService(LogContext context)
        {
            _context = context;
        }
        public User Atualizar(User user)
        {
            var userExistente = BuscarPorId(user.Id);
            if (userExistente == null)
                throw new ObjectNotFoundException("Usuario não encontrado");
            userExistente.Nome = user.Nome;
            userExistente.Email = user.Email;
            userExistente.Senha = user.Senha;
            _context.SaveChanges();
            return userExistente;
        }

        public User? BuscarPorId(int id)
        {
            return _context.Users.Find(id);
        }

        public bool Deletar(int id)
        {
            User? usuarioBuscado = BuscarPorId(id);
            if (usuarioBuscado == null)
                return false;
            _context.Users.Remove(usuarioBuscado);
            _context.SaveChanges();
            return true;
        }

        public User Incluir(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public List<User> Listar()
        {
            return _context.Users.ToList();
        }
    }
}
