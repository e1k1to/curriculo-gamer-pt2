using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace curriculo_gamer_pt2.Models.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(128)]
        public string Nome { get; set; }
        [Required]
        [StringLength(128)]
        public string Email { get; set; }
        [Required]
        [StringLength(128)]
        public string Senha { get; set; }
        ICollection<JogoJogado> JogosJogados { get; set; } = default!;
        
    }
}
