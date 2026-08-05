using ApiOdonto.Models;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace ApiOdonto.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync(); // Aplica as migrações pendentes no banco de dados. Se o banco ainda não existir, ele será criado. Se já estiver atualizado, não acontece nenhuma alteração.

        if (await context.Administradores.AnyAsync())
            return;

        var nome = Environment.GetEnvironmentVariable(
            "ADMIN_INICIAL_NOME");

        var email = Environment.GetEnvironmentVariable(
            "ADMIN_INICIAL_EMAIL");

        var senha = Environment.GetEnvironmentVariable(
            "ADMIN_INICIAL_SENHA");

        if (string.IsNullOrWhiteSpace(nome) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(senha))
        {
            throw new InvalidOperationException(
                "As configurações do administrador inicial são obrigatórias.");
        }

        var administrador = new Administrador
        {
            Nome = nome.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            SenhaHash = BC.HashPassword(senha),
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };

        await context.Administradores.AddAsync(administrador);
        await context.SaveChangesAsync();
    }
}