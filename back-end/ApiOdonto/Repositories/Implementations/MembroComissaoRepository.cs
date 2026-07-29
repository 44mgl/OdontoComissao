using ApiOdonto.Data;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiOdonto.Repositories.Implementations;

public class MembroComissaoRepository : IMembroComissaoRepository
{
    private readonly AppDbContext _context;

    public MembroComissaoRepository(AppDbContext context) => _context = context;

    public Task<List<MembroComissao>> GetAllAsync() =>
        _context.MembrosComissao.AsNoTracking()
            .OrderBy(m => m.OrdemExibicao).ToListAsync();

    public Task<List<MembroComissao>> GetOrdenadosAsync() =>
        _context.MembrosComissao.AsNoTracking()
            .Where(m => m.Ativo)
            .OrderBy(m => m.OrdemExibicao)
            .ThenBy(m => m.Nome)
            .ToListAsync();

    public Task<MembroComissao?> GetByIdAsync(int id) =>
        _context.MembrosComissao.FirstOrDefaultAsync(m => m.Id == id);

    public async Task<MembroComissao> CreateAsync(MembroComissao membro)
    {
        await _context.MembrosComissao.AddAsync(membro);
        await _context.SaveChangesAsync();
        return membro;
    }

    public async Task UpdateAsync(MembroComissao membro)
    {
        _context.MembrosComissao.Update(membro);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var membro = await _context.MembrosComissao.FindAsync(id);
        if (membro is null)
            return false;

        membro.Ativo = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
