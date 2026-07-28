using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ApiOdonto.Models;

public class ItemReserva
{
    public int Id { get; set; }

    // Chave estrangeira da reserva.
    public int ReservaId { get; set; }

    // Chave estrangeira da variação escolhida.
    public int VariacaoProdutoId { get; set; }

    public int Quantidade { get; set; }

    /*
     * Guarda o preço no momento da reserva.
     * Se o preço do produto mudar posteriormente,
     * o histórico da reserva continuará correto.
     */
    public decimal PrecoUnitario { get; set; }

    public Reserva Reserva { get; set; } = null!;

    public VariacaoProduto VariacaoProduto { get; set; } = null!;
}