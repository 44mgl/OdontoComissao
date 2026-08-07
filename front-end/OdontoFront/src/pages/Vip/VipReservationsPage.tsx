import { getVipReservations } from '../../api/vip'
import { ContentState } from '../../components/ContentState/ContentState'
import { useApiList } from '../../hooks/useApiList'
import { formatCurrency } from '../../utils/currency'
import { formatDateTime } from '../../utils/date'
import { reservationStatus } from '../../utils/reservationStatus'
import styles from './VipPages.module.css'

export function VipReservationsPage() {
  const reservations = useApiList(getVipReservations)

  return (
    <section className={styles.page} aria-labelledby="vip-reservations-title">
      <div className={styles.heading}>
        <span>7.5 · Histórico</span>
        <h2 id="vip-reservations-title">Minhas reservas</h2>
        <p>Somente reservas vinculadas à identidade do seu token são exibidas.</p>
      </div>

      {reservations.loading && <ContentState kind="loading" message="Carregando reservas…" />}
      {reservations.error && <ContentState kind="error" message={reservations.error} />}
      {!reservations.loading && !reservations.error && reservations.data.length === 0 && (
        <ContentState kind="empty" message="Você ainda não possui reservas VIP." />
      )}

      <div className={styles.reservations}>
        {reservations.data.map((reservation) => (
          <article key={reservation.codigoReserva}>
            <header>
              <div>
                <span>{formatDateTime(reservation.dataReserva)}</span>
                <h3>{reservation.codigoReserva}</h3>
              </div>
              <strong>{reservationStatus[reservation.status]}</strong>
            </header>
            <div className={styles.reservationSummary}>
              <span>{reservation.itens.reduce((total, item) => total + item.quantidade, 0)} item(ns)</span>
              <strong>{formatCurrency(reservation.valorTotal)}</strong>
            </div>
            <details>
              <summary>Ver detalhes</summary>
              <ul>
                {reservation.itens.map((item, index) => (
                  <li key={`${reservation.codigoReserva}-${index}`}>
                    <div>
                      <strong>{item.produto}</strong>
                      <span>Tamanho {item.tamanho} · {item.quantidade} unidade(s)</span>
                    </div>
                    <strong>{formatCurrency(item.subtotal)}</strong>
                  </li>
                ))}
              </ul>
            </details>
          </article>
        ))}
      </div>
    </section>
  )
}
