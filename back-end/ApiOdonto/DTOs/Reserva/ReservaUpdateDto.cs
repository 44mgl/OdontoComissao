using System.ComponentModel.DataAnnotations;
using ApiOdonto.Enums;

namespace ApiOdonto.DTOs.Reserva;

public class ReservaUpdateDto
{
    [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
    public string NomeCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "O contato é obrigatório.")]
    public string Contato { get; set; } = string.Empty;

    public int? MembroVipId { get; set; }
    public StatusReserva Status { get; set; } = StatusReserva.Pendente;
    public string? Observacoes { get; set; }
}
