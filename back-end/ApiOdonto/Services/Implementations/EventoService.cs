using ApiOdonto.DTOs.Eventos;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using ApiOdonto.Services.Interfaces;

namespace ApiOdonto.Services.Implementations;

public class EventoService : IEventoService
{
    private readonly IEventoRepository _eventoRepository;

    public EventoService(IEventoRepository eventoRepository)
    {
        _eventoRepository = eventoRepository;
    }

    private static EventoResponseDto MapToDto(Evento evento)
    {
        return new EventoResponseDto
        {
            Id = evento.Id,
            Titulo = evento.Titulo,
            Descricao = evento.Descricao,
            DataHora = evento.DataHora,
            Local = evento.Local,
            Categoria = evento.Categoria,
            Status = evento.Status,
            ImagemUrl = evento.ImagemUrl,
            Ativo = evento.Ativo,
            DataCriacao = evento.DataCriacao,
            DataAtualizacao = evento.DataAtualizacao
        };
    }

    public async Task<List<EventoResponseDto>> GetAllAsync()
    {
        var eventos = await _eventoRepository.GetAllAsync();
        return eventos.Select(MapToDto).ToList();
    }

    public async Task<List<EventoResponseDto>> GetProximosEventosAsync()
    {
        var eventos = await _eventoRepository.GetProximosEventosAsync();
        return eventos.Select(MapToDto).ToList();
    }

    public async Task<EventoResponseDto?> GetByIdAsync(int id)
    {
        var evento = await _eventoRepository.GetByIdAsync(id);
        return evento is null ? null : MapToDto(evento);
    }

    public async Task<EventoResponseDto> CreateAsync(CriarEventoDto dto)
    {
        var evento = new Evento
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            DataHora = dto.DataHora,
            Local = dto.Local,
            Categoria = dto.Categoria,
            Status = dto.Status,
            ImagemUrl = dto.ImagemUrl
        };

        var eventoCriado = await _eventoRepository.CreateAsync(evento);
        return MapToDto(eventoCriado);
    }

    public async Task<bool> UpdateAsync(int id, AtualizarEventoDto dto)
    {
        var evento = await _eventoRepository.GetByIdAsync(id);
        if (evento is null)
            return false;

        evento.Titulo = dto.Titulo;
        evento.Descricao = dto.Descricao;
        evento.DataHora = dto.DataHora;
        evento.Local = dto.Local;
        evento.Categoria = dto.Categoria;
        evento.Status = dto.Status;
        evento.ImagemUrl = dto.ImagemUrl;
        evento.Ativo = dto.Ativo;
        evento.DataAtualizacao = DateTime.UtcNow;

        await _eventoRepository.UpdateAsync(evento);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _eventoRepository.DeleteAsync(id);
    }
}
