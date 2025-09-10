namespace curriculo_gamer_pt2.Models.DTOs
{
    public class JogoDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int AnoLancamento { get; set; }
        public List<string> Categorias { get; set; } = new List<string>();
    }
}
