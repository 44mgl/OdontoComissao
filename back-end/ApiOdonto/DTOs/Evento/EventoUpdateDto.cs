using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ApiOdonto.Enums;

namespace ApiOdonto.DTOs.Evento
{
    public class EventoUpdateDto
    {
        [Required(ErrorMessage = "O título do evento é obrigatório.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição do evento é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data e hora do evento são obrigatórias.")]
        public DateTime DataHora { get; set; }

        [Required(ErrorMessage = "O local do evento é obrigatório.")]
        public string Local { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria do evento é obrigatória.")]
        public string Categoria { get; set; } = string.Empty;

        public StatusEvento Status { get; set; } = StatusEvento.Agendado;

        public string? ImagemUrl { get; set; }
    }
}
