import { useState } from 'react'
import { NavLink } from 'react-router-dom'
import brasaoOdontologia from '../../assets/brasao_odontologia.jpg'
import styles from './Header.module.css'

const navigation = [
  { label: 'Início', to: '/' },
  { label: 'Cronograma', to: '/eventos' },
  { label: 'Comissão', to: '/comissao' },
  { label: 'Shop', to: '/shop' },
]

export function Header() {
  const [menuOpen, setMenuOpen] = useState(false)
  const closeMenu = () => setMenuOpen(false)

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
            <li className={styles.mobileVip}>
              <NavLink to="/vip/login" onClick={closeMenu}>Área VIP</NavLink>
            </li>
          </ul>
        </nav>

        <NavLink className={styles.vip} to="/vip/login">Área VIP</NavLink>
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
    </header>
  )
}
