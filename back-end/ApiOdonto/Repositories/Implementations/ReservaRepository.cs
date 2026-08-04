using ApiOdonto.Data;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using ApiOdonto.Enums;
using Microsoft.EntityFrameworkCore;

namespace ApiOdonto.Repositories.Implementations;

public class ReservaRepository : IReservaRepository
{
    private readonly AppDbContext _context;

    public ReservaRepository(AppDbContext context) => _context = context;

    public Task<List<Reserva>> GetAllAsync() =>
        _context.Reservas.AsNoTracking()
            .Include(r => r.Itens)
            .ThenInclude(i => i.VariacaoProduto)
            .OrderByDescending(r => r.DataReserva)
            .ToListAsync();

    public Task<Reserva?> GetByIdAsync(int id) =>
        _context.Reservas
            .Include(r => r.Itens)
            .ThenInclude(i => i.VariacaoProduto)
            .FirstOrDefaultAsync(r => r.Id == id);

    public Task<Reserva?> GetByCodigoAsync(string codigoReserva) =>
        _context.Reservas.AsNoTracking()
        .Include(r => r.Itens)
        .ThenInclude(i => i.VariacaoProduto)
        .ThenInclude(v => v.Produto)
        .FirstOrDefaultAsync(r => r.CodigoReserva == codigoReserva);
    public Task<bool> CodigoExistsAsync(string codigoReserva) =>
        _context.Reservas.AnyAsync(r => r.CodigoReserva == codigoReserva);

    public async Task<Reserva> CreateAsync(Reserva reserva)
    {
        await _context.Reservas.AddAsync(reserva);
        await _context.SaveChangesAsync();
        return reserva;
    }


    public async Task<bool> UpdateStatusAsync(int id, StatusReserva status)
    {
        if (!Enum.IsDefined(status))
            throw new ArgumentOutOfRangeException(
                nameof(status),
                "Status da reserva inválido.");

        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva is null)
            return false;

        reserva.Status = status;
        reserva.DataAtualizacao = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
