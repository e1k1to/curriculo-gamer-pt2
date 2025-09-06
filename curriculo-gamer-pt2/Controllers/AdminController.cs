using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.DTOs;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Enums;
using curriculo_gamer_pt2.Models.Interfaces;
using curriculo_gamer_pt2.Services;
using curriculo_gamer_pt2.Views.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Text;

namespace curriculo_gamer_pt2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJogoJogadoService _jogoJogadoService;
        private readonly IJogoService _jogoService;
        public AdminController(IUserService userService, IJogoJogadoService jogoJogadoService, IJogoService jogoService)
        {
            _userService = userService;
            _jogoJogadoService = jogoJogadoService;
            _jogoService = jogoService;
        }

        private static string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        //List<User> Listar();
        [HttpGet]
        public IActionResult ListarUsuarios()
        {
            var usuarios = _userService.Listar();
            return Ok(
                usuarios.Where(usuario => usuario.Role == "Admin").Select(usuario => new SimpleUserQuery {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Role = usuario.Role,
                }).ToList()
            );
        }

        [HttpPost("register")]
        public IActionResult Registrar([FromBody] RegistroDto registroDto)
        {
            if (HashPassword(registroDto.Senha) != HashPassword(registroDto.ConfirmarSenha))
            {
                return BadRequest("As senhas não coincidem.");
            }

            User user = new User
            {
                Nome = registroDto.Nome,
                Email = registroDto.Email,
                Senha = HashPassword(registroDto.Senha),
                Role = "Admin"
            };

            var novoUsuario = _userService.Incluir(user);

            return CreatedAtAction(nameof(Registrar), new { id = novoUsuario.Id }, novoUsuario);
        }

        //User Atualizar(User user);
        [HttpPut]
        public IActionResult AtualizarUsuario([FromBody] UserDto userDto)
        {
            User user =  new User
            {
                Id = userDto.Id,
                Nome = userDto.Nome,
                Email = userDto.Email,
                Senha = HashPassword(userDto.Senha)
            };
            try
            {
                var usuarioAtualizado = _userService.Atualizar(user);
                return Ok(usuarioAtualizado);
            } catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        //bool Deletar(int id);
        [HttpDelete("{id}")]
        public IActionResult DeletarUsuario(int id)
        {
            var sucesso = _userService.Deletar(id);
            if(!sucesso)
                return NotFound();
            return NoContent();
        }

        
    }
}
