using ApiOdonto.DTOs.Eventos;

namespace ApiOdonto.Services.Interfaces;

public interface IEventoService
{
    Task<List<EventoResponseDto>> GetAllAsync();
    Task<List<EventoResponseDto>> GetProximosEventosAsync();
    Task<EventoResponseDto?> GetByIdAsync(int id);
    Task<EventoResponseDto> CreateAsync(CriarEventoDto dto);
    Task<bool> UpdateAsync(int id, AtualizarEventoDto dto);
    Task<bool> DeleteAsync(int id);
}
