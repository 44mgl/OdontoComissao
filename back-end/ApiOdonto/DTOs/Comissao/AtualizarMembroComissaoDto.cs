using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.Comissao;

public class AtualizarMembroComissaoDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O cargo é obrigatório.")]
    public string Cargo { get; set; } = string.Empty;

    public string? Descricao { get; set; }
    public string? FotoUrl { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "A ordem de exibição não pode ser negativa.")]
    public int OrdemExibicao { get; set; }

    public bool Ativo { get; set; }
}
