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
            // Claims são informações declaradas sobre o usuário autenticado.
            // O valor de uma claim precisa ser em string
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, administrador.Id.ToString()),
            new Claim(ClaimTypes.Name, administrador.Nome),
            new Claim(ClaimTypes.Email, administrador.Email),
            new Claim(ClaimTypes.Role, "Administrador")
        };
            // SymmetricSecurityKey significa que a mesma chave é usada para: assinar o token; validar a assinatura do token.
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_key) // transforma a chave em um conjunto de bytes.
            );
            
            // Defini qual chave será usada e qual algoritmo fará a assinatura
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256 // serve para garantir a integridade e a autenticidade de um dado.
            );

            // Aqui o token é montado.
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expiresMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token); // WrtikeToken(token) converte esse objeto para o formatu JWT
        }
    }
}