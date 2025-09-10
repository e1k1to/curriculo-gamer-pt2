using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.DTOs;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace curriculo_gamer_pt2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;
        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        //Categoria? BuscarPorId(int id);
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Categoria? resposta = _categoriaService.BuscarPorId(id);
            if (resposta == null)
            {
                return NotFound();
            }
            return Ok(resposta);
        }

        //Categoria Incluir(Jogo jogo);
        [HttpPost]
        public IActionResult IncluirCategoria([FromBody] CategoriaDto categoriaDto)
        {
            Categoria categoria = new Categoria
            {
                Nome = categoriaDto.Nome
            };
            _categoriaService.Incluir(categoria);
            return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
        }

        //List<Categoria> ListarTodos();
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ListarCategorias()
        {
            List<Categoria> categorias = _categoriaService.ListarTodos();
            return Ok(categorias);
        }

        //bool Deletar(int id);
        [HttpDelete("{id}")]
        public IActionResult DeleteCategoria(int id)
        {
            var sucesso = _categoriaService.Deletar(id);
            if(!sucesso)
                return NotFound();
            return NoContent();
        }
    }
}
