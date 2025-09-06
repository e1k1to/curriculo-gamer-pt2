using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.DTOs;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Enums;
using curriculo_gamer_pt2.Models.Interfaces;
using curriculo_gamer_pt2.Services;
using curriculo_gamer_pt2.Views.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Text;

namespace curriculo_gamer_pt2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJogoJogadoService _jogoJogadoService;
        private readonly IJogoService _jogoService;
        public UserController(IUserService userService, IJogoJogadoService jogoJogadoService, IJogoService jogoService)
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
                usuarios.Select(usuario => new SimpleUserQuery
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Role = usuario.Role,
                }).ToList()
            );
        }

        //User? BuscarPorId(int id);
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)
        {
            var usuario = _userService.BuscarPorId(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(new UserQuery {
                Nome = usuario.Nome,
                Role = usuario.Role,
                Email = usuario.Email,
                JogosJogados = _jogoJogadoService.Listar()!
                    .Where(a => a.UserId == usuario.Id)
                    .Select(b =>
                    {
                        var jogo = _jogoService.BuscarPorId(b.JogoId);
                        return new JogoJogadoQuery
                        {
                            Nome = jogo?.Nome ?? "Não encontrado",
                            Descricao = jogo?.Descricao ?? "Não encontrado",
                            HorasJogadas = b.HorasJogadas,
                            StatusJogo = b.StatusJogo.ToString()
                        };
                    }).ToList()

            });
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
        public IActionResult DeletarUsuario(int id, string senha)
        {
            var usuario = _userService.BuscarPorId(id);

            if(usuario == null || usuario.Senha != HashPassword(senha))
                return NotFound();

            var sucesso = _userService.Deletar(id);

            return NoContent();
        }

        
    }
}
