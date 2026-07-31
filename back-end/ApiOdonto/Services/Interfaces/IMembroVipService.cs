using ApiOdonto.DTOs.VIPs;

namespace ApiOdonto.Services.Interfaces;

public interface IMembroVipService
{
    Task<List<MembroVipResponseDto>> GetAllAsync();
    Task<MembroVipResponseDto?> GetByIdAsync(int id);
    Task<MembroVipResponseDto?> GetByEmailAsync(string email);
    Task<MembroVipResponseDto?> GetByNumeroIdentificacaoAsync(string numeroIdentificacao);
    Task<MembroVipResponseDto> CreateAsync(CriarMembroVipDto dto);
    Task<bool> UpdateAsync(int id, AtualizarMembroVipDto dto);
    Task<bool> DeleteAsync(int id);
}
