using ApiOdonto.DTOs.Reservas;

namespace ApiOdonto.Services.Interfaces;

public interface IReservaService
{
    Task<List<ReservaResponseDto>> GetAllAsync();
    Task<ReservaResponseDto?> GetByIdAsync(int id);
    Task<ReservaPublicaResponseDto?> GetByCodigoAsync(string codigoReserva);
    Task<List<ReservaPublicaResponseDto>> GetByMembroVipIdAsync(int membroVipId);
    Task<ReservaResponseDto> CreateAsync(CriarReservaDto dto, int? membroVipAutenticacaoId);
    Task<bool> UpdateStatusAsync(int id, AtualizarStatusReservaDto dto);
}
