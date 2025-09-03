using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace curriculo_gamer_pt2.Models.Entities
{
    public class Jogo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(128)]
        public string Nome { get; set; }
        [Required]
        [StringLength(4096)]
        public string Descricao { get; set; }
        [Required]
        public int AnoLancamento { get; set; }
    }
}
