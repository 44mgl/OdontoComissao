using ApiOdonto.DTOs.Publicacoes;

namespace ApiOdonto.Services.Interfaces;

public interface IPublicacaoService
{
    Task<List<PublicacaoResponseDto>> GetAllAsync();
    Task<List<PublicacaoResponseDto>> GetDestaquesAsync();
    Task<PublicacaoResponseDto?> GetByIdAsync(int id);
    Task<PublicacaoResponseDto> CreateAsync(CriarPublicacaoDto dto);
    Task<bool> UpdateAsync(int id, AtualizarPublicacaoDto dto);
    Task<bool> DeleteAsync(int id);
}
