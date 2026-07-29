using ApiOdonto.Models;

namespace ApiOdonto.Repositories.Interfaces;

public interface IEventoRepository
{
    Task<List<Evento>> GetAllAsync();
    Task<List<Evento>> GetProximosEventosAsync();
    Task<Evento?> GetByIdAsync(int id);
    Task<Evento> CreateAsync(Evento evento);
    Task UpdateAsync(Evento evento);
    Task<bool> DeleteAsync(int id);
}
