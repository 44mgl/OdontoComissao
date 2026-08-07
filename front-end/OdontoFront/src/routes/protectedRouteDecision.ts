import type { SessionStatus, SessionUser, UserRole } from '../types/auth'

export type ProtectedRouteDecision = 'loading' | 'login' | 'denied' | 'allow'

export function getProtectedRouteDecision(
  status: SessionStatus,
  user: SessionUser | null,
  allowedRoles: UserRole[],
): ProtectedRouteDecision {
  if (status === 'loading') {
    return 'loading'
  }

  if (status === 'unauthenticated' || user === null) {
    return 'login'
  }

  return allowedRoles.includes(user.perfil) ? 'allow' : 'denied'
}
