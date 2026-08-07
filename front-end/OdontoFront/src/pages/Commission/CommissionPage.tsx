import { getComissao } from '../../api/home'
import { ContentState } from '../../components/ContentState/ContentState'
import { RemoteImage } from '../../components/RemoteImage/RemoteImage'
import { useApiList } from '../../hooks/useApiList'
import styles from './CommissionPage.module.css'

function getInitials(name: string) {
  return name.trim().split(/\s+/).slice(0, 2).map((part) => part.charAt(0)).join('').toUpperCase()
}

export function CommissionPage() {
  const membros = useApiList(getComissao)

  return (
    <main>
      <section className={styles.hero}>
        <div className={styles.heroInner}>
          <p>Quem faz acontecer</p>
          <h1>
            Nossa
            <br />
            <em>comissão</em>
          </h1>
          <span>
            Pessoas diferentes, reunidas pelo propósito de construir momentos
            inesquecíveis.
          </span>
        </div>
        <div className={styles.heroWord} aria-hidden="true">
          Equipe
        </div>
      </section>

      <section className={styles.content} aria-labelledby="commission-title">
        <div className={styles.heading}>
          <div>
            <span>Um trabalho coletivo</span>
            <h2 id="commission-title">Conheça a equipe</h2>
          </div>
          <p>
            Cada detalhe passa por quem escolheu representar e cuidar da nossa turma.
          </p>
        </div>

        <div className={styles.grid}>
          {membros.loading && (
            <ContentState kind="loading" message="Carregando a comissão…" />
          )}
          {membros.error && <ContentState kind="error" message={membros.error} />}
          {!membros.loading &&
            !membros.error &&
            membros.data.length === 0 && (
              <ContentState
                kind="empty"
                message="Os integrantes serão apresentados em breve."
              />
            )}

          {membros.data.map((membro, index) => (
            <article className={styles.member} key={membro.id}>
              <div className={styles.photo}>
                <RemoteImage
                  src={membro.fotoUrl}
                  alt={`Foto de ${membro.nome}`}
                  fallback={
                    <span className={styles.initials}>
                      {getInitials(membro.nome)}
                    </span>
                  }
                />
                <span className={styles.order} aria-hidden="true">
                  {String(index + 1).padStart(2, '0')}
                </span>
              </div>

              <div className={styles.memberContent}>
                <p>{membro.cargo}</p>
                <h3>{membro.nome}</h3>
                {membro.descricao && <span>{membro.descricao}</span>}
              </div>
            </article>
          ))}
        </div>
      </section>
    </main>
  )
}
