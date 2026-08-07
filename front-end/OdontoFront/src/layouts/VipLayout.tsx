import { NavLink, Outlet } from 'react-router-dom'
import { useAuth } from '../hooks/useAuth'
import styles from './VipLayout.module.css'

const vipNavigation = [
  { to: '/vip', label: 'Visão geral', end: true, vipOnly: false },
  { to: '/vip/produtos', label: 'Produtos', end: false, vipOnly: false },
  { to: '/vip/reserva', label: 'Nova reserva', end: false, vipOnly: true },
  { to: '/vip/reservas', label: 'Minhas reservas', end: false, vipOnly: true },
  { to: '/vip/perfil', label: 'Meu perfil', end: false, vipOnly: true },
]

export function VipLayout() {
  const { user } = useAuth()

  return (
    <main className={styles.area}>
      <header className={styles.hero}>
        <div>
          <span>{user?.perfil === 'Administrador' ? 'Visualização administrativa' : 'Área exclusiva'}</span>
          <h1>Olá, {user?.nome}</h1>
          <p>
            {user?.perfil === 'Administrador'
              ? 'Confira a experiência e o catálogo disponíveis aos membros VIP.'
              : 'Produtos, reservas e informações da sua participação VIP.'}
          </p>
        </div>
      </header>

      <nav className={styles.navigation} aria-label="Navegação da área VIP">
        <div>
          {vipNavigation
            .filter((item) => !item.vipOnly || user?.perfil === 'VIP')
            .map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              end={item.end}
              className={({ isActive }) => (isActive ? styles.active : undefined)}
            >
              {item.label}
            </NavLink>
            ))}
        </div>
      </nav>

      <Outlet />
    </main>
  )
}
