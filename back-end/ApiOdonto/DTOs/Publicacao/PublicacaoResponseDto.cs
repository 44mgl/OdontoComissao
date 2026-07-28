namespace ApiOdonto.DTOs.Publicacao;

public class PublicacaoResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string? ImagemUrl { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int OrdemExibicao { get; set; }
    public bool Destaque { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataPublicacao { get; set; }
}
