import { Link } from 'react-router-dom'
import { getComissao, getDestaques, getProximosEventos } from '../../api/home'
import brasaoOdontologia from '../../assets/brasao_odontologia.jpg'
import { ContentState } from '../../components/ContentState/ContentState'
import { RemoteImage } from '../../components/RemoteImage/RemoteImage'
import { useApiList } from '../../hooks/useApiList'
import { formatDate, formatTime } from '../../utils/date'
import styles from './HomePage.module.css'

export function HomePage() {
  const destaques = useApiList(getDestaques)
  const eventos = useApiList(getProximosEventos)
  const comissao = useApiList(getComissao)

  return (
    <main>
      <section className={styles.hero}>
        <div className={styles.heroContent}>
          <p className={styles.kicker}>
            Odontologia • TURMA VII • ANO 2025-2029
          </p>
          <h1>
            Um sorriso marca.
            <br />
            <em>Uma história permanece.</em>
          </h1>
          <p className={styles.intro}>
            Acompanhe os encontros, novidades e projetos que fazem parte da nossa
            jornada.
          </p>

          <div className={styles.actions}>
            <Link className={styles.primaryAction} to="/eventos">
              Ver cronograma
            </Link>
            <Link className={styles.secondaryAction} to="/shop">
              Conhecer o shop <span aria-hidden="true">→</span>
            </Link>
          </div>
        </div>

        <div className={styles.heroArt} aria-hidden="true">
          <div className={styles.heroCrest}>
            <img src={brasaoOdontologia} alt="" />
          </div>
          <span className={styles.orbitOne} />
          <span className={styles.orbitTwo} />
        </div>
      </section>

      <section className={styles.section} aria-labelledby="destaques-title">
        <div className={styles.sectionHeading}>
          <div>
            <span className={styles.sectionNumber}>01</span>
            <p>Em evidência</p>
          </div>
          <h2 id="destaques-title">Destaques</h2>
          <p>Os momentos e avisos mais importantes da nossa turma.</p>
        </div>

        <div className={styles.stories}>
          {destaques.loading && <ContentState kind="loading" />}
          {destaques.error && (
            <ContentState kind="error" message={destaques.error} />
          )}
          {!destaques.loading &&
            !destaques.error &&
            destaques.data.length === 0 && <ContentState kind="empty" />}

          {destaques.data.slice(0, 5).map((item, index) => (
            <article className={styles.story} key={item.id}>
              <div className={styles.storyImage}>
                {item.imagemUrl ? (
                  <img src={item.imagemUrl} alt="" />
                ) : (
                  <span>{String(index + 1).padStart(2, '0')}</span>
                )}
              </div>
              <p>{item.titulo}</p>
            </article>
          ))}
        </div>
      </section>

      <section
        className={`${styles.section} ${styles.eventsSection}`}
        aria-labelledby="eventos-title"
      >
        <div className={styles.sectionHeading}>
          <div>
            <span className={styles.sectionNumber}>02</span>
            <p>Agenda</p>
          </div>
          <h2 id="eventos-title">Próximos encontros</h2>
          <Link to="/eventos">
            Cronograma completo <span aria-hidden="true">→</span>
          </Link>
        </div>

        <div className={styles.eventGrid}>
          {eventos.loading && <ContentState kind="loading" />}
          {eventos.error && <ContentState kind="error" message={eventos.error} />}
          {!eventos.loading &&
            !eventos.error &&
            eventos.data.length === 0 && <ContentState kind="empty" />}

          {eventos.data.slice(0, 3).map((evento) => (
            <article className={styles.eventCard} key={evento.id}>
              <div className={styles.dateBox}>
                <strong>{formatDate(evento.dataHora, 'day')}</strong>
                <span>{formatDate(evento.dataHora, 'month')}</span>
              </div>
              <div>
                <span className={styles.category}>{evento.categoria}</span>
                <h3>{evento.titulo}</h3>
                <p>
                  {formatTime(evento.dataHora)} · {evento.local}
                </p>
              </div>
            </article>
          ))}
        </div>
      </section>

      <section className={styles.section} aria-labelledby="comissao-title">
        <div className={styles.sectionHeading}>
          <div>
            <span className={styles.sectionNumber}>03</span>
            <p>Quem faz acontecer</p>
          </div>
          <h2 id="comissao-title">Nossa comissão</h2>
          <Link to="/comissao">
            Conheça a equipe <span aria-hidden="true">→</span>
          </Link>
        </div>

        <div className={styles.peopleGrid}>
          {comissao.loading && <ContentState kind="loading" />}
          {comissao.error && (
            <ContentState kind="error" message={comissao.error} />
          )}
          {!comissao.loading &&
            !comissao.error &&
            comissao.data.length === 0 && <ContentState kind="empty" />}

          {comissao.data.slice(0, 4).map((membro) => (
            <article className={styles.person} key={membro.id}>
              <div className={styles.personImage}>
                <RemoteImage
                  src={membro.fotoUrl}
                  alt={`Foto de ${membro.nome}`}
                  fallback={
                    <span aria-hidden="true">{membro.nome.charAt(0)}</span>
                  }
                />
              </div>
              <p>{membro.cargo}</p>
              <h3>{membro.nome}</h3>
            </article>
          ))}
        </div>
      </section>
    </main>
  )
}
