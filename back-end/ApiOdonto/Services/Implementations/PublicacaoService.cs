using ApiOdonto.DTOs.Publicacoes;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using ApiOdonto.Services.Interfaces;

namespace ApiOdonto.Services.Implementations;

public class PublicacaoService : IPublicacaoService
{
    private readonly IPublicacaoRepository _publicacaoRepository;

    public PublicacaoService(IPublicacaoRepository publicacaoRepository)
    {
        _publicacaoRepository = publicacaoRepository;
    }

    private static PublicacaoResponseDto MapToDto(Publicacao publicacao)
    {
        return new PublicacaoResponseDto
        {
            Id = publicacao.Id,
            Titulo = publicacao.Titulo,
            Descricao = publicacao.Descricao,
            ImagemUrl = publicacao.ImagemUrl,
            Tipo = publicacao.Tipo,
            OrdemExibicao = publicacao.OrdemExibicao,
            Destaque = publicacao.Destaque,
            Ativo = publicacao.Ativo,
            DataPublicacao = publicacao.DataPublicacao
        };
    }

    public async Task<List<PublicacaoResponseDto>> GetAllAsync()
    {
        var publicacoes = await _publicacaoRepository.GetAllAsync();
        return publicacoes.Select(MapToDto).ToList();
    }

    public async Task<List<PublicacaoResponseDto>> GetDestaquesAsync()
    {
        var publicacoes = await _publicacaoRepository.GetDestaquesAsync();
        return publicacoes.Select(MapToDto).ToList();
    }

    public async Task<PublicacaoResponseDto?> GetByIdAsync(int id)
    {
        var publicacao = await _publicacaoRepository.GetByIdAsync(id);
        return publicacao is null ? null : MapToDto(publicacao);
    }

    public async Task<PublicacaoResponseDto> CreateAsync(CriarPublicacaoDto dto)
    {
        var publicacao = new Publicacao
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            ImagemUrl = dto.ImagemUrl,
            Tipo = dto.Tipo,
            OrdemExibicao = dto.OrdemExibicao,
            Destaque = dto.Destaque
        };

        var publicacaoCriada = await _publicacaoRepository.CreateAsync(publicacao);
        return MapToDto(publicacaoCriada);
    }

    public async Task<bool> UpdateAsync(int id, AtualizarPublicacaoDto dto)
    {
        var publicacao = await _publicacaoRepository.GetByIdAsync(id);
        if (publicacao is null)
            return false;

        publicacao.Titulo = dto.Titulo;
        publicacao.Descricao = dto.Descricao;
        publicacao.ImagemUrl = dto.ImagemUrl;
        publicacao.Tipo = dto.Tipo;
        publicacao.OrdemExibicao = dto.OrdemExibicao;
        publicacao.Destaque = dto.Destaque;
        publicacao.Ativo = dto.Ativo;

        await _publicacaoRepository.UpdateAsync(publicacao);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _publicacaoRepository.DeleteAsync(id);
    }
}
