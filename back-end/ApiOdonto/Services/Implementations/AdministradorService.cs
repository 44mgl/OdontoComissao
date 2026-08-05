using ApiOdonto.DTOs.Administradores;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using ApiOdonto.Services.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace ApiOdonto.Services.Implementations;

public class AdministradorService : IAdministradorService
{
    private readonly IAdministradorRepository _administradorRepository;
    public AdministradorService(IAdministradorRepository administradorRepository)
    {
        _administradorRepository = administradorRepository;
    }

    private static AdministradorResponseDto MapToDto(Administrador administrador)
    {
        return new AdministradorResponseDto
        {
            Id = administrador.Id,
            Nome = administrador.Nome,
            Email = administrador.Email,
            Ativo = administrador.Ativo,
            DataCriacao = administrador.DataCriacao,
            UltimoAcesso = administrador.UltimoAcesso
        };
    }

    private static string NormalizarEmail(string email) => email.Trim().ToLowerInvariant();


    public async Task<AdministradorResponseDto> CreateAsync(CriarAdministradorDto dto)
    {
        var email = NormalizarEmail(dto.Email);

        if (await _administradorRepository
            .EmailExistsAsync(email))
        {
            throw new InvalidOperationException(
                "Já existe um administrador com esse e-mail.");
        }

        var administrador = new Administrador
        {
            Nome = dto.Nome.Trim(),
            Email = email,
            Ativo = true,
            SenhaHash = BC.HashPassword(dto.Senha),
            DataCriacao = DateTime.UtcNow
        };

        var administradorCriado = await _administradorRepository.CreateAsync(administrador);
        return MapToDto(administradorCriado);
    }

    public async Task<List<AdministradorResponseDto>> GetAllAsync()
    {
        var administradores = await _administradorRepository.GetAllAsync();
        return administradores.Select(MapToDto).ToList();
    }

    public async Task<AdministradorResponseDto?> GetByIdAsync(int id)
    {
        var administrador = await _administradorRepository.GetByIdAsync(id);
        return administrador is null ? null : MapToDto(administrador);
    }

    public async Task<bool> UpdateAsync(int id, AtualizarAdministradorDto dto)
    {
        var administrador = await _administradorRepository.GetByIdAsync(id);
        if (administrador is null)
            return false;

        var email = NormalizarEmail(dto.Email);

        if (await _administradorRepository.EmailExistsAsync(email, id))
        {
            throw new InvalidOperationException(
                "Já existe um administrador com esse e-mail.");
        }

        if (administrador.Ativo && !dto.Ativo)
        {
        var quantidadeAtivos = await _administradorRepository.CountAtivosAsync();

            if (quantidadeAtivos <= 1)
            {
                throw new InvalidOperationException(
                    "O último administrador ativo não pode ser desativado.");
            }
        
        }

        administrador.Nome = dto.Nome.Trim();
        administrador.Email = email;
        administrador.Ativo = dto.Ativo;
        if (!string.IsNullOrWhiteSpace(dto.Senha))
        {
            administrador.SenhaHash =
                BC.HashPassword(dto.Senha);
        }

        await _administradorRepository.UpdateAsync(administrador);
        return true;
    }

    
}