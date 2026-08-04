using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.Administradores;

public class AtualizarAdministradorDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; } = string.Empty;

    public bool Ativo { get; set; }

    [MinLength(8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
    public string? Senha { get; set; }
}