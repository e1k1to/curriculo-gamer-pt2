using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.DTOs;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace curriculo_gamer_pt2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        //List<User> Listar();
        [HttpGet]
        public IActionResult ListarUsuarios()
        {
            var usuarios = _userService.Listar();
            return Ok(usuarios);
        }

        //User Incluir(User user);
        [HttpPost]
        public IActionResult IncluirUsuario([FromBody] UserDto userDto)
        {
            User user = new User
            {
                Nome = userDto.Nome,
                Email = userDto.Email,
                Senha = userDto.Senha
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
            return Ok(usuario);
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
                Senha = userDto.Senha
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
