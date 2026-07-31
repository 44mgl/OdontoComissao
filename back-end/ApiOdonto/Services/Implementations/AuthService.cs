using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiOdonto.DTOs.Autenticacao;
using ApiOdonto.Repositories.Interfaces;
using ApiOdonto.Services.Interfaces;


namespace ApiOdonto.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IAdministradorRepository _administradorRepository;
    private readonly ITokenService _tokenService;

    public AuthService(
        IAdministradorRepository administradorRepository,
        ITokenService tokenService)
    {
        _administradorRepository = administradorRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto?> LoginAsync(
        LoginRequestDto dto)
    {
        var administrador =
            await _administradorRepository.GetByEmailAsync(dto.Email);

        if (administrador is null)
        {
            return null;
        }

        if (!administrador.Ativo)
        {
            return null;
        }
        
        // compara uma senha em texto puro digitada pelo usuário com um hash salvo no banco de dados. 
        // Ele retorna true se as senhas forem iguais ou false se forem diferentes.
        var senhaCorreta = BCrypt.Net.BCrypt.Verify(
            dto.Senha,
            administrador.SenhaHash
        );

        if (!senhaCorreta)
        {
            return null;
        }

        var token = _tokenService.GenerateToken(administrador);

        return new LoginResponseDto
        {
            Token = token,
            Nome = administrador.Nome,
            Email = administrador.Email
        };
    }
}