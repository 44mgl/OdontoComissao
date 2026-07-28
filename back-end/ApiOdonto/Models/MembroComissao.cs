using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOdonto.Models;

public class MembroComissao
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Cargo { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public string? FotoUrl { get; set; }

    public int OrdemExibicao { get; set; }

    public bool Ativo { get; set; } = true;
}