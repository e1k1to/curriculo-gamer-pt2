using curriculo_gamer_pt2.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace curriculo_gamer_pt2.Models.Entities
{
    public class JogoJogado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int JogoId { get; set; } = default!;
        [Required]
        public int UserId { get; set; } = default!;
        [Required]
        public StatusJogoEnum StatusJogo { get; set; } = default!;
        [Required]
        public int HorasJogadas { get; set; } = default!;

        public User User { get; set; } = default!;
        public Jogo Jogo { get; set; } = default!;
    }
}
