using ApiOdonto.Enums;

namespace ApiOdonto.DTOs.Reserva;

public class ReservaResponseDto
{
    public int Id { get; set; }
    public string CodigoReserva { get; set; } = string.Empty;
    public string NomeCliente { get; set; } = string.Empty;
    public string Contato { get; set; } = string.Empty;
    public int? MembroVipId { get; set; }
    public DateTime DataReserva { get; set; }
    public StatusReserva Status { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataAtualizacao { get; set; }
}
