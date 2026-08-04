using ApiOdonto.DTOs.Comissao;
using ApiOdonto.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiOdonto.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembroComissaoController : ControllerBase
{
    private readonly IMembroComissaoService _membroComissaoService;

    public MembroComissaoController(IMembroComissaoService membroComissaoService)
    {
        _membroComissaoService = membroComissaoService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<List<MembroComissaoResponseDto>>> GetAll()
    {
        var membros = await _membroComissaoService.GetAllAsync();
        return Ok(membros);
    }

    [HttpGet("ordenados")]
    [AllowAnonymous]
    public async Task<ActionResult<List<MembroComissaoResponseDto>>> GetOrdenados()
    {
        var membros = await _membroComissaoService.GetOrdenadosAsync();
        return Ok(membros);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<MembroComissaoResponseDto>> GetById(int id)
    {
        var membro = await _membroComissaoService.GetByIdAsync(id);
        if (membro is null)
            return NotFound();

        return Ok(membro);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<MembroComissaoResponseDto>> Create([FromBody] CriarMembroComissaoDto dto)
    {
        var membro = await _membroComissaoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = membro.Id }, membro);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Update(int id, [FromBody] AtualizarMembroComissaoDto dto)
    {
        var atualizado = await _membroComissaoService.UpdateAsync(id, dto);
        return atualizado ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _membroComissaoService.DeleteAsync(id);
        return deletado ? NoContent() : NotFound();
    }
}
