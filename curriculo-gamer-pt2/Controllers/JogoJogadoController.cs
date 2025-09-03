using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.DTOs;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace curriculo_gamer_pt2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JogoJogadoController : ControllerBase
    {
        private readonly IJogoJogadoService _jogoJogadoService;
        public JogoJogadoController(IJogoJogadoService jogoJogadoService)
        {
            _jogoJogadoService = jogoJogadoService;
        }   
        //List<JogoJogado>? Listar();
        [HttpGet]
        public IActionResult Listar()
        {
            var lista = _jogoJogadoService.Listar();
            return Ok(lista);
        }
        //JogoJogado? BuscarPorId(int id);
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)
        {
            var jogoJogado = _jogoJogadoService.BuscarPorId(id);
            if (jogoJogado == null)
            {
                return NotFound();
            }
            return Ok(jogoJogado);
        }
        //JogoJogado Incluir(JogoJogado jogoJogado);
        [HttpPost]
        public IActionResult Incluir([FromBody] JogoJogadoDto jogoJogadoDto)
        {
            JogoJogado novoJogoJogado = new JogoJogado
            {
                JogoId = jogoJogadoDto.JogoId,
                UserId = jogoJogadoDto.UserId,
                HorasJogadas = jogoJogadoDto.HorasJogadas,
                StatusJogo = jogoJogadoDto.StatusJogo,
            };
            _jogoJogadoService.Incluir(novoJogoJogado);
            return CreatedAtAction(nameof(BuscarPorId), new { id = novoJogoJogado.Id }, novoJogoJogado);
        }
        //JogoJogado Atualizar(JogoJogado jogoJogado);
        [HttpPut]
        public IActionResult Atualizar([FromBody] JogoJogadoDto jogoJogadoDto)
        {
            JogoJogado jogoJogado = new JogoJogado
            {
                Id = jogoJogadoDto.Id,
                JogoId = jogoJogadoDto.JogoId,
                UserId = jogoJogadoDto.UserId,
                HorasJogadas = jogoJogadoDto.HorasJogadas,
                StatusJogo = jogoJogadoDto.StatusJogo,
            };
            try
            {
                var jogoAtualizado = _jogoJogadoService.Atualizar(jogoJogado);
                return Ok(jogoAtualizado);
            } catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        //bool Deletar(int id);
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var sucesso = _jogoJogadoService.Deletar(id);
            if(!sucesso)
                return NotFound();
            return NoContent();
        }
    }
}
