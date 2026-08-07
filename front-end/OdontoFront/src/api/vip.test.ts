import { afterEach, describe, expect, it, vi } from 'vitest'
import { getVipProfile, getVipReservations } from './vip'

afterEach(() => {
  vi.unstubAllGlobals()
})

describe('API da área VIP', () => {
  it('consulta somente o perfil ligado ao token', async () => {
    const fetchMock = vi.fn(async () => new Response(JSON.stringify({ id: 7 }), {
      status: 200,
      headers: { 'Content-Type': 'application/json' },
    }))
    vi.stubGlobal('fetch', fetchMock)

    await getVipProfile()

    expect(fetchMock).toHaveBeenCalledWith(
      'https://localhost:7103/api/VipArea/perfil',
      expect.objectContaining({ credentials: 'include' }),
    )
  })

  it('consulta somente as reservas ligadas ao token', async () => {
    const fetchMock = vi.fn(async () => new Response('[]', {
      status: 200,
      headers: { 'Content-Type': 'application/json' },
    }))
    vi.stubGlobal('fetch', fetchMock)

    await getVipReservations()

    expect(fetchMock).toHaveBeenCalledWith(
      'https://localhost:7103/api/VipArea/reservas',
      expect.objectContaining({ credentials: 'include' }),
    )
  })
})
