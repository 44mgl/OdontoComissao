using ApiOdonto.DTOs.Administradores;

namespace ApiOdonto.Services.Interfaces;

public interface IAdministradorService
{
    Task<List<AdministradorResponseDto>>GetAllAsync();

    Task<AdministradorResponseDto?>GetByIdAsync(int id);

    Task<AdministradorResponseDto>CreateAsync(CriarAdministradorDto dto);

    Task<bool> UpdateAsync(int id, AtualizarAdministradorDto dto);
}