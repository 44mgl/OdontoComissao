using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ApiOdonto.Models;

public class MembroVip
{
    public int Id { get; set; }

    public string NumeroIdentificacao { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public string SenhaHash { get; set; } = string.Empty;

    public bool Ativo { get; set; } = true;

    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    public DateTime? DataValidade { get; set; }

    // Relacionamento: um membro VIP pode possuir várias reservas.
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}