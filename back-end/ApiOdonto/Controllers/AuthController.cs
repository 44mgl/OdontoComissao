using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApiOdonto.DTOs.Autenticacao;
using ApiOdonto.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ApiOdonto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous] // deixa explícito que essa rota não exige autenticação.
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(
            LoginRequestDto dto)
        {
            var resultado = await _authService.LoginAsync(dto);

            if (resultado is null)
            {
                return Unauthorized(new
                {
                    mensagem = "E-mail ou senha inválidos."
                });
            }

            var expiresMinutesValue =
                Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES"); // O valor vem como texto porque todas as variáveis de ambiente são lidas como string

            if (!int.TryParse(expiresMinutesValue, out var expiresMinutes))
            {
                throw new InvalidOperationException(
                    "JWT_EXPIRES_MINUTES inválido ou não configurado."
                );
            }

            // Gravando o cookie
            Response.Cookies.Append( 
                "access_token", // Nome do cookie
                resultado.Token, // Valor do cookie
                new CookieOptions // Configurações do cookie
                {
                    HttpOnly = true, // Impede que o JavaScript do React leia o cookie.
                    Secure = true, // Determina que o cookie só será enviado por HTTPS.
                    SameSite = SameSiteMode.None, // Essa configuração permite que o cookie seja enviado quando frontend e backend estiverem em origens diferentes.
                    Expires = DateTimeOffset.UtcNow.AddMinutes(expiresMinutes), // Define quando o navegador deve remover o cookie.
                    Path = "/" // Significa que o cookie poderá ser enviado para todas as rotas da API:
                }
            );

            return Ok(resultado);
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(
                "access_token",
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Path = "/"
                }
            );

            return NoContent();
        }

        [Authorize]
        [HttpGet("me")] // Esse endpoint serve para testar se a autenticação está funcionando.
        public IActionResult Me()
        {
            return Ok(new
            {
                // User: É um objeto fornecido pelo ControllerBase. representa o usuário autenticado da requisição atual.
                id = User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier // Procura a Claim que você criou no TokenService:
                )?.Value,

                nome = User.Identity?.Name, // Acesse Name somente se Identity não for nulo.

                email = User.FindFirst(
                    System.Security.Claims.ClaimTypes.Email
                )?.Value,

                perfil = User.FindFirst(
                    System.Security.Claims.ClaimTypes.Role
                )?.Value
            });
        }
    }
}