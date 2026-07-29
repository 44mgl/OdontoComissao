using ApiOdonto.Data;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiOdonto.Repositories.Implementations;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context) => _context = context;

    public Task<List<Produto>> GetAllAsync() =>
        _context.Produtos.AsNoTracking()
            .Include(p => p.Variacoes)
            .OrderBy(p => p.Nome)
            .ToListAsync();

    public Task<List<Produto>> GetByCategoriaAsync(string categoria) =>
        _context.Produtos.AsNoTracking()
            .Include(p => p.Variacoes)
            .Where(p => p.Ativo && EF.Functions.Like(p.Categoria, categoria))
            .OrderBy(p => p.Nome)
            .ToListAsync();

    public Task<Produto?> GetByIdAsync(int id) =>
        _context.Produtos.Include(p => p.Variacoes)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Produto> CreateAsync(Produto produto)
    {
        await _context.Produtos.AddAsync(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task UpdateAsync(Produto produto)
    {
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto is null)
            return false;

        produto.Ativo = false;
        produto.DataAtualizacao = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
