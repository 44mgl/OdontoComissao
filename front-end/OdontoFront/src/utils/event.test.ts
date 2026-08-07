import { describe, expect, it } from 'vitest'
import { eventStatus, formatLongDate } from './event'

describe('utilitários de evento', () => {
  it('traduz todos os status do contrato', () => {
    expect(eventStatus).toEqual({
      1: 'Agendado',
      2: 'Em andamento',
      3: 'Concluído',
      4: 'Cancelado',
    })
  })

  it('formata uma data extensa em português', () => {
    expect(formatLongDate('2026-08-20T19:30:00')).toContain('20 de agosto de 2026')
  })
})
