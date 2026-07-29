namespace ApiOdonto.DTOs.Autenticacao;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiracao { get; set; }
    public int AdministradorId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
