using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.DTOs;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace curriculo_gamer_pt2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JogoController : Controller
    {
        private readonly IJogoService _jogoService;
        private readonly ICategoriaService _categoriaService;
        public JogoController(IJogoService jogoService, ICategoriaService catagoriaService)
        {
            _jogoService = jogoService;
            _categoriaService = catagoriaService;
        }

        //Jogo? BuscarPorId(int id);
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Jogo? resposta = _jogoService.BuscarPorId(id);
            if (resposta == null)
            {
                return NotFound();
            }
            return View(resposta);
        }

        //Jogo Incluir(Jogo jogo);
        [HttpPost]
        public IActionResult AddJogo([FromForm] CriarJogoDto jogoDto)
        {
            if(jogoDto == null)
                return BadRequest("Dados incompletos");

            List<Categoria> categorias = _categoriaService.BuscarPorIds(jogoDto.IdsCategorias);
            if (categorias.Count == 0 || categorias.Count != jogoDto.IdsCategorias.Count)
                return BadRequest("Uma ou mais categorias não foram encontradas.");

            Jogo jogo = new Jogo
            {
                Nome = jogoDto.Nome,
                Descricao = jogoDto.Descricao,
                AnoLancamento = jogoDto.AnoLancamento,
                Categorias = categorias
            };
            _jogoService.Incluir(jogo);
            /*return CreatedAtAction(nameof(GetById), new { id = jogo.Id }, 
                new JogoDto
                {
                    Nome = jogo.Nome,
                    Descricao = jogo.Descricao,
                    AnoLancamento = jogo.AnoLancamento,
                    Categorias = jogo.Categorias.Select(c => c.Nome).ToList()
                }
                );*/
            return View("GetById", jogo);
        }

        //List<Jogo> ListarTodos();
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ListarJogos()
        {
            List<Jogo> jogos = _jogoService.ListarTodos();
            return View(jogos);
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
        public IActionResult UpdateJogo([FromBody] CriarJogoDto jogoDto)
        {
            List<Categoria> categorias = _categoriaService.BuscarPorIds(jogoDto.IdsCategorias);
            if (categorias.Count == 0 || categorias.Count != jogoDto.IdsCategorias.Count)
                return BadRequest("Uma ou mais categorias não foram encontradas.");

            Jogo jogo = new Jogo
            {
                Nome = jogoDto.Nome,
                Descricao = jogoDto.Descricao,
                AnoLancamento = jogoDto.AnoLancamento,
                Categorias = categorias
            };

            try
            {
                var jogoAtualizado = _jogoService.Atualizar(jogo);
                return Ok(new JogoDto
                {
                    Nome = jogo.Nome,
                    Descricao = jogo.Descricao,
                    AnoLancamento = jogo.AnoLancamento,
                    Categorias = jogo.Categorias.Select(c => c.Nome).ToList()
                });
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
            
        }

        [HttpPut("{id}/categorias")]
        public IActionResult AtualizarCategorias(int id, [FromBody] AtualizarCategoriaDto atualizarCategoriaDto)
        {
            var jogo = _jogoService.BuscarPorId(id);

            if(jogo == null)
                return NotFound("Jogo não encontrado");

            var categorias = _categoriaService.BuscarPorIds(atualizarCategoriaDto.CategoriaIds);

            _jogoService.AtualizarCategorias(jogo, categorias);

            return Ok(jogo);

        }

        [HttpGet("NovoJogo")]
        public IActionResult NovoJogo()
        {
            var getTodasCategorias = _categoriaService.ListarTodos().OrderBy(a => a.Nome);
            ViewBag.Categorias = getTodasCategorias;
            return View(new CriarJogoDto());
        }
    }
}
