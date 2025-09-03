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
        public int JogoId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public StatusJogo StatusJogo { get; set; }
        [Required]
        public int HorasJogadas { get; set; }
    }
}
