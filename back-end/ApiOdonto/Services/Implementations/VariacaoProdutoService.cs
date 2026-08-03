using ApiOdonto.DTOs.Variacoes;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using ApiOdonto.Services.Interfaces;

namespace ApiOdonto.Services.Implementations;

public class VariacaoProdutoService : IVariacaoProdutoService
{
    private readonly IVariacaoProdutoRepository _variacaoProdutoRepository;
    private readonly IProdutoRepository _produtoRepository;

    public VariacaoProdutoService(IVariacaoProdutoRepository variacaoProdutoRepository, IProdutoRepository produtoRepository)
    {
        _variacaoProdutoRepository = variacaoProdutoRepository;
        _produtoRepository = produtoRepository;
    }

    private static VariacaoProdutoResponseDto MapToDto(VariacaoProduto variacao)
    {
        return new VariacaoProdutoResponseDto
        {
            Id = variacao.Id,
            ProdutoId = variacao.ProdutoId,
            Tamanho = variacao.Tamanho,
            QuantidadeDisponivel = variacao.QuantidadeDisponivel,
            Ativo = variacao.Ativo
        };
    }

    private static string NormalizarTamanho(string tamanho) =>
    tamanho.Trim().ToUpperInvariant();

    public async Task<List<VariacaoProdutoResponseDto>> GetAllAsync()
    {
        var variacoes = await _variacaoProdutoRepository.GetAllAsync();
        return variacoes.Select(MapToDto).ToList();
    }

    public async Task<List<VariacaoProdutoResponseDto>> GetByProdutoIdAsync(int produtoId)
    {
        var variacoes = await _variacaoProdutoRepository.GetByProdutoIdAsync(produtoId);
        return variacoes.Select(MapToDto).ToList();
    }

    public async Task<VariacaoProdutoResponseDto?> GetByIdAsync(int id)
    {
        var variacao = await _variacaoProdutoRepository.GetByIdAsync(id);
        return variacao is null ? null : MapToDto(variacao);
    }

    public async Task<VariacaoProdutoResponseDto> CreateAsync(CriarVariacaoProdutoDto dto)
    {
        var tamanho = NormalizarTamanho(dto.Tamanho);

        var produto = await _produtoRepository.GetByIdAsync(dto.ProdutoId);

        if (produto is null)
        {
            throw new InvalidOperationException("O produto informado não existe.");
        }

        if (await _variacaoProdutoRepository.VariacaoExistsAsync(dto.ProdutoId, tamanho))
        {
            throw new InvalidOperationException("Esse produto já possui uma variação com esse tamanho.");
        }
        
        var variacao = new VariacaoProduto

        {
            ProdutoId = dto.ProdutoId,
            Tamanho = tamanho,
            QuantidadeDisponivel = dto.QuantidadeDisponivel
        };

        var variacaoCriada = await _variacaoProdutoRepository.CreateAsync(variacao);
        return MapToDto(variacaoCriada);
    }

    public async Task<bool> UpdateAsync(int id, CriarVariacaoProdutoDto dto)
    {
        var variacao = await _variacaoProdutoRepository.GetByIdAsync(id);
        if (variacao is null)
            return false;

            var tamanho = NormalizarTamanho(dto.Tamanho);
            var produto = await _produtoRepository.GetByIdAsync(dto.ProdutoId);

        if (produto is null)
        {
            throw new InvalidOperationException(
            "O produto informado não existe.");
        }

        if (await _variacaoProdutoRepository.VariacaoExistsAsync(dto.ProdutoId, tamanho, id))
        {
            throw new InvalidOperationException(
                "Esse produto já possui outra variação com esse tamanho.");
        }

        variacao.ProdutoId = dto.ProdutoId;
        variacao.Tamanho = tamanho;
        variacao.QuantidadeDisponivel = dto.QuantidadeDisponivel;

        await _variacaoProdutoRepository.UpdateAsync(variacao);
        return true;
    }

    public async Task<bool> UpdateEstoqueAsync(int id, AtualizarEstoqueDto dto)
    {
        return await _variacaoProdutoRepository.UpdateEstoqueAsync(id, dto.QuantidadeDisponivel);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _variacaoProdutoRepository.DeleteAsync(id);
    }

}
