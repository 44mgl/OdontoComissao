using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.Reserva;

public class ReservaCreateDto
{
    [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
    public string NomeCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "O contato é obrigatório.")]
    public string Contato { get; set; } = string.Empty;

    public int? MembroVipId { get; set; }
    public string? Observacoes { get; set; }
}
