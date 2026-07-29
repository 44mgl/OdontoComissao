using ApiOdonto.Models;

namespace ApiOdonto.Repositories.Interfaces;

public interface IPublicacaoRepository
{
    Task<List<Publicacao>> GetAllAsync();
    Task<List<Publicacao>> GetDestaquesAsync();
    Task<Publicacao?> GetByIdAsync(int id);
    Task<Publicacao> CreateAsync(Publicacao publicacao);
    Task UpdateAsync(Publicacao publicacao);
    Task<bool> DeleteAsync(int id);
}
