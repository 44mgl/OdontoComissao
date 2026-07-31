using ApiOdonto.DTOs.Reservas;

namespace ApiOdonto.Services.Interfaces;

public interface IReservaService
{
    Task<List<ReservaResponseDto>> GetAllAsync();
    Task<ReservaResponseDto?> GetByIdAsync(int id);
    Task<ReservaResponseDto?> GetByCodigoAsync(string codigoReserva);
    Task<ReservaResponseDto> CreateAsync(CriarReservaDto dto);
    Task<bool> UpdateStatusAsync(int id, AtualizarStatusReservaDto dto);
}
