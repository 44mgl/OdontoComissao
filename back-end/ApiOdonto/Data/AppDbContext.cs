using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiOdonto.Models;

namespace ApiOdonto.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Administrador> Administradores { get; set; } = null!;
        public DbSet<Evento> Eventos { get; set; } = null!; 
        public DbSet<ItemReserva> ItensReservas { get; set; } = null!;
        public DbSet<LogAdministrativo> LogsAdministrativos { get; set; } = null!;
        public DbSet<MembroComissao> MembrosComissao { get; set; } = null!; 
        public DbSet<MembroVip> MembrosVip { get; set; } = null!; 
        public DbSet<Produto> Produtos { get; set; } = null!;
        public DbSet<Publicacao> Publicacoes { get; set; } = null!;
        public DbSet<Reserva> Reservas { get; set; } = null!;
        public DbSet<VariacaoProduto> VariacoesProdutos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Não permite dois administradores com o mesmo e-mail.
            modelBuilder.Entity<Administrador>()
                .HasIndex(a => a.Email)
                .IsUnique();

            modelBuilder.Entity<MembroVip>()
                .HasIndex(m => m.Email)
                .IsUnique();    

            // Não permite dois membros VIP com a mesma identificação.
            modelBuilder.Entity<MembroVip>()
                .HasIndex(m => m.NumeroIdentificacao)
                .IsUnique();

            // Não permite duas reservas com o mesmo código.
            modelBuilder.Entity<Reserva>()
                .HasIndex(r => r.CodigoReserva)
                .IsUnique();

            // Define a precisão dos valores monetários no PostgreSQL.
            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ItemReserva>()
                .Property(i => i.PrecoUnitario)
                .HasPrecision(18, 2);

            // Não permite duas variações de um mesmo produto e mesmo tamanho.
            modelBuilder.Entity<VariacaoProduto>()
                .HasIndex(v => new
            {
                v.ProdutoId,
                v.Tamanho
            })
            .IsUnique();
        }

    }
}
