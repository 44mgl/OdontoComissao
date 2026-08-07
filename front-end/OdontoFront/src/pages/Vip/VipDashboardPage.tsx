import { Link } from 'react-router-dom'
import { useAuth } from '../../hooks/useAuth'
import styles from './VipPages.module.css'

const actions = [
  { number: '01', title: 'Produtos exclusivos', text: 'Conheça o catálogo reservado aos membros VIP.', to: '/vip/produtos' },
  { number: '02', title: 'Nova reserva', text: 'Monte uma solicitação vinculada à sua conta.', to: '/vip/reserva' },
  { number: '03', title: 'Minhas reservas', text: 'Acompanhe status, itens e valores anteriores.', to: '/vip/reservas' },
  { number: '04', title: 'Meu perfil', text: 'Consulte seus dados e a validade do acesso.', to: '/vip/perfil' },
]

export function VipDashboardPage() {
  const { user } = useAuth()
  const visibleActions = user?.perfil === 'Administrador'
    ? actions.filter((action) => action.to === '/vip/produtos')
    : actions

  return (
    <section className={styles.page} aria-labelledby="vip-dashboard-title">
      <div className={styles.heading}>
        <span>7.1 · {user?.perfil === 'Administrador' ? 'Prévia administrativa' : 'Bem-vindo'}</span>
        <h2 id="vip-dashboard-title">
          {user?.perfil === 'Administrador' ? 'Experiência dos membros VIP' : 'Sua experiência VIP'}
        </h2>
        <p>
          {user?.perfil === 'Administrador'
            ? 'Você está visualizando a área VIP sem assumir a identidade de um membro.'
            : `${user?.nome}, escolha uma das áreas para continuar.`}
        </p>
      </div>

      <div className={styles.actionGrid}>
        {visibleActions.map((action) => (
          <Link to={action.to} key={action.to}>
            <span>{action.number}</span>
            <h3>{action.title}</h3>
            <p>{action.text}</p>
            <strong aria-hidden="true">→</strong>
          </Link>
        ))}
      </div>
    </section>
  )
}
