using ApiOdonto.Data;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApiOdonto.Enums;

namespace ApiOdonto.Repositories.Implementations;

public class EventoRepository : IEventoRepository
{
    private readonly AppDbContext _context;

    public EventoRepository(AppDbContext context) => _context = context;

    public Task<List<Evento>> GetAllAsync() =>
        _context.Eventos.AsNoTracking().OrderBy(e => e.DataHora).ToListAsync();

    public Task<List<Evento>> GetProximosEventosAsync() =>
        _context.Eventos.AsNoTracking()
        .Where(e =>
            e.Ativo &&
            e.Status == StatusEvento.Agendado &&
            e.DataHora >= DateTime.UtcNow)
        .OrderBy(e => e.DataHora)
        .ToListAsync();

    public Task<Evento?> GetByIdAsync(int id) =>
        _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<Evento> CreateAsync(Evento evento)
    {
        await _context.Eventos.AddAsync(evento);
        await _context.SaveChangesAsync();
        return evento;
    }

    public async Task UpdateAsync(Evento evento)
    {
        _context.Eventos.Update(evento);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento is null)
            return false;

        evento.Ativo = false;
        evento.DataAtualizacao = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
