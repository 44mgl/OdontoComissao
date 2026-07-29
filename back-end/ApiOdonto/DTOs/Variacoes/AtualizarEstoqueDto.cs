using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.Variacoes;

public class AtualizarEstoqueDto
{
    [Range(0, int.MaxValue, ErrorMessage = "A quantidade não pode ser negativa.")]
    public int QuantidadeDisponivel { get; set; }
}
