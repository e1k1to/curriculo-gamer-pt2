using curriculo_gamer_pt2.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace curriculo_gamer_pt2.Models.DTOs
{
    public class AdicionarJogoJogadoDto
    {
        [Required(ErrorMessage = "Id do jogo não existe.")]
        public int JogoId { get; set; }
        [Required]
        public StatusJogoEnum StatusJogo { get; set; }

        public int HorasJogadas { get; set; } = 0;
    }
}
