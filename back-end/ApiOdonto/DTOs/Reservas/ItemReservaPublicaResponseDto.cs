namespace ApiOdonto.DTOs.Reservas;

public class ItemReservaPublicaResponseDto
{
    public string Produto { get; set; } = string.Empty;
    public string Tamanho { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
}