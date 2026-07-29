using System.ComponentModel.DataAnnotations;
using ApiOdonto.Enums;

namespace ApiOdonto.DTOs.Reservas;

public class AtualizarStatusReservaDto
{
    [EnumDataType(typeof(StatusReserva), ErrorMessage = "Status da reserva inválido.")]
    public StatusReserva Status { get; set; }
}
