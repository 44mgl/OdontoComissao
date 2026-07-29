using ApiOdonto.Models;

namespace ApiOdonto.Repositories.Interfaces;

public interface IVariacaoProdutoRepository
{
    Task<List<VariacaoProduto>> GetAllAsync();
    Task<List<VariacaoProduto>> GetByProdutoIdAsync(int produtoId);
    Task<VariacaoProduto?> GetByIdAsync(int id);
    Task<VariacaoProduto> CreateAsync(VariacaoProduto variacao);
    Task UpdateAsync(VariacaoProduto variacao);
    Task<bool> UpdateEstoqueAsync(int id, int quantidadeDisponivel);
    Task<bool> DeleteAsync(int id);
}
