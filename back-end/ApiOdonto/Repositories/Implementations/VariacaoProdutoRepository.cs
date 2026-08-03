using ApiOdonto.Data;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiOdonto.Repositories.Implementations;

public class VariacaoProdutoRepository : IVariacaoProdutoRepository
{
    private readonly AppDbContext _context;

    public VariacaoProdutoRepository(AppDbContext context) => _context = context;

    public Task<List<VariacaoProduto>> GetAllAsync() =>
        _context.VariacoesProdutos.AsNoTracking()
            .OrderBy(v => v.ProdutoId).ThenBy(v => v.Tamanho).ToListAsync();

    public Task<List<VariacaoProduto>> GetByProdutoIdAsync(int produtoId) =>
        _context.VariacoesProdutos.AsNoTracking()
            .Where(v => v.ProdutoId == produtoId)
            .OrderBy(v => v.Tamanho)
            .ToListAsync();

    public Task<VariacaoProduto?> GetByIdAsync(int id) =>
        _context.VariacoesProdutos
        .Include(v => v.Produto)
        .FirstOrDefaultAsync(v => v.Id == id);

    public async Task<VariacaoProduto> CreateAsync(VariacaoProduto variacao)
    {
        await _context.VariacoesProdutos.AddAsync(variacao);
        await _context.SaveChangesAsync();
        return variacao;
    }

    public async Task UpdateAsync(VariacaoProduto variacao)
    {
        _context.VariacoesProdutos.Update(variacao);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateEstoqueAsync(
        int id,
        int quantidadeDisponivel)
    {
        if (quantidadeDisponivel < 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantidadeDisponivel),
                "A quantidade disponível não pode ser negativa.");

        var variacao = await _context.VariacoesProdutos.FindAsync(id);
        if (variacao is null)
            return false;

        variacao.QuantidadeDisponivel = quantidadeDisponivel;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var variacao = await _context.VariacoesProdutos.FindAsync(id);
        if (variacao is null)
            return false;

        variacao.Ativo = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public Task<bool> VariacaoExistsAsync(int produtoId, string tamanho, int? ignorarId = null) =>
    _context.VariacoesProdutos.AnyAsync(v =>
        v.ProdutoId == produtoId && v.Tamanho == tamanho &&
        (!ignorarId.HasValue || v.Id != ignorarId.Value));
}
