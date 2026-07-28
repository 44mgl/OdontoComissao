using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.Administrador
{
    public class AdministradorCreateDto
    {
        [Required(ErrorMessage = " O Nome do Administrador é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = " O Email do Administrador é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string Senha { get; set; } = string.Empty;

    }
}