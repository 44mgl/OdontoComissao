using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiOdonto.Models;
using ApiOdonto.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;


namespace ApiOdonto.Services.Implementations
{
    //cuida exclusivamente da criação do JWT.
    public class TokenService : ITokenService 
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiresMinutes;

        
        private string GenerateToken(IEnumerable<Claim> claims)
        {
            // Cria a chave de segurança a partir da chave secreta
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_key));

            // Cria as credenciais de assinatura usando a chave e o algoritmo HMAC-SHA256
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            // Cria o token JWT 
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expiresMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
            .WriteToken(token);
        }

        public TokenService()
        {
            // usa o Environment.GetEnvironmentVariable() para procurar a variavel no ambiente .env
            _key = Environment.GetEnvironmentVariable("JWT_KEY") 
                ?? throw new InvalidOperationException("JWT_KEY não configurada.");

            _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                ?? throw new InvalidOperationException("JWT_ISSUER não configurado.");

            _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                ?? throw new InvalidOperationException("JWT_AUDIENCE não configurado.");

            var expiresValue =
                Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES");

            if (!int.TryParse(expiresValue, out _expiresMinutes)) // Converte o Expires_Minutes em int. A vantagem do TryParse é que ele não quebra imediatamente por causa de uma conversão inválida. Você pode tratar o erro de maneira controlada.
            {
                throw new InvalidOperationException(
                    "JWT_EXPIRES_MINUTES inválido ou não configurado."
                );
            }
        }

        public string GenerateToken(Administrador administrador)
        {
            var claims = new List<Claim> // Claims são informações colocadas dentro do token e protegidas pela assinatura.
          {
            new(ClaimTypes.NameIdentifier, administrador.Id.ToString()),
            new(ClaimTypes.Name, administrador.Nome),
            new(ClaimTypes.Email, administrador.Email),
            new(ClaimTypes.Role, "Administrador")
          };

            return GenerateToken(claims);
        }

        public string GenerateToken(MembroVip membroVip)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, membroVip.Id.ToString()),
                new(ClaimTypes.Name, membroVip.Nome),
                new("numeroIdentificacao", membroVip.NumeroIdentificacao),
                new(ClaimTypes.Role, "VIP")
            };

            return GenerateToken(claims);
        }

    }
}