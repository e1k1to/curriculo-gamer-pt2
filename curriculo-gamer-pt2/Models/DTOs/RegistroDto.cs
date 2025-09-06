using Microsoft.AspNetCore.Mvc;

namespace curriculo_gamer_pt2.Models.DTOs
{
    public class RegistroDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string ConfirmarSenha { get; set; }
    }
}
