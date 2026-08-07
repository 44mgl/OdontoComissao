import { createContext } from 'react'
import type { SessionStatus, SessionUser } from '../types/auth'

export type AuthContextValue = {
  user: SessionUser | null
  status: SessionStatus
  error: string | null
  refreshSession: (signal?: AbortSignal) => Promise<SessionUser | null>
  loginAdmin: (email: string, password: string) => Promise<void>
  loginVip: (identificationNumber: string, password: string) => Promise<void>
  logout: () => Promise<void>
}

export const AuthContext = createContext<AuthContextValue | undefined>(undefined)
