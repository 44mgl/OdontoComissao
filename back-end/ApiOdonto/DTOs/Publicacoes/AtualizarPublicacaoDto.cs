using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.Publicacoes;

public class AtualizarPublicacaoDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    public string Descricao { get; set; } = string.Empty;

    public string? ImagemUrl { get; set; }

    [Required(ErrorMessage = "O tipo é obrigatório.")]
    public string Tipo { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "A ordem de exibição não pode ser negativa.")]
    public int OrdemExibicao { get; set; }

    public bool Destaque { get; set; }
    public bool Ativo { get; set; }
}
