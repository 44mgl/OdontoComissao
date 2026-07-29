using ApiOdonto.Data;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiOdonto.Repositories.Implementations;

public class ItemReservaRepository : IItemReservaRepository
{
    private readonly AppDbContext _context;

    public ItemReservaRepository(AppDbContext context) => _context = context;

    public Task<List<ItemReserva>> GetByReservaIdAsync(int reservaId) =>
        _context.ItensReservas.AsNoTracking() 
            .Include(i => i.VariacaoProduto)
            .Where(i => i.ReservaId == reservaId)
            .ToListAsync();

    public Task<ItemReserva?> GetByIdAsync(int id) =>
        _context.ItensReservas.Include(i => i.VariacaoProduto)
            .FirstOrDefaultAsync(i => i.Id == id);

    public async Task<ItemReserva> CreateAsync(ItemReserva itemReserva)
    {
        await _context.ItensReservas.AddAsync(itemReserva);
        await _context.SaveChangesAsync();
        return itemReserva;
    }

    public async Task UpdateAsync(ItemReserva itemReserva)
    {
        _context.ItensReservas.Update(itemReserva);
        await _context.SaveChangesAsync();
    }
}
