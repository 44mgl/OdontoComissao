using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.LogAdministrativo;

public class LogAdministrativoCreateDto
{
    [Range(1, int.MaxValue, ErrorMessage = "O administrador é obrigatório.")]
    public int AdministradorId { get; set; }

    [Required(ErrorMessage = "A ação é obrigatória.")]
    public string Acao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A entidade é obrigatória.")]
    public string Entidade { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "O registro é obrigatório.")]
    public int RegistroId { get; set; }
}
