namespace ApiOdonto.DTOs.VIPs;

public class MembroVipResponseDto
{
    public int Id { get; set; }
    public string NumeroIdentificacao { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime? DataValidade { get; set; }
}
