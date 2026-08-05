using ApiOdonto.DTOs.VIPs;
using ApiOdonto.Exceptions;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using ApiOdonto.Services.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace ApiOdonto.Services.Implementations;

public class MembroVipService : IMembroVipService
{
    private readonly IMembroVipRepository _membroVipRepository;

    public MembroVipService(IMembroVipRepository membroVipRepository)
    {
        _membroVipRepository = membroVipRepository;
    }

    private static MembroVipResponseDto MapToDto(MembroVip membroVip)
    {
        return new MembroVipResponseDto
        {
            Id = membroVip.Id,
            NumeroIdentificacao = membroVip.NumeroIdentificacao,
            Nome = membroVip.Nome,
            Email = membroVip.Email,
            Telefone = membroVip.Telefone,
            Ativo = membroVip.Ativo,
            DataCadastro = membroVip.DataCadastro,
            DataValidade = membroVip.DataValidade
        };
    }

    private static string NormalizarEmail(string email) => email.Trim().ToLowerInvariant();

    private static string NormalizarNumeroIdentificacao( string numeroIdentificacao) =>
    numeroIdentificacao.Trim().ToUpperInvariant();

    public async Task<List<MembroVipResponseDto>> GetAllAsync()
    {
        var membros = await _membroVipRepository.GetAllAsync();
        return membros.Select(MapToDto).ToList();
    }

    public async Task<MembroVipResponseDto?> GetByIdAsync(int id)
    {
        var membro = await _membroVipRepository.GetByIdAsync(id);
        return membro is null ? null : MapToDto(membro);
    }

    public async Task<MembroVipResponseDto?> GetByEmailAsync(string email)
    {
        var emailNormalizado = NormalizarEmail(email);
        var membro = await _membroVipRepository.GetByEmailAsync(emailNormalizado);
        return membro is null ? null : MapToDto(membro);
    }

    public async Task<MembroVipResponseDto?> GetByNumeroIdentificacaoAsync(string numeroIdentificacao)
    {
        var numeroIdentificacaoNormalizado = NormalizarNumeroIdentificacao(numeroIdentificacao);
        var membro = await _membroVipRepository.GetByNumeroIdentificacaoAsync(numeroIdentificacaoNormalizado);
        return membro is null ? null : MapToDto(membro);
    }

    public async Task<MembroVipResponseDto> CreateAsync(CriarMembroVipDto dto)
    {
        var email = NormalizarEmail(dto.Email);

        var numeroIdentificacao =
            NormalizarNumeroIdentificacao(dto.NumeroIdentificacao);

        if (await _membroVipRepository.EmailExistsAsync(email))
        {
            throw new RegraNegocioException(
                "Já existe um membro VIP com esse e-mail.");
        }

        if (await _membroVipRepository
            .NumeroIdentificacaoExistsAsync(numeroIdentificacao))
        {
            throw new RegraNegocioException(
                "Já existe um membro VIP com essa identificação.");
        }

        if (dto.DataValidade is null ||
            dto.DataValidade <= DateTime.UtcNow)
        {
            throw new RegraNegocioException(
                "A validade do membro VIP deve ser uma data futura.");
        }

        var membro = new MembroVip
        {
            NumeroIdentificacao = numeroIdentificacao,
            Nome = dto.Nome.Trim(),
            Email = email,
            Telefone = dto.Telefone.Trim(),
            SenhaHash = BC.HashPassword(dto.Senha),
            DataValidade = dto.DataValidade
        };

        var membroCriado = await _membroVipRepository.CreateAsync(membro);
        return MapToDto(membroCriado);
    }

    public async Task<bool> UpdateAsync(int id, AtualizarMembroVipDto dto)
    {
        var membro = await _membroVipRepository.GetByIdAsync(id);
        if (membro is null)
            return false;

        var email = NormalizarEmail(dto.Email);

        var numeroIdentificacao =
            NormalizarNumeroIdentificacao(dto.NumeroIdentificacao);

        if (await _membroVipRepository
            .EmailExistsAsync(email, id))
        {
            throw new RegraNegocioException(
                "Já existe outro membro VIP com esse e-mail.");
        }

        if (await _membroVipRepository
            .NumeroIdentificacaoExistsAsync(numeroIdentificacao, id))
        {
            throw new RegraNegocioException(
                "Já existe outro membro VIP com essa identificação.");
        }

        if (dto.DataValidade is null)
        {
            throw new RegraNegocioException(
                "A validade do membro VIP é obrigatória.");
        }

        if (dto.Ativo &&
            dto.DataValidade <= DateTime.UtcNow)
        {
            throw new RegraNegocioException(
                "Um membro VIP ativo deve possuir validade futura.");
        }

        membro.NumeroIdentificacao = numeroIdentificacao;
        membro.Nome = dto.Nome.Trim();
        membro.Email = email;
        membro.Telefone = dto.Telefone.Trim();
        membro.Ativo = dto.Ativo;
        membro.DataValidade = dto.DataValidade;

        if (!string.IsNullOrWhiteSpace(dto.Senha))
            membro.SenhaHash = BC.HashPassword(dto.Senha);

        await _membroVipRepository.UpdateAsync(membro);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _membroVipRepository.DeleteAsync(id);
    }
}
