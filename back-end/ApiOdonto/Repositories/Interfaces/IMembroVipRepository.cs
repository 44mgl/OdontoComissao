using ApiOdonto.Models;

namespace ApiOdonto.Repositories.Interfaces;

public interface IMembroVipRepository
{
    Task<List<MembroVip>> GetAllAsync();
    Task<MembroVip?> GetByIdAsync(int id);
    Task<MembroVip?> GetByEmailAsync(string email);
    Task<MembroVip?> GetByNumeroIdentificacaoAsync(string numeroIdentificacao);
    Task<bool> EmailExistsAsync(string email, int? ignorarId = null); // Nenhum ID foi informado para ignorar ou o registro encontrado possui outro ID
    Task<bool> NumeroIdentificacaoExistsAsync(string numeroIdentificacao, int? ignorarId = null);
    Task<MembroVip> CreateAsync(MembroVip membroVip);
    Task UpdateAsync(MembroVip membroVip);
    Task<bool> DeleteAsync(int id);
}
