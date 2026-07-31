using ApiOdonto.DTOs.Eventos;
using ApiOdonto.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiOdonto.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{
    private readonly IEventoService _eventoService;

    public EventoController(IEventoService eventoService)
    {
        _eventoService = eventoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EventoResponseDto>>> GetAll()
    {
        var eventos = await _eventoService.GetAllAsync();
        return Ok(eventos);
    }

    [HttpGet("proximos")]
    public async Task<ActionResult<List<EventoResponseDto>>> GetProximosEventos()
    {
        var eventos = await _eventoService.GetProximosEventosAsync();
        return Ok(eventos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventoResponseDto>> GetById(int id)
    {
        var evento = await _eventoService.GetByIdAsync(id);
        if (evento is null)
            return NotFound();

        return Ok(evento);
    }

    [HttpPost]
    public async Task<ActionResult<EventoResponseDto>> Create([FromBody] CriarEventoDto dto)
    {
        var evento = await _eventoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = evento.Id }, evento);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AtualizarEventoDto dto)
    {
        var atualizado = await _eventoService.UpdateAsync(id, dto);
        return atualizado ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _eventoService.DeleteAsync(id);
        return deletado ? NoContent() : NotFound();
    }
}
