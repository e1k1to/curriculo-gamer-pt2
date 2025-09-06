using Microsoft.AspNetCore.Mvc;

namespace curriculo_gamer_pt2.Views.ModelView
{
    public record UserLogado 
    {
        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
