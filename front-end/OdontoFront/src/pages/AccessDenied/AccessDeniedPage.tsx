import { Link } from 'react-router-dom'
import { useAuth } from '../../hooks/useAuth'
import styles from './AccessDeniedPage.module.css'

export function AccessDeniedPage() {
  const { user } = useAuth()
  const homePath = user?.perfil === 'Administrador'
    ? '/admin'
    : user?.perfil === 'VIP'
      ? '/vip'
      : '/'

  return (
    <main className={styles.page}>
      <section aria-labelledby="access-denied-title">
        <span>403</span>
        <p>Acesso restrito</p>
        <h1 id="access-denied-title">Você não possui permissão para esta área.</h1>
        <small>
          Sua sessão continua ativa, mas o perfil atual não pode acessar este
          conteúdo.
        </small>
        <Link to={homePath}>Voltar para minha área</Link>
      </section>
    </main>
  )
}
