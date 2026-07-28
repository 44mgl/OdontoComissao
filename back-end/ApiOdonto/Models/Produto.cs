using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOdonto.Models;

public class Produto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public decimal Preco { get; set; }

    public string? ImagemUrl { get; set; }

    public string Categoria { get; set; } = string.Empty;

    public bool ExclusivoVip { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    // Relacionamento: um produto pode possuir várias variações.
    public ICollection<VariacaoProduto> Variacoes { get; set; }
        = new List<VariacaoProduto>();
}