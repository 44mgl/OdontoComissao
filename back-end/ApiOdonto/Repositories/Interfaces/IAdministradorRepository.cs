using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiOdonto.Models;

namespace ApiOdonto.Repositories.Interfaces
{
    public interface IAdministradorRepository
    {
        Task<List<Administrador>> GetAllAsync();

        Task<Administrador?> GetByIdAsync(int id);

        Task<Administrador?> GetByEmailAsync(string email);

        Task<bool> EmailExistsAsync(string email);

        Task<Administrador> CreateAsync(Administrador administrador);

        Task UpdateAsync(Administrador administrador);
    }
}