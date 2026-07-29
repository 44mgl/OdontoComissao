using ApiOdonto.Models;

namespace ApiOdonto.Repositories.Interfaces;

public interface IProdutoRepository
{
    Task<List<Produto>> GetAllAsync();
    Task<List<Produto>> GetByCategoriaAsync(string categoria);
    Task<Produto?> GetByIdAsync(int id);
    Task<Produto> CreateAsync(Produto produto);
    Task UpdateAsync(Produto produto);
    Task<bool> DeleteAsync(int id);
}
