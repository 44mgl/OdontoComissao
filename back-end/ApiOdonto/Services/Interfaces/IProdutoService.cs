using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiOdonto.DTOs.Produtos;

namespace ApiOdonto.Services.Interfaces
{
    public interface IProdutoService
    {
    Task<List<ProdutoResponseDto>> GetAllAsync();
    Task<List<ProdutoResponseDto>> GetByCategoriaAsync(string categoria); 
    Task<ProdutoResponseDto?> GetByIdAsync(int id);
    Task<ProdutoResponseDto> CreateAsync(CriarProdutoDto dto);
    Task<bool> UpdateAsync(int id, AtualizarProdutoDto dto);
    Task<bool> AlterarStatusAsync(int id, bool ativo);
    Task<bool> DeleteAsync(int id);
    }
}