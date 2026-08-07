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
