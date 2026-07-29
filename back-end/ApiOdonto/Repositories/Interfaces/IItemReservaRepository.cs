using ApiOdonto.Models;

namespace ApiOdonto.Repositories.Interfaces;

public interface IItemReservaRepository
{
    Task<List<ItemReserva>> GetByReservaIdAsync(int reservaId);
    Task<ItemReserva?> GetByIdAsync(int id);
    Task<ItemReserva> CreateAsync(ItemReserva itemReserva);
    Task UpdateAsync(ItemReserva itemReserva);
}
