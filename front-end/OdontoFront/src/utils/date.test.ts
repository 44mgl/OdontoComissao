import { describe, expect, it } from 'vitest'
import { formatDate, formatDateTime, formatTime, toBrazilDateTimeInput } from './date'

describe('formatadores de data', () => {
  const date = '2026-08-20T19:30:00'

  it('formata dia e mês em português', () => {
    expect(formatDate(date, 'day')).toBe('20')
    expect(formatDate(date, 'month')).toBe('ago')
  })

  it('formata o horário com dois dígitos', () => {
    expect(formatTime(date)).toBe('19:30')
  })

  it('converte UTC para o horário de São Paulo', () => {
    expect(formatDateTime('2026-08-07T17:08:00Z')).toContain('14:08')
  })

  it('prepara o horário brasileiro para campos datetime-local', () => {
    expect(toBrazilDateTimeInput('2026-08-07T17:08:00Z')).toBe('2026-08-07T14:08')
  })
})
