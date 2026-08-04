using ApiOdonto.Enums;

namespace ApiOdonto.DTOs.Reservas;

public class ReservaPublicaResponseDto
{
    public string CodigoReserva { get; set; } = string.Empty;
    public DateTime DataReserva { get; set; }
    public StatusReserva Status { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public decimal ValorTotal { get; set; }
    public List<ItemReservaPublicaResponseDto> Itens { get; set; } = [];
}