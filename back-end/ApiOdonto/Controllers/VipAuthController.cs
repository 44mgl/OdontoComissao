using ApiOdonto.DTOs.VIPs;
using ApiOdonto.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiOdonto.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VipAuthController : Controller
    {
       private readonly IMembroVipAuthService _membroVipAuthService;
       public VipAuthController(IMembroVipAuthService membroVipAuthService)
       {
        _membroVipAuthService = membroVipAuthService;
       }

    [AllowAnonymous] // Permite que usuários nao autenticados acessem este endpoint
    [HttpPost("login")]
    public async Task<ActionResult<LoginVipResponseDto>>
        Login(LoginVipRequestDto dto)
    {
        var resultado = await _membroVipAuthService.LoginAsync(dto);

        if (resultado is null)
        {
            return Unauthorized(new
            {
                mensagem = "Identificação ou senha inválida."
            });
        }

        var expiresValue = Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES");

        if (!int.TryParse(expiresValue, out var expiresMinutes))
        {
            throw new InvalidOperationException("JWT_EXPIRES_MINUTES inválido ou não configurado."); 
        }

        Response.Cookies.Append("access_token", resultado.Token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, // Permite que o cookie seja enviado em requisições cross-site (necessário para o front-end hospedado em outro domínio)
                Expires = DateTimeOffset.UtcNow.AddMinutes(expiresMinutes),
                Path = "/" 
            });

        return Ok(resultado);
    }
    }
}