using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.Produto;

public class ProdutoUpdateDto
{
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
    public string Descricao { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "79228162514264337593543950335",
        ErrorMessage = "O preço não pode ser negativo.")]
    public decimal Preco { get; set; }

    public string? ImagemUrl { get; set; }

    [Required(ErrorMessage = "A categoria do produto é obrigatória.")]
    public string Categoria { get; set; } = string.Empty;

    public bool ExclusivoVip { get; set; }
}
