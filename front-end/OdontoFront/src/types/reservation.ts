export type CreateReservationItem = {
  variacaoProdutoId: number
  quantidade: number
}

export type CreateReservation = {
  nomeCliente: string
  contato: string
  observacoes?: string | null
  itens: CreateReservationItem[]
}

export type ReservationItemResponse = {
  id: number
  variacaoProdutoId: number
  quantidade: number
  precoUnitario: number
}

export type ReservationResponse = {
  id: number
  codigoReserva: string
  nomeCliente: string
  contato: string
  membroVipId: number | null
  dataReserva: string
  status: 1 | 2 | 3 | 4 | 5
  observacoes: string | null
  dataAtualizacao: string
  itens: ReservationItemResponse[]
}

export type PublicReservationItem = {
  produto: string
  tamanho: string
  quantidade: number
  precoUnitario: number
  subtotal: number
}

export type PublicReservation = {
  codigoReserva: string
  dataReserva: string
  status: 1 | 2 | 3 | 4 | 5
  dataAtualizacao: string
  valorTotal: number
  itens: PublicReservationItem[]
}
