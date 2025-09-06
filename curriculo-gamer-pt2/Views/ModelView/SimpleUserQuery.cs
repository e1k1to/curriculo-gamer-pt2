using curriculo_gamer_pt2.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace curriculo_gamer_pt2.Views.ModelView
{
    public record SimpleUserQuery
    {
        public string Nome { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
