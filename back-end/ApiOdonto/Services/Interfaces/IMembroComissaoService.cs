using ApiOdonto.DTOs.Comissao;

namespace ApiOdonto.Services.Interfaces;

public interface IMembroComissaoService
{
    Task<List<MembroComissaoResponseDto>> GetAllAsync();
    Task<List<MembroComissaoResponseDto>> GetOrdenadosAsync();
    Task<MembroComissaoResponseDto?> GetByIdAsync(int id);
    Task<MembroComissaoResponseDto> CreateAsync(CriarMembroComissaoDto dto);
    Task<bool> UpdateAsync(int id, AtualizarMembroComissaoDto dto);
    Task<bool> DeleteAsync(int id);
}
