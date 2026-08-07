import { describe, expect, it } from 'vitest'
import { validateReservationForm } from './reservation'

describe('validateReservationForm', () => {
  it('rejeita uma reserva vazia', () => {
    expect(validateReservationForm({ nomeCliente: '', contato: '', itemCount: 0 })).toEqual({
      nomeCliente: 'Informe o nome completo.',
      contato: 'Informe um contato válido.',
      itemCount: 'Adicione pelo menos um produto.',
    })
  })

  it('aceita os campos mínimos', () => {
    expect(validateReservationForm({ nomeCliente: 'Miguel Ramos', contato: '11999999999', itemCount: 1 })).toEqual({})
  })
})
