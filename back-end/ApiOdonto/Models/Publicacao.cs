using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOdonto.Models
{
    public class Publicacao
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string? ImagemUrl { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public int OrdemExibicao { get; set; }
        public bool Destaque { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataPublicacao { get; set; } = DateTime.UtcNow;
    }
}
