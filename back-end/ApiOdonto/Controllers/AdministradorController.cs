using ApiOdonto.DTOs.Administradores;
using ApiOdonto.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiOdonto.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class AdministradorController : ControllerBase
{
    private readonly IAdministradorService _administradorService;

    public AdministradorController(IAdministradorService administradorService)
    {
        _administradorService = administradorService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AdministradorResponseDto>>>GetAll()
    {
        var administradores = await _administradorService.GetAllAsync();

        return Ok(administradores);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AdministradorResponseDto>>GetById(int id)
    {
        var administrador = await _administradorService.GetByIdAsync(id);

        if (administrador is null)
            return NotFound();

        return Ok(administrador);
    }

    [HttpPost]
    public async Task<ActionResult<AdministradorResponseDto>>Create(CriarAdministradorDto dto)
    {
        var administrador =await _administradorService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = administrador.Id },
            administrador);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, AtualizarAdministradorDto dto)
    {
        var atualizado = await _administradorService.UpdateAsync(id, dto);

        return atualizado
            ? NoContent()
            : NotFound();
    }
}