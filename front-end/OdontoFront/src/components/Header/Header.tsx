import { useState } from 'react'
import { NavLink, useNavigate } from 'react-router-dom'
import brasaoOdontologia from '../../assets/brasao_odontologia.jpg'
import { useAuth } from '../../hooks/useAuth'
import styles from './Header.module.css'

const navigation = [
  { label: 'Início', to: '/' },
  { label: 'Cronograma', to: '/eventos' },
  { label: 'Comissão', to: '/comissao' },
  { label: 'Shop', to: '/shop' },
  { label: 'Minha reserva', to: '/consultar-reserva' },
]

export function Header() {
  const [menuOpen, setMenuOpen] = useState(false)
  const [logoutError, setLogoutError] = useState<string | null>(null)
  const { user, status, logout } = useAuth()
  const navigate = useNavigate()
  const closeMenu = () => setMenuOpen(false)
  const accountPath = user?.perfil === 'Administrador' ? '/admin' : '/vip'
  const accountLabel = user?.perfil === 'Administrador' ? 'Painel' : 'Área VIP'

  async function handleLogout() {
    setLogoutError(null)

    try {
      await logout()
      closeMenu()
      navigate('/')
    } catch (requestError) {
      setLogoutError(
        requestError instanceof Error
          ? requestError.message
          : 'Não foi possível encerrar a sessão.',
      )
    }
  }

  return (
    <header className={styles.header}>
      <div className={styles.inner}>
        <NavLink className={styles.brand} to="/" aria-label="Odonto Comissão — início">
          <img
            className={styles.crest}
            src={brasaoOdontologia}
            alt=""
            aria-hidden="true"
          />
          <span>
            <strong>Odonto</strong>
            <small>Comissão</small>
          </span>
        </NavLink>

        <nav
          className={`${styles.nav} ${menuOpen ? styles.navOpen : ''}`}
          aria-label="Navegação principal"
        >
          <ul className={styles.navigation}>
            {navigation.map((item) => (
              <li key={item.to}>
                <NavLink
                  to={item.to}
                  className={({ isActive }) => (isActive ? styles.active : undefined)}
                  onClick={closeMenu}
                >
                  {item.label}
                </NavLink>
              </li>
            ))}
            {status !== 'loading' && (
              <li className={styles.mobileVip}>
                <NavLink
                  to={user ? accountPath : '/vip/login'}
                  onClick={closeMenu}
                >
                  {user ? accountLabel : 'Área VIP'}
                </NavLink>
              </li>
            )}
            {user && (
              <li className={styles.mobileLogout}>
                <button type="button" onClick={handleLogout}>Sair</button>
              </li>
            )}
          </ul>
        </nav>

        <div className={styles.sessionActions}>
          {status !== 'loading' && (
            <NavLink className={styles.vip} to={user ? accountPath : '/vip/login'}>
              {user ? accountLabel : 'Área VIP'}
            </NavLink>
          )}
          {user && (
            <button className={styles.logout} type="button" onClick={handleLogout}>
              Sair
            </button>
          )}
        </div>
        <button
          className={styles.menuButton}
          type="button"
          aria-label={menuOpen ? 'Fechar menu' : 'Abrir menu'}
          aria-expanded={menuOpen}
          onClick={() => setMenuOpen((open) => !open)}
        >
          <span />
          <span />
          <span />
        </button>
      </div>
      {logoutError && (
        <p className={styles.logoutError} role="alert">{logoutError}</p>
      )}
    </header>
  )
}
