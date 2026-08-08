const API_URL = (
  import.meta.env.VITE_API_URL ?? 'https://localhost:7103'
).replace(/\/+$/, '')

export const AUTH_UNAUTHORIZED_EVENT = 'auth:unauthorized'
export const AUTH_FORBIDDEN_EVENT = 'auth:forbidden'

type ErrorBody = {
  message?: string
  Message?: string
  mensagem?: string
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

function notifyAuthorizationFailure(status: number) {
  if (typeof window === 'undefined') {
    return
  }

  if (status === 401) {
    window.dispatchEvent(new Event(AUTH_UNAUTHORIZED_EVENT))
  }

  if (status === 403) {
    window.dispatchEvent(new Event(AUTH_FORBIDDEN_EVENT))
  }
}

async function readError(response: Response, fallbackMessage: string) {
  let body: ErrorBody = {}

  try {
    body = (await response.json()) as ErrorBody
  } catch {
    // Algumas respostas de erro podem não possuir JSON.
  }

  const validationMessage = body.errors
    ? Object.values(body.errors).flat().join(' ')
    : undefined

  notifyAuthorizationFailure(response.status)

  return new ApiError(
    validationMessage ??
      body.message ??
      body.Message ??
      body.mensagem ??
      fallbackMessage,
    response.status,
  )
}

export async function apiGet<T>(path: string, signal?: AbortSignal): Promise<T> {
  const response = await fetch(`${API_URL}${path}`, {
    credentials: 'include',
    headers: { Accept: 'application/json' },
    signal,
  })

  if (!response.ok) {
    throw await readError(response, 'Não foi possível carregar os dados.')
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
    throw await readError(response, 'Não foi possível concluir a operação.')
  }

  return (await response.json()) as TResponse
}

export async function apiPostNoContent(path: string, signal?: AbortSignal) {
  const response = await fetch(`${API_URL}${path}`, {
    method: 'POST',
    credentials: 'include',
    headers: { Accept: 'application/json' },
    signal,
  })

  if (!response.ok) {
    throw await readError(response, 'Não foi possível concluir a operação.')
  }
}

async function apiWriteNoContent<TBody>(
  method: 'PUT' | 'PATCH',
  path: string,
  body: TBody,
) {
  const response = await fetch(`${API_URL}${path}`, {
    method,
    credentials: 'include',
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(body),
  })

  if (!response.ok) {
    throw await readError(response, 'Não foi possível salvar as alterações.')
  }
}

export function apiPut<TBody>(path: string, body: TBody) {
  return apiWriteNoContent('PUT', path, body)
}

export function apiPatch<TBody>(path: string, body: TBody) {
  return apiWriteNoContent('PATCH', path, body)
}

export async function apiDelete(path: string) {
  const response = await fetch(`${API_URL}${path}`, {
    method: 'DELETE',
    credentials: 'include',
    headers: { Accept: 'application/json' },
  })

  if (!response.ok) {
    throw await readError(response, 'Não foi possível remover o registro.')
  }
}
