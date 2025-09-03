using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.DTOs;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace curriculo_gamer_pt2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JogoController : ControllerBase
    {
        private readonly IJogoService _jogoService;
        public JogoController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        //Jogo? BuscarPorId(int id);
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Jogo? resposta = _jogoService.BuscarPorId(id);
            if (resposta == null)
            {
                return NotFound();
            }
            return Ok(resposta);
        }

        //Jogo Incluir(Jogo jogo);
        [HttpPost]
        public IActionResult AddJogo([FromBody] JogoDto jogoDto)
        {
            Jogo jogo = new Jogo
            {
                Nome = jogoDto.Nome,
                Descricao = jogoDto.Descricao,
                AnoLancamento = jogoDto.AnoLancamento
            };
            _jogoService.Incluir(jogo);
            return CreatedAtAction(nameof(GetById), new { id = jogo.Id }, jogo);
        }

        //List<Jogo> ListarTodos();
        [HttpGet]
        public IActionResult ListarTodos()
        {
            var lista = _jogoService.ListarTodos();
            return Ok(lista);
        }

        //bool Deletar(int id);
        [HttpDelete("{id}")]
        public IActionResult DeleteJogo(int id)
        {
            var sucesso = _jogoService.Deletar(id);
            if(!sucesso)
                return NotFound();
            return NoContent();
        }
        //Jogo Atualizar(Jogo jogo);
        [HttpPut]
        public IActionResult UpdateJogo([FromBody] JogoDto jogoDto)
        {
            Jogo jogo = new Jogo
            {
                Id = jogoDto.Id,
                Nome = jogoDto.Nome,
                Descricao = jogoDto.Descricao,
                AnoLancamento = jogoDto.AnoLancamento
            };

            try
            {
                var jogoAtualizado = _jogoService.Atualizar(jogo);
                return Ok(jogoAtualizado);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
            
        }
    }
}
