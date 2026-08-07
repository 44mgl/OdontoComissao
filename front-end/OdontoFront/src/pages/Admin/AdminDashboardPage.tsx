import { useApiList } from '../../hooks/useApiList'
import { getDashboardData } from '../../api/admin'
import styles from './AdminPages.module.css'

const getDashboardList = async (signal?: AbortSignal) => [await getDashboardData(signal)]

export function AdminDashboardPage() {
  const dashboard = useApiList(getDashboardList)
  const data = dashboard.data[0]
  return <section className={styles.page}><header className={styles.heading}><div><span>8.1 · Visão geral</span><h1>Dashboard</h1><p>Acompanhe os principais volumes cadastrados no sistema.</p></div></header>{dashboard.loading && <p>Carregando indicadores...</p>}{dashboard.error && <p className={styles.error}>{dashboard.error}</p>}{data && <div className={styles.metrics}>{Object.entries(data).map(([label, value], index) => <article key={label}><span>{String(index + 1).padStart(2, '0')}</span><strong>{value}</strong><p>{label}</p></article>)}</div>}</section>
}
