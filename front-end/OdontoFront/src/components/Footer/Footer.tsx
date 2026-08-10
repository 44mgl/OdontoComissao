import styles from './Footer.module.css'

export function Footer() {
  return (
    <footer className={styles.footer}>
      <div className={styles.inner}>
        <div>
          <span className={styles.eyebrow}>Odontologia • Turma 2025 - 2029</span>
        </div>
        <small>
          © 2026 Miguel Amores Ramos
          <a
            className={styles.social}
            href="https://github.com/44mgl"
            target="_blank"
            rel="noopener noreferrer"
            aria-label="GitHub de Miguel Amores Ramos"
            title="Abrir GitHub"
          >
            <svg aria-hidden="true" viewBox="0 0 19 19">
              <use href="/icons.svg#github-icon" />
            </svg>
          </a>
          <a
            className={styles.social}
            href="https://instagram.com/miguel_amrs"
            target="_blank"
            rel="noopener noreferrer"
            aria-label="Instagram de Miguel Amores Ramos"
            title="Abrir Instagram"
          >
            <svg aria-hidden="true" viewBox="0 0 24 24">
              <use href="/icons.svg#instagram-icon" />
            </svg>
          </a>
        </small>
      </div>
    </footer>
  )
}
