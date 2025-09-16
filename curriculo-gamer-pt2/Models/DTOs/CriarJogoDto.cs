using System.ComponentModel.DataAnnotations;

namespace curriculo_gamer_pt2.Models.DTOs
{
    public class CriarJogoDto
    {
        [Required(ErrorMessage = "O nome do jogo é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "A descrição do jogo é obrigatória.")]
        [StringLength(2048)]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "O ano de lançamento do jogo é obrigatório.")]
        public int AnoLancamento { get; set; }

        [Required(ErrorMessage = "É obrigatório selecionar pelo menos uma categoria.")]
        [MinLength(1)]
        public List<int> IdsCategorias { get; set; } = new List<int>();
    }
}
