using ApiOdonto.DTOs.VIPs;
using ApiOdonto.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiOdonto.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembroVipController : ControllerBase
{
    private readonly IMembroVipService _membroVipService;

    public MembroVipController(IMembroVipService membroVipService)
    {
        _membroVipService = membroVipService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MembroVipResponseDto>>> GetAll()
    {
        var membros = await _membroVipService.GetAllAsync();
        return Ok(membros);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MembroVipResponseDto>> GetById(int id)
    {
        var membro = await _membroVipService.GetByIdAsync(id);
        if (membro is null)
            return NotFound();

        return Ok(membro);
    }

    [HttpGet("email")]
    public async Task<ActionResult<MembroVipResponseDto>> GetByEmail([FromQuery] string email)
    {
        var membro = await _membroVipService.GetByEmailAsync(email);
        if (membro is null)
            return NotFound();

        return Ok(membro);
    }

    [HttpGet("identificacao")]
    public async Task<ActionResult<MembroVipResponseDto>> GetByNumeroIdentificacao([FromQuery] string numeroIdentificacao)
    {
        var membro = await _membroVipService.GetByNumeroIdentificacaoAsync(numeroIdentificacao);
        if (membro is null)
            return NotFound();

        return Ok(membro);
    }

    [HttpPost]
    public async Task<ActionResult<MembroVipResponseDto>> Create([FromBody] CriarMembroVipDto dto)
    {
        var membro = await _membroVipService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = membro.Id }, membro);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AtualizarMembroVipDto dto)
    {
        var atualizado = await _membroVipService.UpdateAsync(id, dto);
        return atualizado ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _membroVipService.DeleteAsync(id);
        return deletado ? NoContent() : NotFound();
    }
}
