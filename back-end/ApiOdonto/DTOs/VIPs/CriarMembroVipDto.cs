using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.VIPs;

public class CriarMembroVipDto
{
    [Required(ErrorMessage = "O número de identificação é obrigatório.")]
    public string NumeroIdentificacao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
    public string Senha { get; set; } = string.Empty;

    public DateTime? DataValidade { get; set; }
}
