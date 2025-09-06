using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.Context;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace curriculo_gamer_pt2.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly string SecretKey;
        private readonly LogContext _context;
        public UserService(LogContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            SecretKey = _configuration["Jwt:Key"] ?? string.Empty;
        }

        public string GerarTokenJwt(User user)
        {
            if (string.IsNullOrEmpty(SecretKey)) { return string.Empty; }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim("Email", user.Email),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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
