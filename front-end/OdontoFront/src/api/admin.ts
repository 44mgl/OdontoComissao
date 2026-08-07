import type { Evento, MembroComissao, Produto, Publicacao, VariacaoProduto } from '../types/content'
import type { AdminReservation, AdminUser, DashboardData } from '../types/admin'
import type { VipProfile } from '../types/vip'
import { apiDelete, apiGet, apiPatch, apiPost, apiPut } from './client'

export const getAdminPublications = (signal?: AbortSignal) => apiGet<Publicacao[]>('/api/Publicacao', signal)
export const getAdminEvents = (signal?: AbortSignal) => apiGet<Evento[]>('/api/Evento', signal)
export const getAdminCommission = (signal?: AbortSignal) => apiGet<MembroComissao[]>('/api/MembroComissao', signal)
export const getAdminProducts = (signal?: AbortSignal) => apiGet<Produto[]>('/api/Produto', signal)
export const getAdminReservations = (signal?: AbortSignal) => apiGet<AdminReservation[]>('/api/Reserva', signal)
export const getAdminVips = (signal?: AbortSignal) => apiGet<VipProfile[]>('/api/MembroVip', signal)
export const getAdministrators = (signal?: AbortSignal) => apiGet<AdminUser[]>('/api/Administrador', signal)

export async function getDashboardData(signal?: AbortSignal): Promise<DashboardData> {
  const [publicacoes, eventos, comissao, produtos, reservas, vips, administradores] = await Promise.all([
    getAdminPublications(signal), getAdminEvents(signal), getAdminCommission(signal),
    getAdminProducts(signal), getAdminReservations(signal), getAdminVips(signal),
    getAdministrators(signal),
  ])
  return {
    publicacoes: publicacoes.length,
    eventos: eventos.length,
    comissao: comissao.length,
    produtos: produtos.length,
    reservas: reservas.length,
    vips: vips.length,
    administradores: administradores.length,
  }
}

export const createAdminResource = <TResponse, TBody>(path: string, body: TBody) =>
  apiPost<TResponse, TBody>(path, body)
export const updateAdminResource = <TBody>(path: string, body: TBody) => apiPut(path, body)
export const deleteAdminResource = (path: string) => apiDelete(path)

export const createVariation = (body: { produtoId: number; tamanho: string; quantidadeDisponivel: number }) =>
  apiPost<VariacaoProduto, typeof body>('/api/VariacaoProduto', body)
export const updateVariation = (id: number, body: { produtoId: number; tamanho: string; quantidadeDisponivel: number }) =>
  apiPut(`/api/VariacaoProduto/${id}`, body)
export const updateVariationStock = (id: number, quantidadeDisponivel: number) =>
  apiPatch(`/api/VariacaoProduto/${id}/estoque`, { quantidadeDisponivel })
export const deleteVariation = (id: number) => apiDelete(`/api/VariacaoProduto/${id}`)
export const updateReservationStatus = (id: number, status: number) =>
  apiPatch(`/api/Reserva/${id}/status`, { status })
