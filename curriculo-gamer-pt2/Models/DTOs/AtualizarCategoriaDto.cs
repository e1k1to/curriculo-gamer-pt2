using System.ComponentModel.DataAnnotations;

namespace curriculo_gamer_pt2.Models.DTOs
{
    public class AtualizarCategoriaDto
    {
        public List<int> CategoriaIds { get; set; } = new List<int>();

    }
}
