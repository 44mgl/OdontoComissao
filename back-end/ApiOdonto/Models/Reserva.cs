using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiOdonto.Enums;

namespace ApiOdonto.Models;

public class Reserva
{
    public int Id { get; set; }

    public string CodigoReserva { get; set; } = string.Empty;

    public string NomeCliente { get; set; } = string.Empty;

    public string Contato { get; set; } = string.Empty;

    /*
     * Esta chave estrangeira é opcional.
     * Uma reserva pública não precisa pertencer a um membro VIP.
     */
    public int? MembroVipId { get; set; }

    public DateTime DataReserva { get; set; } = DateTime.UtcNow;

    public StatusReserva Status { get; set; } = StatusReserva.Pendente;

    public string? Observacoes { get; set; }

    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    // Membro VIP relacionado, quando a reserva for VIP.
    public MembroVip? MembroVip { get; set; }

    // Uma reserva pode possuir vários itens.
    public ICollection<ItemReserva> Itens { get; set; }
        = new List<ItemReserva>();
}