using curriculo_gamer_pt2.Models.DTOs;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Interfaces;
using curriculo_gamer_pt2.Views.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        [HttpGet("login")]
        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return View("JaLogado");
            }
            return View();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("X-Access-Token");
            return Redirect("Home");
        }

        [HttpPost("login")]
        public IActionResult Login([FromForm] LoginDto loginDto)
        {
            if (loginDto == null)
                return BadRequest("Dados incompletos");
            var usuarios = _userService.Listar();
            var usuario = usuarios.FirstOrDefault(u => u.Email == loginDto.Email && u.Senha == HashPassword(loginDto.Senha));
            if (usuario == null)
            {
                return View("LoginInvalido");
            }

            string token = _userService.GerarTokenJwt(usuario);

            Response.Cookies.Append("X-Access-Token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });

            return Redirect("Home");
        }

        [HttpPost("register")]
        public IActionResult Registrar([FromForm] RegistroDto registroDto)
        {
            if(HashPassword(registroDto.Senha) != HashPassword(registroDto.ConfirmarSenha))
            {
                return BadRequest("As senhas não coincidem.");
            }

            if(_userService.Listar().Any(u => u.Email == registroDto.Email))
            {
                return BadRequest("Email já está em uso.");
            }
            if(_userService.Listar().Any(u => u.Nome == registroDto.Nome))
            {
                return BadRequest("Nome de usuário já está em uso.");
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
