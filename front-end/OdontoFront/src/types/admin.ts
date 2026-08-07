import type { Evento, MembroComissao, Produto, Publicacao } from './content'
import type { ReservationResponse } from './reservation'
import type { VipProfile } from './vip'

export type AdminUser = {
  id: number
  nome: string
  email: string
  ativo: boolean
  dataCriacao: string
  ultimoAcesso: string | null
}

export type AdminResource = Publicacao | Evento | MembroComissao | VipProfile | AdminUser
export type AdminReservation = ReservationResponse

export type DashboardData = {
  publicacoes: number
  eventos: number
  comissao: number
  produtos: number
  reservas: number
  vips: number
  administradores: number
}

export type AdminProduct = Produto
