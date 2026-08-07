import type { PublicReservation } from '../types/reservation'

export const reservationStatus: Record<PublicReservation['status'], string> = {
  1: 'Pendente',
  2: 'Confirmada',
  3: 'Separada',
  4: 'Retirada',
  5: 'Cancelada',
}
