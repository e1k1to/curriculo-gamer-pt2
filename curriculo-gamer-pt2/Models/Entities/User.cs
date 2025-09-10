using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace curriculo_gamer_pt2.Models.Entities
{
    public class User
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
        [NotNull]
        public string Role { get; set; } = "User";

        public ICollection<JogoJogado> JogosJogados { get; set; } = new List<JogoJogado>();

    }
}
