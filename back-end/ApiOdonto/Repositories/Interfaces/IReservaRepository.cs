using ApiOdonto.Models;
using ApiOdonto.Enums;

namespace ApiOdonto.Repositories.Interfaces;

public interface IReservaRepository
{
    Task<List<Reserva>> GetAllAsync();
    Task<Reserva?> GetByIdAsync(int id);
    Task<Reserva?> GetByCodigoAsync(string codigoReserva);
    Task<List<Reserva>> GetByMembroVipIdAsync(int membroVipId);
    Task<bool> CodigoExistsAsync(string codigoReserva);
    Task<Reserva> CreateAsync(Reserva reserva);
    Task<bool> UpdateStatusAsync(int id, StatusReserva status);
}
