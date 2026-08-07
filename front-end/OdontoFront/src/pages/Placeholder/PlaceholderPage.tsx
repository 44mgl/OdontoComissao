import { Link, useLocation } from 'react-router-dom'
import styles from './PlaceholderPage.module.css'

const pageNames: Record<string, string> = {
  '/eventos': 'Cronograma',
  '/comissao': 'Comissão',
  '/shop': 'Shop',
  '/vip/login': 'Área VIP',
  '/admin/login': 'Administração',
  '/reserva': 'Solicitar reserva',
}

export function PlaceholderPage() {
  const { pathname } = useLocation()
  const title = pageNames[pathname] ?? 'Página não encontrada'

  return (
    <main className={styles.page}>
      <p>Próxima etapa</p>
      <h1>{title}</h1>
      <span>Esta rota já está preparada e será construída no próximo fluxo do planejamento.</span>
      <Link to="/">Voltar ao início</Link>
    </main>
  )
}
