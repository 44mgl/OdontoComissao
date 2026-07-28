using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiOdonto.Enums;


namespace ApiOdonto.Models;

public class Evento
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public DateTime DataHora { get; set; }

    public string Local { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public StatusEvento Status { get; set; } = StatusEvento.Agendado;

    public string? ImagemUrl { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
}