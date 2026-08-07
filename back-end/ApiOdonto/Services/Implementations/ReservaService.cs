using ApiOdonto.DTOs.Reservas;
using ApiOdonto.Enums;
using ApiOdonto.Exceptions;
using ApiOdonto.Models;
using ApiOdonto.Repositories.Interfaces;
using ApiOdonto.Services.Interfaces;
using System.Security.Cryptography;

namespace ApiOdonto.Services.Implementations;

public class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservaRepository;
    private readonly IVariacaoProdutoRepository _variacaoProdutoRepository;
    private readonly IMembroVipRepository _membroVipRepository;

    public ReservaService(
        IReservaRepository reservaRepository,
        IVariacaoProdutoRepository variacaoProdutoRepository,
        IMembroVipRepository membroVipRepository)
    {
        _reservaRepository = reservaRepository;
        _variacaoProdutoRepository = variacaoProdutoRepository;
        _membroVipRepository = membroVipRepository;
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

    private static ReservaPublicaResponseDto MapToPublicDto(Reserva reserva)
{
    var itens = reserva.Itens.Select(item =>
        {
            var subtotal = item.Quantidade * item.PrecoUnitario;

            return new ItemReservaPublicaResponseDto
            {
                Produto = item.VariacaoProduto.Produto.Nome,
                Tamanho =item.VariacaoProduto.Tamanho,
                Quantidade = item.Quantidade,
                PrecoUnitario = item.PrecoUnitario,
                Subtotal = subtotal
            };
        })
        .ToList();

        return new ReservaPublicaResponseDto
        {
            CodigoReserva = reserva.CodigoReserva,
            DataReserva = reserva.DataReserva,
            Status = reserva.Status,
            DataAtualizacao = reserva.DataAtualizacao,

            ValorTotal = itens.Sum(item => item.Subtotal),

            Itens = itens
        };
    }

        private static bool TransicaoStatusPermitida(
        StatusReserva statusAtual,
        StatusReserva novoStatus)
    {
        return (statusAtual, novoStatus) switch // 
        {
            (StatusReserva.Pendente, StatusReserva.Confirmada) => true,
            (StatusReserva.Pendente, StatusReserva.Cancelada) => true,

            (StatusReserva.Confirmada, StatusReserva.Separada) => true,
            (StatusReserva.Confirmada, StatusReserva.Cancelada) => true,

            (StatusReserva.Separada, StatusReserva.Retirada) => true,
            (StatusReserva.Separada, StatusReserva.Cancelada) => true,

            _ => false
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

    public async Task<ReservaPublicaResponseDto?>GetByCodigoAsync(string codigoReserva)
    {
        var codigoNormalizado = codigoReserva.Trim().ToUpperInvariant();

        var reserva = await _reservaRepository.GetByCodigoAsync(codigoNormalizado);

        return reserva is null ? null : MapToPublicDto(reserva);
    }

    public async Task<List<ReservaPublicaResponseDto>> GetByMembroVipIdAsync(int membroVipId)
    {
        var reservas = await _reservaRepository.GetByMembroVipIdAsync(membroVipId);
        return reservas.Select(MapToPublicDto).ToList();
    }

    public async Task<ReservaResponseDto> CreateAsync(CriarReservaDto dto, int? membroVipAutenticadoId)
    {
        var itensAgrupados = dto.Itens
            .GroupBy(item => item.VariacaoProdutoId) // junta itens com o mesmo VariacaoProdutoId;
            .Select(grupo => new // cria uma representação do item consolidado;
            {
                VariacaoProdutoId = grupo.Key, // contém o ID usado no agrupamento;
                Quantidade = grupo.Sum(item => item.Quantidade)
            })
            .ToList();

        var itensValidados = new List<(VariacaoProduto Variacao, int Quantidade)>();

        MembroVip? membroVip = null;

        if (membroVipAutenticadoId.HasValue)
        {
            membroVip = await _membroVipRepository
                .GetByIdAsync(membroVipAutenticadoId.Value);

            switch (membroVip)
            {
                case null:
                    throw new RegraNegocioException(
                        "O membro VIP informado não existe.");

                case { Ativo: false }:
                    throw new RegraNegocioException(
                        "O membro VIP informado está inativo.");

                case { DataValidade: null }:
                    throw new RegraNegocioException(
                        "O acesso VIP não possui validade.");

                case { DataValidade: var dataValidade }
                    when dataValidade <= DateTime.UtcNow:
                    throw new RegraNegocioException(
                        "O acesso VIP está vencido.");
            }
        }
    
        foreach (var itemDto in itensAgrupados) // Verifica os itens validados
        {
            var variacao = await _variacaoProdutoRepository
                .GetByIdAsync(itemDto.VariacaoProdutoId);

            switch (variacao)
            {
                case null:
                    throw new RegraNegocioException(
                        $"A variação {itemDto.VariacaoProdutoId} não existe.");

                case { Ativo: false }:
                    throw new RegraNegocioException(
                        $"A variação {variacao.Id} está inativa.");

                case { Produto: { Ativo: false } }:
                    throw new RegraNegocioException(
                        $"O produto {variacao.Produto.Nome} está inativo.");

                case { QuantidadeDisponivel: var estoque }
                    when estoque < itemDto.Quantidade:
                    throw new RegraNegocioException(
                        $"Estoque insuficiente para {variacao.Produto.Nome}, tamanho {variacao.Tamanho}.");

                case { Produto: { ExclusivoVip: true } }
                    when membroVip is null:
                    throw new RegraNegocioException(
                        $"O produto {variacao.Produto.Nome} é exclusivo para membros VIP.");

                default:
                    itensValidados.Add((variacao, itemDto.Quantidade));
                    break;
            }
        }

        var reserva = new Reserva
        {
            CodigoReserva = await GerarCodigoReservaAsync(),
            NomeCliente = dto.NomeCliente,
            Contato = dto.Contato,
            MembroVipId = membroVipAutenticadoId,
            Observacoes = dto.Observacoes,
            Itens = new List<ItemReserva>()
        };

        foreach (var itemValidado in itensValidados) // Cria os itens validados
        {
            var itemReserva = new ItemReserva
            {
                VariacaoProdutoId = itemValidado.Variacao.Id,
                Quantidade = itemValidado.Quantidade,
                PrecoUnitario = itemValidado.Variacao.Produto.Preco,
                Reserva = reserva,
                VariacaoProduto = itemValidado.Variacao
            };

            reserva.Itens.Add(itemReserva);
            itemValidado.Variacao.QuantidadeDisponivel -= itemValidado.Quantidade;
        }

        var reservaCriada = await _reservaRepository.CreateAsync(reserva);

        return MapToDto(reservaCriada);
    }

    public async Task<bool> UpdateStatusAsync(int id, AtualizarStatusReservaDto dto)
    {
        var reserva = await _reservaRepository.GetByIdAsync(id);

        if (reserva is null)
            return false;

        if (!TransicaoStatusPermitida(reserva.Status, dto.Status))
        {
            throw new RegraNegocioException(
                $"Não é permitido alterar uma reserva de " + $"{reserva.Status} para {dto.Status}.");
        }

        if (dto.Status == StatusReserva.Cancelada)
        {
            foreach (var item in reserva.Itens)
            {
                item.VariacaoProduto.QuantidadeDisponivel = item.VariacaoProduto.QuantidadeDisponivel + item.Quantidade;
            }
        }


        return await _reservaRepository.UpdateStatusAsync(id, dto.Status);
    }

    private async Task<string> GerarCodigoReservaAsync() // Ele é mais difícil de adivinhar e pode ser mostrado como comprovante da reserva.
    {
        string codigo;

        do
        {
            codigo = $"RES-{Convert.ToHexString(RandomNumberGenerator.GetBytes(6))}"; // Gerra bytes aleatorios
        }
        while (await _reservaRepository.CodigoExistsAsync(codigo));

        return codigo;
    }
}
