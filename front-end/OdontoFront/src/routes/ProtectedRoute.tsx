import { Navigate, Outlet } from 'react-router-dom'
import { useAuth } from '../hooks/useAuth'
import type { UserRole } from '../types/auth'
import { getProtectedRouteDecision } from './protectedRouteDecision'
import styles from './ProtectedRoute.module.css'

type ProtectedRouteProps = {
  allowedRole: UserRole
  loginPath: '/admin/login' | '/vip/login'
}

export function ProtectedRoute({ allowedRole, loginPath }: ProtectedRouteProps) {
  const { user, status } = useAuth()
  const decision = getProtectedRouteDecision(status, user, allowedRole)

  if (decision === 'loading') {
    return (
      <main className={styles.page} aria-busy="true">
        <div role="status">
          <span className={styles.marker} aria-hidden="true" />
          <p>Verificando acesso seguro...</p>
        </div>
      </main>
    )
  }

  if (decision === 'login') {
    return <Navigate to={loginPath} replace />
  }

  if (decision === 'denied') {
    return <Navigate to="/acesso-negado" replace />
  }

  return <Outlet />
}
