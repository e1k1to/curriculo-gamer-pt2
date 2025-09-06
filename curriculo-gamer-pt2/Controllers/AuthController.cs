using curriculo_gamer_pt2.Models.DTOs;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Interfaces;
using curriculo_gamer_pt2.Views.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace curriculo_gamer_pt2.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        private static string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        [HttpPost("login")]
        
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var usuarios = _userService.Listar();
            var usuario = usuarios.FirstOrDefault(u => u.Email == loginDto.Email && u.Senha == HashPassword(loginDto.Senha));
            if (usuario == null)
            {
                return Unauthorized("Email ou senha inválidos.");
            }
            string token = _userService.GerarTokenJwt(usuario);
            return Ok(new UserLogado
            {
                Email = usuario.Email,
                Token = token
            });
        }

        [HttpPost("register")]
        public IActionResult Registrar([FromBody] RegistroDto registroDto)
        {
            if(HashPassword(registroDto.Senha) != HashPassword(registroDto.ConfirmarSenha))
            {
                return BadRequest("As senhas não coincidem.");
            }

            User user = new User
            {
                Nome = registroDto.Nome,
                Email = registroDto.Email,
                Senha = HashPassword(registroDto.Senha),
                Role = "User"
            };

            var novoUsuario = _userService.Incluir(user);

            return CreatedAtAction(nameof(Registrar), new { id = novoUsuario.Id }, novoUsuario);
        }
    }
}
