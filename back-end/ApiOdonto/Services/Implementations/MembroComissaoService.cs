using ApiOdonto.DTOs.Comissao;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using ApiOdonto.Services.Interfaces;

namespace ApiOdonto.Services.Implementations;

public class MembroComissaoService : IMembroComissaoService
{
    private readonly IMembroComissaoRepository _membroComissaoRepository;

    public MembroComissaoService(IMembroComissaoRepository membroComissaoRepository)
    {
        _membroComissaoRepository = membroComissaoRepository;
    }

    private static MembroComissaoResponseDto MapToDto(MembroComissao membro)
    {
        return new MembroComissaoResponseDto
        {
            Id = membro.Id,
            Nome = membro.Nome,
            Cargo = membro.Cargo,
            Descricao = membro.Descricao,
            FotoUrl = membro.FotoUrl,
            OrdemExibicao = membro.OrdemExibicao,
            Ativo = membro.Ativo
        };
    }

    public async Task<List<MembroComissaoResponseDto>> GetAllAsync()
    {
        var membros = await _membroComissaoRepository.GetAllAsync();
        return membros.Select(MapToDto).ToList();
    }

    public async Task<List<MembroComissaoResponseDto>> GetOrdenadosAsync()
    {
        var membros = await _membroComissaoRepository.GetOrdenadosAsync();
        return membros.Select(MapToDto).ToList();
    }

    public async Task<MembroComissaoResponseDto?> GetByIdAsync(int id)
    {
        var membro = await _membroComissaoRepository.GetByIdAsync(id);
        return membro is null ? null : MapToDto(membro);
    }

    public async Task<MembroComissaoResponseDto> CreateAsync(CriarMembroComissaoDto dto)
    {
        var membro = new MembroComissao
        {
            Nome = dto.Nome,
            Cargo = dto.Cargo,
            Descricao = dto.Descricao,
            FotoUrl = dto.FotoUrl,
            OrdemExibicao = dto.OrdemExibicao
        };

        var membroCriado = await _membroComissaoRepository.CreateAsync(membro);
        return MapToDto(membroCriado);
    }

    public async Task<bool> UpdateAsync(int id, AtualizarMembroComissaoDto dto)
    {
        var membro = await _membroComissaoRepository.GetByIdAsync(id);
        if (membro is null)
            return false;

        membro.Nome = dto.Nome;
        membro.Cargo = dto.Cargo;
        membro.Descricao = dto.Descricao;
        membro.FotoUrl = dto.FotoUrl;
        membro.OrdemExibicao = dto.OrdemExibicao;
        membro.Ativo = dto.Ativo;

        await _membroComissaoRepository.UpdateAsync(membro);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _membroComissaoRepository.DeleteAsync(id);
    }
}
