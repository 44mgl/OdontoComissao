import { useState } from 'react'
import { getProximosEventos } from '../../api/home'
import { ContentState } from '../../components/ContentState/ContentState'
import { RemoteImage } from '../../components/RemoteImage/RemoteImage'
import { useApiList } from '../../hooks/useApiList'
import { formatDate, formatTime } from '../../utils/date'
import { eventStatus, formatLongDate } from '../../utils/event'
import styles from './EventsPage.module.css'

type ScheduleView = 'upcoming' | 'past'

export function EventsPage() {
  const [view, setView] = useState<ScheduleView>('upcoming')
  const eventos = useApiList(getProximosEventos)

  return (
    <main>
      <section className={styles.hero}>
        <div className={styles.heroInner}>
          <p>Agenda da turma</p>
          <h1>Nosso<br /><em>cronograma</em></h1>
          <span>Datas importantes, encontros e celebrações reunidos em um só lugar.</span>
        </div>
        <div className={styles.heroMark} aria-hidden="true">Agenda</div>
      </section>

      <section className={styles.schedule} aria-labelledby="schedule-title">
        <div className={styles.heading}>
          <div>
            <span>Próximos passos</span>
            <h2 id="schedule-title">Eventos</h2>
          </div>
          <div className={styles.tabs} role="tablist" aria-label="Período dos eventos">
            <button
              type="button"
              role="tab"
              aria-selected={view === 'upcoming'}
              onClick={() => setView('upcoming')}
            >
              Próximos
            </button>
            <button
              type="button"
              role="tab"
              aria-selected={view === 'past'}
              onClick={() => setView('past')}
            >
              Anteriores
            </button>
          </div>
        </div>

        {view === 'past' ? (
          <ContentState
            kind="empty"
            message="O histórico de eventos será disponibilizado em breve."
          />
        ) : (
          <div className={styles.eventList}>
            {eventos.loading && (
              <ContentState kind="loading" message="Carregando o cronograma…" />
            )}
            {eventos.error && <ContentState kind="error" message={eventos.error} />}
            {!eventos.loading && !eventos.error && eventos.data.length === 0 && (
              <ContentState kind="empty" message="Ainda não há próximos eventos publicados." />
            )}

            {eventos.data.map((evento, index) => (
              <article className={styles.event} key={evento.id}>
                <div className={styles.imageFrame}>
                  <RemoteImage
                    src={evento.imagemUrl}
                    alt={`Imagem do evento ${evento.titulo}`}
                    fallback={<span className={styles.imageFallback}>{String(index + 1).padStart(2, '0')}</span>}
                  />
                  <span className={`${styles.status} ${styles[`status${evento.status}`]}`}>
                    {eventStatus[evento.status]}
                  </span>
                </div>

                <div className={styles.eventContent}>
                  <div className={styles.dateBadge} aria-hidden="true">
                    <strong>{formatDate(evento.dataHora, 'day')}</strong>
                    <span>{formatDate(evento.dataHora, 'month')}</span>
                  </div>
                  <div className={styles.eventDetails}>
                    <span className={styles.category}>{evento.categoria}</span>
                    <h3>{evento.titulo}</h3>
                    <p>{evento.descricao}</p>
                    <dl>
                      <div>
                        <dt>Data</dt>
                        <dd>{formatLongDate(evento.dataHora)}</dd>
                      </div>
                      <div>
                        <dt>Horário</dt>
                        <dd>{formatTime(evento.dataHora)}</dd>
                      </div>
                      <div>
                        <dt>Local</dt>
                        <dd>{evento.local}</dd>
                      </div>
                    </dl>
                  </div>
                </div>
              </article>
            ))}
          </div>
        )}
      </section>
    </main>
  )
}
