import { describe, expect, it } from 'vitest'
import { reservationStatus } from './reservationStatus'

describe('reservationStatus', () => {
  it('traduz todos os status da reserva', () => {
    expect(reservationStatus).toEqual({
      1: 'Pendente',
      2: 'Confirmada',
      3: 'Separada',
      4: 'Retirada',
      5: 'Cancelada',
    })
  })
})
