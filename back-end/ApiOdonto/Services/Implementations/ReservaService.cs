using ApiOdonto.DTOs.Reservas;
using ApiOdonto.Enums;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using ApiOdonto.Services.Interfaces;

namespace ApiOdonto.Services.Implementations;

public class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservaRepository;
    private readonly IItemReservaRepository _itemReservaRepository;
    private readonly IVariacaoProdutoRepository _variacaoProdutoRepository;

    public ReservaService(
        IReservaRepository reservaRepository,
        IItemReservaRepository itemReservaRepository,
        IVariacaoProdutoRepository variacaoProdutoRepository)
    {
        _reservaRepository = reservaRepository;
        _itemReservaRepository = itemReservaRepository;
        _variacaoProdutoRepository = variacaoProdutoRepository;
    }

    private static ReservaResponseDto MapToDto(Reserva reserva)
    {
        return new ReservaResponseDto
        {
            Id = reserva.Id,
            CodigoReserva = reserva.CodigoReserva,
            NomeCliente = reserva.NomeCliente,
            Contato = reserva.Contato,
            MembroVipId = reserva.MembroVipId,
            DataReserva = reserva.DataReserva,
            Status = reserva.Status,
            Observacoes = reserva.Observacoes,
            DataAtualizacao = reserva.DataAtualizacao,
            Itens = reserva.Itens.Select(i => new ItemReservaResponseDto
            {
                Id = i.Id,
                VariacaoProdutoId = i.VariacaoProdutoId,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList()
        };
    }

    public async Task<List<ReservaResponseDto>> GetAllAsync()
    {
        var reservas = await _reservaRepository.GetAllAsync();
        return reservas.Select(MapToDto).ToList();
    }

    public async Task<ReservaResponseDto?> GetByIdAsync(int id)
    {
        var reserva = await _reservaRepository.GetByIdAsync(id);
        return reserva is null ? null : MapToDto(reserva);
    }

    public async Task<ReservaResponseDto?> GetByCodigoAsync(string codigoReserva)
    {
        var reserva = await _reservaRepository.GetByCodigoAsync(codigoReserva);
        return reserva is null ? null : MapToDto(reserva);
    }

    public async Task<ReservaResponseDto> CreateAsync(CriarReservaDto dto)
    {
        var reserva = new Reserva
        {
            NomeCliente = dto.NomeCliente,
            Contato = dto.Contato,
            MembroVipId = dto.MembroVipId,
            Observacoes = dto.Observacoes,
            Itens = new List<ItemReserva>()
        };

        var reservaCriada = await _reservaRepository.CreateAsync(reserva);

        foreach (var itemDto in dto.Itens)
        {
            var variacao = await _variacaoProdutoRepository.GetByIdAsync(itemDto.VariacaoProdutoId);
            if (variacao is null)
                continue;

            var itemReserva = new ItemReserva
            {
                ReservaId = reservaCriada.Id,
                VariacaoProdutoId = itemDto.VariacaoProdutoId,
                Quantidade = itemDto.Quantidade,
                PrecoUnitario = 0,
                Reserva = reservaCriada,
                VariacaoProduto = variacao
            };

            await _itemReservaRepository.CreateAsync(itemReserva);
        }

        return MapToDto(reservaCriada);
    }

    public async Task<bool> UpdateStatusAsync(int id, AtualizarStatusReservaDto dto)
    {
        return await _reservaRepository.UpdateStatusAsync(id, dto.Status);
    }
}
