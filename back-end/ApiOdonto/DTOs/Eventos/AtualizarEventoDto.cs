using System.ComponentModel.DataAnnotations;
using ApiOdonto.Enums;

namespace ApiOdonto.DTOs.Eventos;

public class AtualizarEventoDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data e hora são obrigatórias.")]
    public DateTime DataHora { get; set; }

    [Required(ErrorMessage = "O local é obrigatório.")]
    public string Local { get; set; } = string.Empty;

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public string Categoria { get; set; } = string.Empty;

    public StatusEvento Status { get; set; }
    public string? ImagemUrl { get; set; }
    public bool Ativo { get; set; }
}
