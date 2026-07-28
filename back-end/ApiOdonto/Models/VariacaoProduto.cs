using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOdonto.Models;

public class VariacaoProduto
{
    public int Id { get; set; }

    // Chave estrangeira de Produto.
    public int ProdutoId { get; set; }

    public string Tamanho { get; set; } = string.Empty;

    public int QuantidadeDisponivel { get; set; }

    public bool Ativo { get; set; } = true;

    // Produto ao qual esta variação pertence.
    public Produto Produto { get; set; } = null!;

    // Uma variação pode aparecer em vários itens de reserva.
    public ICollection<ItemReserva> ItensReserva { get; set; }
        = new List<ItemReserva>();
}