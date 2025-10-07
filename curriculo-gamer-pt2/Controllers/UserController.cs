using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.DTOs;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Enums;
using curriculo_gamer_pt2.Models.Interfaces;
using curriculo_gamer_pt2.Services;
using curriculo_gamer_pt2.Views.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Security.Claims;
using System.Text;

namespace curriculo_gamer_pt2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
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

        [AllowAnonymous]
        [HttpGet("{username}")]
        public IActionResult BuscarPorUsername(string username)
        {
            var usuario = _userService.BuscarPorUsername(username);
            
            if (usuario == null)
            {
                return NotFound();
            }

            var jogosJogados = _jogoJogadoService.GetJogoJogadoUsuario(usuario.Id).Select( jj => new JogoJogadoQuery
            {
                Nome = jj.Jogo.Nome,
                Descricao = jj.Jogo.Descricao,
                HorasJogadas = jj.HorasJogadas,
                StatusJogo = jj.StatusJogo.ToString()
            });


            ViewBag.Dados = new {Usuario = usuario, JogosJogados = jogosJogados };
            return View();

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

        [HttpGet("{username}/jogos")]
        public IActionResult ListarJogosJogados(string username)
        {
            var user = _userService.BuscarPorUsername(username);

            if(user == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            var id = user.Id;

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

        [HttpGet("{username}/adicionarJogoJogado")]
        public IActionResult AdicionarJogoJogado(string username) 
        {
            var user = _userService.BuscarPorUsername(username);

            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != user.Id.ToString())
            {
                return Forbid();
            }

            var jogos = _jogoService.ListarTodos().Select( j => new GetJogoSimplesDto
            {
                Id = j.Id,
                Nome = j.Nome
            });

            ViewBag.Jogos = jogos;
            ViewBag.Username = username;
            return View();
        }

        [HttpGet("{username}/adicionarJogoJogado/{jogoId}")]
        public IActionResult AdicionarJogoJogadoId(string username, int jogoId) 
        {
            var user = _userService.BuscarPorUsername(username);

            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != user.Id.ToString())
            {
                return Forbid();
            }

            var jogo = _jogoService.BuscarPorId(jogoId);

            if (jogo == null)
            {
                return NotFound(new { message = "Jogo não encontrado" });
            }

            var nomeJogo = jogo.Nome;

            ViewBag.Jogo = new GetJogoSimplesDto { Id = jogoId, Nome = nomeJogo };
            ViewBag.Username = username;
            return View();
        }


        [HttpPost("{username}/adicionarJogoJogado")]
        public IActionResult AdicionarJogoJogadoPost(string username, [FromForm] AdicionarJogoJogadoDto jogoJogadoDto)
        {
            var user = _userService.BuscarPorUsername(username);

            if(user == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != user.Id.ToString())
            {
                return Forbid();
            }

            if (_jogoJogadoService.GetJogoJogadoUsuario(user.Id).Any(jj => jj.JogoId == jogoJogadoDto.JogoId))
            {
                return Conflict(new { message = "Jogo já adicionado ao usuário." });
            }

            var jogo = _jogoService.BuscarPorId(jogoJogadoDto.JogoId);

            if (jogo == null)
            {
                return NotFound(new { message = "Jogo não encontrado." });
            }

            JogoJogado novoJogoJogado = new JogoJogado
            {
                JogoId = jogo.Id,
                UserId = user.Id,
                HorasJogadas = jogoJogadoDto.HorasJogadas,
                StatusJogo = jogoJogadoDto.StatusJogo
            };

            try
            {
                _jogoJogadoService.Incluir(novoJogoJogado);
                return CreatedAtAction(nameof(BuscarPorUsername), new { id = novoJogoJogado.Id }, new JogoJogadoQuery
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

        [HttpPut("{username}/jogos")]
        public IActionResult AtualizarJogoJogado(string username, [FromBody] AdicionarJogoJogadoDto jogoJogadoDto)
        {
            var user = _userService.BuscarPorUsername(username);

            if(user == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            var id = user.Id;

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

        [HttpDelete("{username}/jogos/{jogoId}")]
        public IActionResult DeletarJogoJogado(string username, int jogoId)
        {
            var user =  _userService.BuscarPorUsername(username);

            if(user == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            var id = user.Id;

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
