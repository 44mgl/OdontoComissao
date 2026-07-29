using ApiOdonto.Enums;

namespace ApiOdonto.DTOs.Eventos;

public class EventoResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataHora { get; set; }
    public string Local { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public StatusEvento Status { get; set; }
    public string? ImagemUrl { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }
}
