using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace curriculo_gamer_pt2.Models.Entities
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(128)]
        public string Nome { get; set; } = default!;
        [Required]
        [StringLength(128)]
        public string Email { get; set; } = default!;
        [Required]
        [StringLength(128)]
        public string Senha { get; set; } = default!;

        [Required]
        public readonly string Role = "Admin";

        ICollection<JogoJogado> JogosJogados { get; set; } = default!;
        
    }
}
