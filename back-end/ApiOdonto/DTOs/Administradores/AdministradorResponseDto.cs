namespace ApiOdonto.DTOs.Administradores;

public class AdministradorResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? UltimoAcesso { get; set; }
}