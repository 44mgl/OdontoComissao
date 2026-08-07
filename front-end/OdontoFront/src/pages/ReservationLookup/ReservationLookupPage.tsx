import { useEffect, useState, type FormEvent } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { ApiError } from '../../api/client'
import { getReservationByCode } from '../../api/reservations'
import { ContentState } from '../../components/ContentState/ContentState'
import type { PublicReservation } from '../../types/reservation'
import { formatCurrency } from '../../utils/currency'
import { formatDateTime } from '../../utils/date'
import { reservationStatus } from '../../utils/reservationStatus'
import styles from './ReservationLookupPage.module.css'

export function ReservationLookupPage() {
  const { codigo } = useParams()
  const navigate = useNavigate()
  const [search, setSearch] = useState(codigo ?? '')
  const [reservation, setReservation] = useState<PublicReservation | null>(null)
  const [loading, setLoading] = useState(Boolean(codigo))
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    if (!codigo) {
      setReservation(null)
      setLoading(false)
      setError(null)
      return
    }

    const controller = new AbortController()
    setLoading(true)
    setError(null)

    getReservationByCode(codigo, controller.signal)
      .then(setReservation)
      .catch((requestError: unknown) => {
        if (requestError instanceof DOMException && requestError.name === 'AbortError') {
          return
        }

        setReservation(null)
        setError(
          requestError instanceof ApiError && requestError.status === 404
            ? 'Reserva não encontrada. Confira o código informado.'
            : requestError instanceof Error
              ? requestError.message
              : 'Não foi possível consultar a reserva.',
        )
      })
      .finally(() => {
        if (!controller.signal.aborted) setLoading(false)
      })

    return () => controller.abort()
  }, [codigo])

  function submitSearch(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const normalizedCode = search.trim().toUpperCase()

    if (!normalizedCode) {
      setError('Informe o código da reserva.')
      return
    }

    navigate(`/reserva/${encodeURIComponent(normalizedCode)}`)
  }

  return (
    <main>
      <section className={styles.hero}>
        <div>
          <p>Acompanhe seu pedido</p>
          <h1>
            Consultar
            <br />
            <em>reserva</em>
          </h1>
          <span>
            Digite o código recebido na confirmação para verificar o andamento.
          </span>
        </div>
      </section>

      <section className={styles.content} aria-labelledby="lookup-title">
        <form className={styles.search} onSubmit={submitSearch}>
          <div>
            <span>Consulta pública</span>
            <h2 id="lookup-title">Código da reserva</h2>
          </div>
          <label>
            <span className={styles.srOnly}>Código da reserva</span>
            <input
              value={search}
              onChange={(event) => setSearch(event.target.value)}
              placeholder="Ex.: RES-2026-ABC123"
              autoComplete="off"
            />
          </label>
          <button type="submit">Consultar</button>
        </form>

        {loading && <ContentState kind="loading" message="Consultando reserva…" />}
        {error && <ContentState kind="error" message={error} />}

        {!codigo && !loading && !error && (
          <ContentState
            kind="empty"
            message="Informe seu código acima para iniciar a consulta."
          />
        )}

        {reservation && !loading && (
          <article className={styles.result}>
            <header>
              <div>
                <span>Reserva</span>
                <h2>{reservation.codigoReserva}</h2>
              </div>
              <strong className={`${styles.status} ${styles[`status${reservation.status}`]}`}>
                {reservationStatus[reservation.status]}
              </strong>
            </header>

            <div className={styles.meta}>
              <div>
                <span>Solicitada em</span>
                <strong>{formatDateTime(reservation.dataReserva)}</strong>
              </div>
              <div>
                <span>Última atualização</span>
                <strong>{formatDateTime(reservation.dataAtualizacao)}</strong>
              </div>
              <div>
                <span>Valor total</span>
                <strong>{formatCurrency(reservation.valorTotal)}</strong>
              </div>
            </div>

            <div className={styles.items}>
              <h3>Itens reservados</h3>
              {reservation.itens.map((item, index) => (
                <div className={styles.item} key={`${item.produto}-${item.tamanho}-${index}`}>
                  <span>{String(index + 1).padStart(2, '0')}</span>
                  <div>
                    <strong>{item.produto}</strong>
                    <p>
                      Tamanho {item.tamanho} · {item.quantidade} unidade(s) ·{' '}
                      {formatCurrency(item.precoUnitario)} cada
                    </p>
                  </div>
                  <strong>{formatCurrency(item.subtotal)}</strong>
                </div>
              ))}
            </div>

            <footer>
              Guarde este código até a retirada. Nenhum pagamento é realizado pelo site.
            </footer>
          </article>
        )}
      </section>
    </main>
  )
}
