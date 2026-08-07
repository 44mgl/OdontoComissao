import { describe, expect, it } from 'vitest'
import { validateAdminLogin, validateVipLogin } from './authValidation'

describe('validateAdminLogin', () => {
  it('exige e-mail e senha', () => {
    expect(validateAdminLogin('', '')).toEqual({
      email: 'Informe o e-mail.',
      password: 'Informe a senha.',
    })
  })

  it('rejeita e-mail inválido e senha curta', () => {
    expect(validateAdminLogin('email-invalido', '123')).toEqual({
      email: 'Informe um e-mail válido.',
      password: 'A senha deve ter pelo menos 6 caracteres.',
    })
  })

  it('aceita credenciais com formato válido', () => {
    expect(validateAdminLogin(' admin@odonto.com ', '123456')).toEqual({})
  })
})

describe('validateVipLogin', () => {
  it('exige identificação e senha', () => {
    expect(validateVipLogin('  ', '')).toEqual({
      identificationNumber: 'Informe o número de identificação.',
      password: 'Informe a senha.',
    })
  })

  it('rejeita senha curta', () => {
    expect(validateVipLogin('VIP-001', '123')).toEqual({
      password: 'A senha deve ter pelo menos 6 caracteres.',
    })
  })

  it('aceita credenciais VIP com formato válido', () => {
    expect(validateVipLogin('VIP-001', '123456')).toEqual({})
  })
})
