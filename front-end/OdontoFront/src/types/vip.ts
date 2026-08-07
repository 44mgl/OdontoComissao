import type { PublicReservation } from './reservation'

export type VipProfile = {
  id: number
  numeroIdentificacao: string
  nome: string
  email: string
  telefone: string
  ativo: boolean
  dataCadastro: string
  dataValidade: string | null
}

export type VipReservation = PublicReservation
