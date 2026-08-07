import { afterEach, describe, expect, it, vi } from 'vitest'
import { apiPostNoContent } from './client'

afterEach(() => {
  vi.unstubAllGlobals()
})

describe('apiPostNoContent', () => {
  it('aceita resposta 204 sem tentar interpretar JSON', async () => {
    const fetchMock = vi.fn(async () => new Response(null, { status: 204 }))
    vi.stubGlobal('fetch', fetchMock)

    await expect(apiPostNoContent('/api/Auth/logout')).resolves.toBeUndefined()
    expect(fetchMock).toHaveBeenCalledWith(
      'https://localhost:7103/api/Auth/logout',
      expect.objectContaining({
        method: 'POST',
        credentials: 'include',
      }),
    )
  })
})
