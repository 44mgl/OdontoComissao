using ApiOdonto.Models;

namespace ApiOdonto.Repositories.Interfaces;

public interface IMembroComissaoRepository
{
    Task<List<MembroComissao>> GetAllAsync();
    Task<List<MembroComissao>> GetOrdenadosAsync();
    Task<MembroComissao?> GetByIdAsync(int id);
    Task<MembroComissao> CreateAsync(MembroComissao membro);
    Task UpdateAsync(MembroComissao membro);
    Task<bool> DeleteAsync(int id);
}
