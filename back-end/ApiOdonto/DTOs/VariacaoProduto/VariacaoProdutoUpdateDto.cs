using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.VariacaoProduto;

public class VariacaoProdutoUpdateDto
{
    [Range(1, int.MaxValue, ErrorMessage = "O produto é obrigatório.")]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "O tamanho é obrigatório.")]
    public string Tamanho { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "A quantidade disponível não pode ser negativa.")]
    public int QuantidadeDisponivel { get; set; }
}
