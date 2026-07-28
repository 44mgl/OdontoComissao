namespace ApiOdonto.DTOs.ItemReserva;

public class ItemReservaResponseDto
{
    public int Id { get; set; }
    public int ReservaId { get; set; }
    public int VariacaoProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}
