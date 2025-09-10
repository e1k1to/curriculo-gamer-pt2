using curriculo_gamer_pt2.Models.Entities;

namespace curriculo_gamer_pt2.Models.DTOs
{
    public class PerfilUserDto
    {
        public string Nome { get; set; }
        public List<JogoJogado> JogosJogados = new List<JogoJogado>();
    }
}
