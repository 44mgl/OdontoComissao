using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.Variacoes;

public class CriarVariacaoProdutoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "O produto é obrigatório.")]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "O tamanho é obrigatório.")]
    public string Tamanho { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "A quantidade não pode ser negativa.")]
    public int QuantidadeDisponivel { get; set; }
}
