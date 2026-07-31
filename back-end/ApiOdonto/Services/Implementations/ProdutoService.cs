using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiOdonto.DTOs.Produtos;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using ApiOdonto.Services.Interfaces;

namespace ApiOdonto.Services.Implementations
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        private static ProdutoResponseDto MapToDto(Produto produto)
        {
            return new ProdutoResponseDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Categoria = produto.Categoria,
                Ativo = produto.Ativo,
                ExclusivoVip = produto.ExclusivoVip
            };
        }

        public async Task<List<ProdutoResponseDto>> GetAllAsync()
        {
            var produtos = await _produtoRepository.GetAllAsync();

            return produtos.Select(p => MapToDto(p)).ToList();
        }

        public async Task<List<ProdutoResponseDto>> GetByCategoriaAsync(string categoria)
        {
            var produtos = await _produtoRepository.GetByCategoriaAsync(categoria);

            return produtos.Select(p => MapToDto(p)).ToList();
        }

        public async Task<ProdutoResponseDto?> GetByIdAsync(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);

            if (produto is null)
                return null;

            return MapToDto(produto);
        }

        public async Task<ProdutoResponseDto> CreateAsync(CriarProdutoDto dto)
        {
            var produto = new Produto
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Preco = dto.Preco,
                ImagemUrl = dto.ImagemUrl,
                Categoria = dto.Categoria,
                ExclusivoVip = dto.ExclusivoVip
            };

            var produtoCriado = await _produtoRepository.CreateAsync(produto);

            return MapToDto(produtoCriado);
        }

        public async Task<bool> UpdateAsync(int id, AtualizarProdutoDto dto)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto is null)
                return false;

                produto.Nome = dto.Nome;
                produto.Descricao = dto.Descricao;
                produto.Preco = dto.Preco;
                produto.ImagemUrl = dto.ImagemUrl;
                produto.Categoria = dto.Categoria;
                produto.ExclusivoVip = dto.ExclusivoVip;
                produto.DataAtualizacao = DateTime.UtcNow;

            await _produtoRepository.UpdateAsync(produto);
            return true;
        }

        public async Task<bool> AlterarStatusAsync(int id, bool ativo)
        {
            return await _produtoRepository.AlterarStatusAsync(id, ativo);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _produtoRepository.DeleteAsync(id);
        }
    }
}