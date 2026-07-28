using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOdonto.Models
{
    public class LogAdministrativo
    {
        public int Id { get; set; }
        public int AdministradorId { get; set; }
        public string Acao { get; set; } = string.Empty;
        public string Entidade { get; set; } = string.Empty;
        public int RegistroId { get; set; }
        public DateTime DataHora { get; set; } = DateTime.UtcNow;
        public Administrador Administrador { get; set; } = null!;
    }
}

