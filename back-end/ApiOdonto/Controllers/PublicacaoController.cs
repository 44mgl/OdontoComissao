using ApiOdonto.DTOs.Publicacoes;
using ApiOdonto.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiOdonto.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublicacaoController : ControllerBase
{
    private readonly IPublicacaoService _publicacaoService;

    public PublicacaoController(IPublicacaoService publicacaoService)
    {
        _publicacaoService = publicacaoService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<List<PublicacaoResponseDto>>> GetAll()
    {
        var publicacoes = await _publicacaoService.GetAllAsync();
        return Ok(publicacoes);
    }

    [HttpGet("destaques")]
    [AllowAnonymous]
    public async Task<ActionResult<List<PublicacaoResponseDto>>> GetDestaques()
    {
        var publicacoes = await _publicacaoService.GetDestaquesAsync();
        return Ok(publicacoes);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<PublicacaoResponseDto>> GetById(int id)
    {
        var publicacao = await _publicacaoService.GetByIdAsync(id);
        if (publicacao is null)
            return NotFound();

        return Ok(publicacao);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<PublicacaoResponseDto>> Create([FromBody] CriarPublicacaoDto dto)
    {
        var publicacao = await _publicacaoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = publicacao.Id }, publicacao);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Update(int id, [FromBody] AtualizarPublicacaoDto dto)
    {
        var atualizado = await _publicacaoService.UpdateAsync(id, dto);
        return atualizado ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _publicacaoService.DeleteAsync(id);
        return deletado ? NoContent() : NotFound();
    }
}
