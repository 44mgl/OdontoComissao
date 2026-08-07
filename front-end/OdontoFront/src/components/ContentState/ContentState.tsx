import styles from './ContentState.module.css'

type Props = {
  kind: 'loading' | 'empty' | 'error'
  message?: string
}

export function ContentState({ kind, message }: Props) {
  const labels = {
    loading: message ?? 'Carregando conteúdo…',
    empty: message ?? 'Novidades serão publicadas em breve.',
    error: message ?? 'Não foi possível carregar. Tente novamente mais tarde.',
  }

  return <p className={`${styles.state} ${styles[kind]}`} role={kind === 'error' ? 'alert' : undefined}>{labels[kind]}</p>
}
