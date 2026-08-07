import { describe, expect, it } from 'vitest'
import { formatDate, formatTime } from './date'

describe('formatadores de data', () => {
  const date = '2026-08-20T19:30:00'

  it('formata dia e mês em português', () => {
    expect(formatDate(date, 'day')).toBe('20')
    expect(formatDate(date, 'month')).toBe('ago')
  })

  it('formata o horário com dois dígitos', () => {
    expect(formatTime(date)).toBe('19:30')
  })
})
