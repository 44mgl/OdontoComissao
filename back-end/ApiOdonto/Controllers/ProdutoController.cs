using ApiOdonto.DTOs.Produtos;
using ApiOdonto.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

    [Authorize(Roles = "Administrador")]
    [HttpGet]
    public async Task<ActionResult<List<ProdutoResponseDto>>> GetAll()
    {
        var produtos = await _produtoService.GetAllAsync();
        return Ok(produtos);
    }

    [Authorize(Roles = "Administrador")]
    [HttpGet("categoria")]
    public async Task<ActionResult<List<ProdutoResponseDto>>> GetByCategoria([FromQuery] string categoria)
    {
        var produtos = await _produtoService.GetByCategoriaAsync(categoria);
        return Ok(produtos);
    }

    [Authorize(Roles = "Administrador")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProdutoResponseDto>> GetById(int id)
    {
        var produto = await _produtoService.GetByIdAsync(id);
        if (produto is null)
            return NotFound();

        return Ok(produto);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<ProdutoResponseDto>> Create([FromBody] CriarProdutoDto dto)
    {
        var produto = await _produtoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AtualizarProdutoDto dto)
    {
        var atualizado = await _produtoService.UpdateAsync(id, dto);
        return atualizado ? NoContent() : NotFound();
    }
    
    [Authorize(Roles = "Administrador")]
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> AlterarStatus(int id, [FromBody] bool ativo)
    {
        var atualizado = await _produtoService.AlterarStatusAsync(id, ativo);
        return atualizado ? NoContent() : NotFound();
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _produtoService.DeleteAsync(id);
        return deletado ? NoContent() : NotFound();
    }

    [AllowAnonymous]
    [HttpGet("catalogo")]
    public async Task<ActionResult<List<ProdutoResponseDto>>> GetCatalogoPublico()
    {
        var produtos = await _produtoService.GetCatalogoPublicoAsync();
        return Ok(produtos);
    }
    
    [Authorize(Roles = "VIP")] // Só vai rodar se a role de quem fez a requisição for VIP
    [HttpGet("vip")]
    public async Task<ActionResult<List<ProdutoResponseDto>>>GetCatalogoVip()
    {
        var produtos = await _produtoService.GetCatalogoVipAsync();
        return Ok(produtos);
    }
}
