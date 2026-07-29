using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.Reservas;

public class CriarReservaDto
{
    [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
    public string NomeCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "O contato é obrigatório.")]
    public string Contato { get; set; } = string.Empty;

    public int? MembroVipId { get; set; }
    public string? Observacoes { get; set; }

    [Required(ErrorMessage = "A reserva deve possuir pelo menos um item.")]
    [MinLength(1, ErrorMessage = "A reserva deve possuir pelo menos um item.")]
    public List<CriarItemReservaDto> Itens { get; set; } = [];
}
