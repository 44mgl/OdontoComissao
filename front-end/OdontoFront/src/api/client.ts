const API_URL = import.meta.env.VITE_API_URL ?? 'https://localhost:7103'

type ErrorBody = {
  message?: string
  Message?: string
  errors?: Record<string, string[]>
}

export class ApiError extends Error {
  readonly status: number

  constructor(message: string, status: number) {
    super(message)
    this.name = 'ApiError'
    this.status = status
  }
}

export async function apiGet<T>(path: string, signal?: AbortSignal): Promise<T> {
  const response = await fetch(`${API_URL}${path}`, {
    credentials: 'include',
    headers: { Accept: 'application/json' },
    signal,
  })

  if (!response.ok) {
    let body: ErrorBody = {}

    try {
      body = (await response.json()) as ErrorBody
    } catch {
      // Algumas respostas de erro podem não possuir JSON.
    }

    const validationMessage = body.errors
      ? Object.values(body.errors).flat().join(' ')
      : undefined

    throw new ApiError(
      validationMessage ??
        body.message ??
        body.Message ??
        'Não foi possível carregar os dados.',
      response.status,
    )
  }

  return (await response.json()) as T
}

export async function apiPost<TResponse, TBody>(
  path: string,
  body: TBody,
  signal?: AbortSignal,
): Promise<TResponse> {
  const response = await fetch(`${API_URL}${path}`, {
    method: 'POST',
    credentials: 'include',
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(body),
    signal,
  })

  if (!response.ok) {
    let errorBody: ErrorBody = {}
    try {
      errorBody = (await response.json()) as ErrorBody
    } catch {
      // A resposta pode não possuir um corpo JSON.
    }

    const validationMessage = errorBody.errors
      ? Object.values(errorBody.errors).flat().join(' ')
      : undefined

    throw new ApiError(
      validationMessage ?? errorBody.message ?? errorBody.Message ?? 'Não foi possível concluir a operação.',
      response.status,
    )
  }

  return (await response.json()) as TResponse
}
