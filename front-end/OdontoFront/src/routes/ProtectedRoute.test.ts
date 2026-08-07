import { describe, expect, it } from 'vitest'
import type { SessionUser } from '../types/auth'
import { getProtectedRouteDecision } from './protectedRouteDecision'

const adminUser: SessionUser = {
  id: '1',
  nome: 'Administrador',
  email: 'admin@odonto.com',
  perfil: 'Administrador',
}

const vipUser: SessionUser = {
  id: '2',
  nome: 'Membro VIP',
  email: null,
  perfil: 'VIP',
}

describe('getProtectedRouteDecision', () => {
  it('aguarda a restauração antes de decidir', () => {
    expect(getProtectedRouteDecision('loading', null, ['Administrador'])).toBe('loading')
  })

  it('envia uma sessão ausente para o login', () => {
    expect(getProtectedRouteDecision('unauthenticated', null, ['Administrador'])).toBe('login')
  })

  it('nega acesso ao perfil incorreto', () => {
    expect(getProtectedRouteDecision('authenticated', vipUser, ['Administrador'])).toBe('denied')
  })

  it('libera o perfil autorizado', () => {
    expect(getProtectedRouteDecision('authenticated', adminUser, ['Administrador'])).toBe('allow')
  })

  it('aceita qualquer um dos perfis explicitamente permitidos', () => {
    expect(
      getProtectedRouteDecision('authenticated', adminUser, ['VIP', 'Administrador']),
    ).toBe('allow')
  })
})
