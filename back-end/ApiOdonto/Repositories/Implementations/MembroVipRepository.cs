using ApiOdonto.Data;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiOdonto.Repositories.Implementations;

public class MembroVipRepository : IMembroVipRepository
{
    private readonly AppDbContext _context;

    public MembroVipRepository(AppDbContext context) => _context = context;

    public Task<List<MembroVip>> GetAllAsync() =>
        _context.MembrosVip.AsNoTracking().OrderBy(m => m.Nome).ToListAsync();

    public Task<MembroVip?> GetByIdAsync(int id) =>
        _context.MembrosVip.FirstOrDefaultAsync(m => m.Id == id);

    public Task<MembroVip?> GetByEmailAsync(string email) =>
        _context.MembrosVip.FirstOrDefaultAsync(m => m.Email == email);

    public Task<MembroVip?> GetByNumeroIdentificacaoAsync(string numeroIdentificacao) =>
        _context.MembrosVip.FirstOrDefaultAsync(
            m => m.NumeroIdentificacao == numeroIdentificacao);

    public Task<bool> EmailExistsAsync(string email) =>
        _context.MembrosVip.AnyAsync(m => m.Email == email);

    public Task<bool> NumeroIdentificacaoExistsAsync(string numeroIdentificacao) =>
        _context.MembrosVip.AnyAsync(
            m => m.NumeroIdentificacao == numeroIdentificacao);

    public async Task<MembroVip> CreateAsync(MembroVip membroVip)
    {
        await _context.MembrosVip.AddAsync(membroVip);
        await _context.SaveChangesAsync();
        return membroVip;
    }

    public async Task UpdateAsync(MembroVip membroVip)
    {
        _context.MembrosVip.Update(membroVip);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var membroVip = await _context.MembrosVip.FindAsync(id);
        if (membroVip is null)
            return false;

        membroVip.Ativo = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
