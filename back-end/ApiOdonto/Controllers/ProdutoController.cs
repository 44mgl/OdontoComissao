using ApiOdonto.DTOs.Produtos;
using ApiOdonto.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiOdonto.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;

    public ProdutoController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProdutoResponseDto>>> GetAll()
    {
        var produtos = await _produtoService.GetAllAsync();
        return Ok(produtos);
    }

    [HttpGet("categoria")]
    public async Task<ActionResult<List<ProdutoResponseDto>>> GetByCategoria([FromQuery] string categoria)
    {
        var produtos = await _produtoService.GetByCategoriaAsync(categoria);
        return Ok(produtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProdutoResponseDto>> GetById(int id)
    {
        var produto = await _produtoService.GetByIdAsync(id);
        if (produto is null)
            return NotFound();

        return Ok(produto);
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoResponseDto>> Create([FromBody] CriarProdutoDto dto)
    {
        var produto = await _produtoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AtualizarProdutoDto dto)
    {
        var atualizado = await _produtoService.UpdateAsync(id, dto);
        return atualizado ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> AlterarStatus(int id, [FromBody] bool ativo)
    {
        var atualizado = await _produtoService.AlterarStatusAsync(id, ativo);
        return atualizado ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _produtoService.DeleteAsync(id);
        return deletado ? NoContent() : NotFound();
    }
}
