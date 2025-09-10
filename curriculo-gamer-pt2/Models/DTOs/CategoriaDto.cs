using System.ComponentModel.DataAnnotations;

namespace curriculo_gamer_pt2.Models.DTOs
{
    public class CategoriaDto
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

    }
}
