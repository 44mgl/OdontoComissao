import {
  useCallback,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from 'react'
import {
  authenticateAdmin,
  authenticateVip,
  endSession,
  getCurrentUser,
} from '../api/auth'
import { ApiError, AUTH_UNAUTHORIZED_EVENT } from '../api/client'
import type { SessionStatus, SessionUser } from '../types/auth'
import { AuthContext } from './auth-context'

type AuthProviderProps = {
  children: ReactNode
}

export function AuthProvider({ children }: AuthProviderProps) {
  const [user, setUser] = useState<SessionUser | null>(null)
  const [status, setStatus] = useState<SessionStatus>('loading')
  const [error, setError] = useState<string | null>(null)

  const refreshSession = useCallback(async (signal?: AbortSignal) => {
    setStatus('loading')
    setError(null)

    try {
      const currentUser = await getCurrentUser(signal)
      setUser(currentUser)
      setStatus('authenticated')
      return currentUser
    } catch (requestError) {
      if (requestError instanceof DOMException && requestError.name === 'AbortError') {
        return null
      }

      setUser(null)

      if (requestError instanceof ApiError && requestError.status === 401) {
        setStatus('unauthenticated')
        return null
      }

      setStatus('unauthenticated')
      setError(
        requestError instanceof Error
          ? requestError.message
          : 'Não foi possível verificar a sessão.',
      )
      return null
    }
  }, [])

  const loginAdmin = useCallback(
    async (email: string, password: string) => {
      await authenticateAdmin({ email: email.trim(), senha: password })
      const currentUser = await refreshSession()

      if (currentUser?.perfil !== 'Administrador') {
        throw new Error('A API autenticou o usuário, mas não restaurou a sessão administrativa.')
      }
    },
    [refreshSession],
  )

  const loginVip = useCallback(
    async (identificationNumber: string, password: string) => {
      await authenticateVip({
        numeroIdentificacao: identificationNumber.trim(),
        senha: password,
      })
      const currentUser = await refreshSession()

      if (currentUser?.perfil !== 'VIP') {
        throw new Error('A API autenticou o usuário, mas não restaurou a sessão VIP.')
      }
    },
    [refreshSession],
  )

  const clearSession = useCallback(() => {
    setUser(null)
    setStatus('unauthenticated')
    setError(null)
  }, [])

  const logout = useCallback(async () => {
    try {
      await endSession()
      clearSession()
    } catch (requestError) {
      if (requestError instanceof ApiError && requestError.status === 401) {
        clearSession()
        return
      }

      throw requestError
    }
  }, [clearSession])

  useEffect(() => {
    const controller = new AbortController()
    void refreshSession(controller.signal)

    return () => controller.abort()
  }, [refreshSession])

  useEffect(() => {
    window.addEventListener(AUTH_UNAUTHORIZED_EVENT, clearSession)
    return () => window.removeEventListener(AUTH_UNAUTHORIZED_EVENT, clearSession)
  }, [clearSession])

  const value = useMemo(
    () => ({ user, status, error, refreshSession, loginAdmin, loginVip, logout }),
    [user, status, error, refreshSession, loginAdmin, loginVip, logout],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
