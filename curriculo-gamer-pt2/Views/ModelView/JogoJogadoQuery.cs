using curriculo_gamer_pt2.Models.Enums;
using System.Text.Json.Serialization;

namespace curriculo_gamer_pt2.Views.ModelView
{
    public record JogoJogadoQuery
    {
        public string Nome { get; set; } = default!;
        public string Descricao { get; set; } = default!;
        public int HorasJogadas { get; set; } = default!;
        public string StatusJogo { get; set; } = default!;
    }
}
