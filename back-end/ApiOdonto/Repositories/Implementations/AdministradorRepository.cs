using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiOdonto.Data;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;

namespace ApiOdonto.Repositories.Implementations
{
    public class AdministradorRepository : IAdministradorRepository
    {
        private readonly AppDbContext _context;

    public AdministradorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Administrador>> GetAllAsync()
    {
        return await _context.Administradores
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Administrador?> GetByIdAsync(int id)
    {
        return await _context.Administradores
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Administrador?> GetByEmailAsync(string email)
    {
        return await _context.Administradores
            .FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Administradores
            .AnyAsync(a => a.Email == email);
    }

    public async Task<Administrador> CreateAsync(
        Administrador administrador)
    {
        await _context.Administradores.AddAsync(administrador);
        await _context.SaveChangesAsync();

        return administrador;
    }

    public async Task UpdateAsync(Administrador administrador)
    {
        _context.Administradores.Update(administrador);
        await _context.SaveChangesAsync();
    }
    }
}