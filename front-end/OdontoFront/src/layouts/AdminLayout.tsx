import { NavLink, Outlet } from 'react-router-dom'
import styles from './AdminLayout.module.css'

const links = [
  { to: '/admin', label: 'Dashboard', end: true },
  { to: '/admin/publicacoes', label: 'Publicações' },
  { to: '/admin/eventos', label: 'Eventos' },
  { to: '/admin/comissao', label: 'Comissão' },
  { to: '/admin/produtos', label: 'Produtos' },
  { to: '/admin/reservas', label: 'Reservas' },
  { to: '/admin/vips', label: 'VIPs' },
  { to: '/admin/administradores', label: 'Administradores' },
]

export function AdminLayout() {
  return (
    <main className={styles.shell}>
      <aside className={styles.sidebar}>
        <div>
          <span>Odonto Comissão</span>
          <strong>Administração</strong>
        </div>
        <nav aria-label="Navegação administrativa">
          {links.map((link) => (
            <NavLink
              key={link.to}
              to={link.to}
              end={link.end}
              className={({ isActive }) => (isActive ? styles.active : undefined)}
            >
              {link.label}
            </NavLink>
          ))}
        </nav>
        <NavLink className={styles.vipPreview} to="/vip">Visualizar área VIP →</NavLink>
      </aside>
      <div className={styles.content}><Outlet /></div>
    </main>
  )
}
