using System.Text;
using ApiOdonto.Data;
using Microsoft.EntityFrameworkCore;
using ApiOdonto.Repositories.Implementations;
using ApiOdonto.Repositories.Interfaces;
using ApiOdonto.Services.Interfaces;
using ApiOdonto.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;
using ApiOdonto.Middlewares;

// No desenvolvimento, carrega as variáveis do arquivo .env quando ele existir.
// Em produção, como no Render, as configurações vêm das variáveis do ambiente.
var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

if (File.Exists(envFilePath))
{
    Env.Load(envFilePath);
}

// Obtém a URL do front-end a partir da variável de ambiente FRONTEND_URL. Se não estiver configurada, lança uma exceção.
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL")
    ?? throw new InvalidOperationException(
        "FRONTEND_URL não configurada.");
frontendUrl = frontendUrl.TrimEnd('/');


var builder = WebApplication.CreateBuilder(args);

var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
    ?? throw new InvalidOperationException("JWT_KEY não configurada.");

var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
    ?? throw new InvalidOperationException("JWT_ISSUER não configurado.");

var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
    ?? throw new InvalidOperationException("JWT_AUDIENCE não configurado.");

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new ApiOdonto.Infrastructure.Serialization.UtcDateTimeJsonConverter());
    });

// Adicionando o Cors para permitir que o front-end acesse a API.
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(frontendUrl)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Adicionando o Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "API Odonto Comissão",
        Version = "v1",
        Description = "API para gerenciamento da comissão de formatura de Odontologia."
    });
});

// Configurando o JWT no ASP.NET 
builder.Services
    .AddAuthentication(options =>
    {
        // Quando precisar identificar um usuário, utilize autenticação JWT Bearer.
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        // Quando alguém tentar acessar uma rota protegida sem estar autenticado, utilize o comportamento do JWT Bearer.
        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Verifica se o token foi emitido pela sua API.
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            // Verifica se o token foi criado para o cliente esperado.
            ValidateAudience = true,
            ValidAudience = jwtAudience,

            // Verifica se o token ainda não expirou.
            ValidateLifetime = true,

            // Verifica se a assinatura foi criada com a chave secreta correta.
            ValidateIssuerSigningKey = true,

            // define a chave secreta usada para validar a assinatura de tokens JWT em aplicações .NET
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),

            ClockSkew = TimeSpan.Zero // Por padrão, validadores JWT podem aceitar uma pequena tolerância de horário. Ao colocar zero, o token expira exatamente no horário definido.
        };

        options.Events = new JwtBearerEvents
        {
            // Em vez de procurar somente no cabeçalho, pegue o JWT do cookie chamado access_token.
            OnMessageReceived = context =>
            {
                context.Token =
                    context.Request.Cookies["access_token"];

                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorization();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Services
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IPublicacaoService, PublicacaoService>();
builder.Services.AddScoped<IMembroComissaoService, MembroComissaoService>();
builder.Services.AddScoped<IMembroVipService, MembroVipService>();
builder.Services.AddScoped<IMembroVipAuthService, MembroVipAuthService>();
builder.Services.AddScoped<IVariacaoProdutoService, VariacaoProdutoService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IAdministradorService, AdministradorService>();

// Repositories
builder.Services.AddScoped<IAdministradorRepository, AdministradorRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IMembroComissaoRepository, MembroComissaoRepository>();
builder.Services.AddScoped<IPublicacaoRepository, PublicacaoRepository>();
builder.Services.AddScoped<IMembroVipRepository, MembroVipRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IVariacaoProdutoRepository, VariacaoProdutoRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IItemReservaRepository, ItemReservaRepository>();

// Serviçoes de Autenticação
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope()) // Cria um escopo de serviço para inicializar o banco de dados e criar o administrador inicial.
{
    var context = scope.ServiceProvider // Obtém o provedor de serviços do escopo.
        .GetRequiredService<AppDbContext>(); 
    await DbInitializer.InitializeAsync(context);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "API Odonto Comissão v1"
        );
    });
}

app.UseMiddleware<ExceptionMiddleware>();

// Em produção, o Render encerra o HTTPS no proxy e encaminha HTTP ao container.
// O redirecionamento interno é necessário apenas durante o desenvolvimento local.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Rota pública usada pelo Render para verificar se a API está respondendo.
app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy"
})).AllowAnonymous();

app.Run();
