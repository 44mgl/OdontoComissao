namespace ApiOdonto.DTOs.VariacaoProduto;

public class VariacaoProdutoResponseDto
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string Tamanho { get; set; } = string.Empty;
    public int QuantidadeDisponivel { get; set; }
    public bool Ativo { get; set; }
}
