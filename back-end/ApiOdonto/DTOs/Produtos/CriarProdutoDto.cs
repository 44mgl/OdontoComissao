using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.Produtos;

public class CriarProdutoDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    public string Descricao { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "79228162514264337593543950335",
        ErrorMessage = "O preço não pode ser negativo.")]
    public decimal Preco { get; set; }

    public string? ImagemUrl { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public string Categoria { get; set; } = string.Empty;

    public bool ExclusivoVip { get; set; }

    
}
