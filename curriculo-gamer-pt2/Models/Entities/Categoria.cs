using curriculo_gamer_pt2.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace curriculo_gamer_pt2.Models.Entities
{
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(64)]
        public string Nome { get; set; } = default!;
        

        public ICollection<Jogo> Jogos { get; set; } = new List<Jogo>();
    }
}
