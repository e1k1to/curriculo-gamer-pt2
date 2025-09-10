using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.DTOs;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Enums;
using curriculo_gamer_pt2.Models.Interfaces;
using curriculo_gamer_pt2.Services;
using curriculo_gamer_pt2.Views.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Security.Claims;
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
                JogosJogados = _jogoJogadoService.GetJogoJogadoUsuario(id)!
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
            User user = new User
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

            if (usuario == null || usuario.Senha != HashPassword(senha))
                return NotFound();

            var sucesso = _userService.Deletar(id);

            return NoContent();
        }

        [HttpGet("{id}/jogos")]
        public IActionResult ListarJogosJogados(int id)
        {
            var lista = _jogoJogadoService.GetJogoJogadoUsuario(id);

            var jogosJogados = lista.Select(jj => new JogoJogadoQuery
            {
                Nome = jj.Jogo.Nome,
                Descricao = jj.Jogo.Descricao,
                HorasJogadas = jj.HorasJogadas,
                StatusJogo = jj.StatusJogo.ToString()
            });

            return Ok(jogosJogados);
        }

        [HttpPost("{id}/jogos")]
        public IActionResult AdicionarJogoJogado(int id, [FromBody] AdicionarJogoJogadoDto jogoJogadoDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != id.ToString())
            {
                return Forbid();
            }

            if (_jogoJogadoService.GetJogoJogadoUsuario(id)!.Any(jj => jj.JogoId == jogoJogadoDto.JogoId))
            {
                return Conflict(new { message = "Jogo já adicionado ao usuário." });
            }

            JogoJogado novoJogoJogado = new JogoJogado
            {
                JogoId = jogoJogadoDto.JogoId,
                UserId = id,
                HorasJogadas = jogoJogadoDto.HorasJogadas,
                StatusJogo = jogoJogadoDto.StatusJogo
            };

            try
            {
                _jogoJogadoService.Incluir(novoJogoJogado);
                return CreatedAtAction(nameof(BuscarPorId), new { id = novoJogoJogado.Id }, new JogoJogadoQuery
                {
                    Nome = novoJogoJogado.Jogo.Nome,
                    Descricao = novoJogoJogado.Jogo.Descricao,
                    HorasJogadas = novoJogoJogado.HorasJogadas,
                    StatusJogo = novoJogoJogado.StatusJogo.ToString()
                });
            }
            catch (ObjectNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }

        }

        [HttpPut("{id}/jogos")]
        public IActionResult AtualizarJogoJogado(int id, [FromBody] AdicionarJogoJogadoDto jogoJogadoDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != id.ToString())
            {
                return Forbid();
            }

            var jogoJogadoExistente = _jogoJogadoService.GetJogoJogadoUsuario(id)!.FirstOrDefault(jj => jj.JogoId == jogoJogadoDto.JogoId);

            if (jogoJogadoExistente == null)
            {
                return Conflict(new { message = "Não é possivel atualizar um registro inexistente." });
            }

            jogoJogadoExistente.StatusJogo = jogoJogadoDto.StatusJogo;
            jogoJogadoExistente.HorasJogadas = jogoJogadoDto.HorasJogadas;

            var jogoJogadoAtualizado = _jogoJogadoService.Atualizar(jogoJogadoExistente);
            return Ok(new JogoJogadoQuery
            {
                Nome = jogoJogadoAtualizado.Jogo.Nome,
                Descricao = jogoJogadoAtualizado.Jogo.Descricao,
                HorasJogadas = jogoJogadoAtualizado.HorasJogadas,
                StatusJogo = jogoJogadoAtualizado.StatusJogo.ToString()
            });
        }

        [HttpDelete("{id}/jogos/{jogoId}")]
        public IActionResult DeletarJogoJogado(int id, int jogoId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != id.ToString())
            {
                return Forbid();
            }
            var jogoJogadoExistente = _jogoJogadoService.GetJogoJogadoUsuario(id)!.FirstOrDefault(jj => jj.JogoId == jogoId);
            if (jogoJogadoExistente == null)
            {
                return NotFound(new { message = "Registro de jogo jogado não encontrado para o usuário." });
            }
            var sucesso = _jogoJogadoService.Deletar(jogoJogadoExistente.Id);
            return NoContent();
        }
    }
}
