using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.Reservas;

public class CriarItemReservaDto
{
    [Range(1, int.MaxValue, ErrorMessage = "A variação do produto é obrigatória.")]
    public int VariacaoProdutoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
    public int Quantidade { get; set; }
}
