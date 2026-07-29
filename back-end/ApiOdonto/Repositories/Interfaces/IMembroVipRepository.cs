using ApiOdonto.Models;

namespace ApiOdonto.Repositories.Interfaces;

public interface IMembroVipRepository
{
    Task<List<MembroVip>> GetAllAsync();
    Task<MembroVip?> GetByIdAsync(int id);
    Task<MembroVip?> GetByEmailAsync(string email);
    Task<MembroVip?> GetByNumeroIdentificacaoAsync(string numeroIdentificacao);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> NumeroIdentificacaoExistsAsync(string numeroIdentificacao);
    Task<MembroVip> CreateAsync(MembroVip membroVip);
    Task UpdateAsync(MembroVip membroVip);
    Task<bool> DeleteAsync(int id);
}
