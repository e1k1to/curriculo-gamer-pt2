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
                usuarios.Select(usuario => new SimpleUserQuery {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                }).ToList()
            );
        }

        //User Incluir(User user);
        [HttpPost]
        public IActionResult IncluirUsuario([FromBody] UserDto userDto)
        {
            User user = new User
            {
                Nome = userDto.Nome,
                Email = userDto.Email,
                Senha = HashPassword(userDto.Senha)
            };
            var novoUsuario = _userService.Incluir(user);
            return CreatedAtAction(nameof(IncluirUsuario), new { id = novoUsuario.Id }, novoUsuario);
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
        public IActionResult DeletarUsuario(int id)
        {
            var sucesso = _userService.Deletar(id);
            if(!sucesso)
                return NotFound();
            return NoContent();
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
    }
}
