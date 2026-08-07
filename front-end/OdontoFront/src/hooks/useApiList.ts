import { useEffect, useState } from 'react'

type ListState<T> = {
  data: T[]
  loading: boolean
  error: string | null
}

export function useApiList<T>(request: (signal: AbortSignal) => Promise<T[]>) {
  const [state, setState] = useState<ListState<T>>({
    data: [],
    loading: true,
    error: null,
  })

  useEffect(() => {
    const controller = new AbortController()

    request(controller.signal)
      .then((data) => setState({ data, loading: false, error: null }))
      .catch((error: unknown) => {
        if (error instanceof DOMException && error.name === 'AbortError') return

        setState({
          data: [],
          loading: false,
          error:
            error instanceof TypeError
              ? 'Não foi possível conectar à API.'
              : error instanceof Error
                ? error.message
                : 'Erro inesperado.',
        })
      })

    return () => controller.abort()
  }, [request])

  return state
}
