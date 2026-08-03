using ApiOdonto.DTOs.Reservas;
using ApiOdonto.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiOdonto.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservaController : ControllerBase
{
    private readonly IReservaService _reservaService;

    public ReservaController(IReservaService reservaService)
    {
        _reservaService = reservaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReservaResponseDto>>> GetAll()
    {
        var reservas = await _reservaService.GetAllAsync();
        return Ok(reservas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReservaResponseDto>> GetById(int id)
    {
        var reserva = await _reservaService.GetByIdAsync(id);
        if (reserva is null)
            return NotFound();

        return Ok(reserva);
    }

    [HttpGet("codigo")]
    public async Task<ActionResult<ReservaResponseDto>> GetByCodigo([FromQuery] string codigoReserva)
    {
        var reserva = await _reservaService.GetByCodigoAsync(codigoReserva);
        if (reserva is null)
            return NotFound();

        return Ok(reserva);
    }

    [HttpPost]
    public async Task<ActionResult<ReservaResponseDto>> Create([FromBody] CriarReservaDto dto)
    {
        int? membroVipAutenticadoId = null;

        if (User.IsInRole("VIP"))
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            if (!int.TryParse(idClaim, out var membroVipId))
            {
                throw new InvalidOperationException("O token VIP não possui uma identificação válida.");
            }

            membroVipAutenticadoId = membroVipId;
        }

        var reserva = await _reservaService.CreateAsync(dto, membroVipAutenticadoId);
        return CreatedAtAction(nameof(GetById), new { id = reserva.Id }, reserva);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] AtualizarStatusReservaDto dto)
    {
        var atualizado = await _reservaService.UpdateStatusAsync(id, dto);
        return atualizado ? NoContent() : NotFound();
    }
}
