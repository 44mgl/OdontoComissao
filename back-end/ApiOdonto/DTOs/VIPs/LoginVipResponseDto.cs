namespace ApiOdonto.DTOs.VIPs
{
    public class LoginVipResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int MembroVipId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string NumeroIdentificacao { get; set; } = string.Empty;
    }
}