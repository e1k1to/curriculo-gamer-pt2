using curriculo_gamer_pt2.Models.Enums;

namespace curriculo_gamer_pt2.Models.DTOs
{
    public class JogoJogadoDto
    {
        public int Id { get; set; }
        public int JogoId { get; set; }
        public int UserId { get; set; }
        public StatusJogoEnum StatusJogo { get; set; }
        public int HorasJogadas { get; set; }
    }
}
