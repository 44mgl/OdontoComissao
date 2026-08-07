import { describe, expect, it } from 'vitest'
import { formatCurrency } from './currency'

describe('formatCurrency', () => {
  it('formata valores em real brasileiro', () => {
    expect(formatCurrency(89.9)).toContain('89,90')
  })
})
