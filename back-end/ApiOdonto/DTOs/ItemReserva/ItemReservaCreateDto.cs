using System.ComponentModel.DataAnnotations;

namespace ApiOdonto.DTOs.ItemReserva;

public class ItemReservaCreateDto
{
    [Range(1, int.MaxValue, ErrorMessage = "A reserva é obrigatória.")]
    public int ReservaId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A variação do produto é obrigatória.")]
    public int VariacaoProdutoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
    public int Quantidade { get; set; }

    [Range(typeof(decimal), "0", "79228162514264337593543950335",
        ErrorMessage = "O preço unitário não pode ser negativo.")]
    public decimal PrecoUnitario { get; set; }
}
