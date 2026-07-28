namespace ApiOdonto.DTOs.LogAdministrativo;

public class LogAdministrativoResponseDto
{
    public int Id { get; set; }
    public int AdministradorId { get; set; }
    public string Acao { get; set; } = string.Empty;
    public string Entidade { get; set; } = string.Empty;
    public int RegistroId { get; set; }
    public DateTime DataHora { get; set; }
}
