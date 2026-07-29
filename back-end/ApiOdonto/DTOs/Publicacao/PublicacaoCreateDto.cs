using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.Publicacao;

public class PublicacaoCreateDto
{
    [Required(ErrorMessage = "O título da publicação é obrigatório.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição da publicação é obrigatória.")]
    public string Descricao { get; set; } = string.Empty;

    public string? ImagemUrl { get; set; }

    [Required(ErrorMessage = "O tipo da publicação é obrigatório.")]
    public string Tipo { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "A ordem de exibição não pode ser negativa.")]
    public int OrdemExibicao { get; set; }

    public bool Destaque { get; set; }
}
