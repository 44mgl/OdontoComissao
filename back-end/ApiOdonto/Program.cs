using ApiOdonto.Data;
using Microsoft.EntityFrameworkCore;
using ApiOdonto.Repositories.Implementations;
using ApiOdonto.Repositories.Interfaces;

using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
Env.Load();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddScoped<
    IAdministradorRepository,
    AdministradorRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IMembroComissaoRepository, MembroComissaoRepository>();
builder.Services.AddScoped<IPublicacaoRepository, PublicacaoRepository>();
builder.Services.AddScoped<IMembroVipRepository, MembroVipRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IVariacaoProdutoRepository, VariacaoProdutoRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IItemReservaRepository, ItemReservaRepository>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
