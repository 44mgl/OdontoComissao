using System.ComponentModel.DataAnnotations;
using ApiOdonto.Enums;

namespace ApiOdonto.DTOs.Eventos;

public class CriarEventoDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data e hora são obrigatórias.")]
    public DateTime? DataHora { get; set; }

    [Required(ErrorMessage = "O local é obrigatório.")]
    public string Local { get; set; } = string.Empty;

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public string Categoria { get; set; } = string.Empty;

    [EnumDataType(typeof(StatusEvento),
    ErrorMessage = "Status do evento inválido.")]
    public StatusEvento Status { get; set; } = StatusEvento.Agendado;
    public string? ImagemUrl { get; set; }
}
