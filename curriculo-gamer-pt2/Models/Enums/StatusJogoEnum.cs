using System.ComponentModel.DataAnnotations;

namespace curriculo_gamer_pt2.Models.Enums
{
    public enum StatusJogoEnum
    {
        [Display(Name = "Não iniciado")]
        NaoIniciado = 0, 
        [Display(Name = "Jogando")]
        Jogando = 1,
        [Display(Name = "Concluído")]
        Concluido = 2,
        [Display(Name = "Abandonado")]
        Abandonado = 3,
    }
}
