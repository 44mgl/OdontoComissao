using ApiOdonto.DTOs.Variacoes;
using ApiOdonto.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiOdonto.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VariacaoProdutoController : ControllerBase
{
    private readonly IVariacaoProdutoService _variacaoProdutoService;

    public VariacaoProdutoController(IVariacaoProdutoService variacaoProdutoService)
    {
        _variacaoProdutoService = variacaoProdutoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<VariacaoProdutoResponseDto>>> GetAll()
    {
        var variacoes = await _variacaoProdutoService.GetAllAsync();
        return Ok(variacoes);
    }

    [HttpGet("produto/{produtoId:int}")]
    public async Task<ActionResult<List<VariacaoProdutoResponseDto>>> GetByProdutoId(int produtoId)
    {
        var variacoes = await _variacaoProdutoService.GetByProdutoIdAsync(produtoId);
        return Ok(variacoes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VariacaoProdutoResponseDto>> GetById(int id)
    {
        var variacao = await _variacaoProdutoService.GetByIdAsync(id);
        if (variacao is null)
            return NotFound();

        return Ok(variacao);
    }

    [HttpPost]
    public async Task<ActionResult<VariacaoProdutoResponseDto>> Create([FromBody] CriarVariacaoProdutoDto dto)
    {
        var variacao = await _variacaoProdutoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = variacao.Id }, variacao);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CriarVariacaoProdutoDto dto)
    {
        var atualizado = await _variacaoProdutoService.UpdateAsync(id, dto);
        return atualizado ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}/estoque")]
    public async Task<IActionResult> UpdateEstoque(int id, [FromBody] AtualizarEstoqueDto dto)
    {
        var atualizado = await _variacaoProdutoService.UpdateEstoqueAsync(id, dto);
        return atualizado ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _variacaoProdutoService.DeleteAsync(id);
        return deletado ? NoContent() : NotFound();
    }
}
