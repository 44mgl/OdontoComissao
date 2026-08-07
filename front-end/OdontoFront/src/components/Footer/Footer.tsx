import styles from './Footer.module.css'

export function Footer() {
  return (
    <footer className={styles.footer}>
      <div className={styles.inner}>
        <div>
          <span className={styles.eyebrow}>Odontologia • Turma 2026</span>
        </div>
        <small>© 2026 Miguel Amores Ramos</small>
      </div>
    </footer>
  )
}
