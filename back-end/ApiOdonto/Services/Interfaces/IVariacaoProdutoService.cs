using ApiOdonto.DTOs.Variacoes;

namespace ApiOdonto.Services.Interfaces;

public interface IVariacaoProdutoService
{
    Task<List<VariacaoProdutoResponseDto>> GetAllAsync();
    Task<List<VariacaoProdutoResponseDto>> GetByProdutoIdAsync(int produtoId);
    Task<VariacaoProdutoResponseDto?> GetByIdAsync(int id);
    Task<VariacaoProdutoResponseDto> CreateAsync(CriarVariacaoProdutoDto dto);
    Task<bool> UpdateAsync(int id, CriarVariacaoProdutoDto dto);
    Task<bool> UpdateEstoqueAsync(int id, AtualizarEstoqueDto dto);
    Task<bool> DeleteAsync(int id);
}
