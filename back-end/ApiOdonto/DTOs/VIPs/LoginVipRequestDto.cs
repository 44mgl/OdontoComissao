using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.VIPs
{
    public class LoginVipRequestDto
    {
    [Required(ErrorMessage = "O número de identificação é obrigatório.")]
    public string NumeroIdentificacao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
    public string Senha { get; set; } = string.Empty;
    }
}