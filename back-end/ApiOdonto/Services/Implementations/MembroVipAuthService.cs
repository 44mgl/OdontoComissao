using ApiOdonto.DTOs.VIPs;
using ApiOdonto.Repositories.Interfaces;
using ApiOdonto.Services.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace ApiOdonto.Services.Implementations;

public class MembroVipAuthService : IMembroVipAuthService
{
    private readonly IMembroVipRepository _membroVipRepository;

    private readonly ITokenService _tokenService;

    public MembroVipAuthService( IMembroVipRepository membroVipRepository, ITokenService tokenService)
    {
        _membroVipRepository = membroVipRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginVipResponseDto?> LoginAsync(LoginVipRequestDto dto)
    {
        var numeroIdentificacao = dto.NumeroIdentificacao.Trim().ToUpperInvariant();

        var membroVip = await _membroVipRepository.GetByNumeroIdentificacaoAsync(numeroIdentificacao);

        if (membroVip is null)
            return null;

        if (!membroVip.Ativo)
            return null;

        if (membroVip.DataValidade is null ||
            membroVip.DataValidade <= DateTime.UtcNow)
        {
            return null;
        }

        var senhaCorreta = BC.Verify(dto.Senha, membroVip.SenhaHash);

        if (!senhaCorreta)
            return null;

        var token = _tokenService.GenerateToken(membroVip);

        return new LoginVipResponseDto
        {
            Token = token,
            MembroVipId = membroVip.Id,
            Nome = membroVip.Nome,
            NumeroIdentificacao =
            membroVip.NumeroIdentificacao
        };
    }
}