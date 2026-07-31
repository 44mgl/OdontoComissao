using ApiOdonto.DTOs.VIPs;
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
        var membro = await _membroVipRepository.GetByEmailAsync(email);
        return membro is null ? null : MapToDto(membro);
    }

    public async Task<MembroVipResponseDto?> GetByNumeroIdentificacaoAsync(string numeroIdentificacao)
    {
        var membro = await _membroVipRepository.GetByNumeroIdentificacaoAsync(numeroIdentificacao);
        return membro is null ? null : MapToDto(membro);
    }

    public async Task<MembroVipResponseDto> CreateAsync(CriarMembroVipDto dto)
    {
        var membro = new MembroVip
        {
            NumeroIdentificacao = dto.NumeroIdentificacao,
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone,
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

        membro.NumeroIdentificacao = dto.NumeroIdentificacao;
        membro.Nome = dto.Nome;
        membro.Email = dto.Email;
        membro.Telefone = dto.Telefone;
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
