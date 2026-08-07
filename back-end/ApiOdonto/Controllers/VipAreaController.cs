using System.Security.Claims;
using ApiOdonto.DTOs.Reservas;
using ApiOdonto.DTOs.VIPs;
using ApiOdonto.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiOdonto.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "VIP")]
public class VipAreaController : ControllerBase
{
    private readonly IMembroVipService _membroVipService;
    private readonly IReservaService _reservaService;

    public VipAreaController(
        IMembroVipService membroVipService,
        IReservaService reservaService)
    {
        _membroVipService = membroVipService;
        _reservaService = reservaService;
    }

    [HttpGet("perfil")]
    public async Task<ActionResult<MembroVipResponseDto>> GetPerfil()
    {
        var membroVipId = GetMembroVipId();
        var membro = await _membroVipService.GetByIdAsync(membroVipId);

        return membro is null ? NotFound() : Ok(membro);
    }

    [HttpGet("reservas")]
    public async Task<ActionResult<List<ReservaPublicaResponseDto>>> GetReservas()
    {
        var membroVipId = GetMembroVipId();
        var reservas = await _reservaService.GetByMembroVipIdAsync(membroVipId);
        return Ok(reservas);
    }

    private int GetMembroVipId()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(idClaim, out var membroVipId))
        {
            throw new UnauthorizedAccessException(
                "O token VIP não possui uma identificação válida.");
        }

        return membroVipId;
    }
}
