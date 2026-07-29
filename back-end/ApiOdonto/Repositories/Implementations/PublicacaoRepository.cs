using ApiOdonto.Data;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiOdonto.Repositories.Implementations;

public class PublicacaoRepository : IPublicacaoRepository
{
    private readonly AppDbContext _context;

    public PublicacaoRepository(AppDbContext context) => _context = context;

    public Task<List<Publicacao>> GetAllAsync() =>
        _context.Publicacoes.AsNoTracking()
            .OrderBy(p => p.OrdemExibicao)
            .ThenByDescending(p => p.DataPublicacao)
            .ToListAsync();

    public Task<List<Publicacao>> GetDestaquesAsync() =>
        _context.Publicacoes.AsNoTracking()
            .Where(p => p.Ativo && p.Destaque)
            .OrderBy(p => p.OrdemExibicao)
            .ThenByDescending(p => p.DataPublicacao)
            .ToListAsync();

    public Task<Publicacao?> GetByIdAsync(int id) =>
        _context.Publicacoes.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Publicacao> CreateAsync(Publicacao publicacao)
    {
        await _context.Publicacoes.AddAsync(publicacao);
        await _context.SaveChangesAsync();
        return publicacao;
    }

    public async Task UpdateAsync(Publicacao publicacao)
    {
        _context.Publicacoes.Update(publicacao);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var publicacao = await _context.Publicacoes.FindAsync(id);
        if (publicacao is null)
            return false;

        publicacao.Ativo = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
