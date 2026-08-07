import { getVipProfileList } from '../../api/vip'
import { ContentState } from '../../components/ContentState/ContentState'
import { useApiList } from '../../hooks/useApiList'
import { formatFullDate } from '../../utils/date'
import styles from './VipPages.module.css'

export function VipProfilePage() {
  const profile = useApiList(getVipProfileList)
  const data = profile.data[0]

  return (
    <section className={styles.page} aria-labelledby="vip-profile-title">
      <div className={styles.heading}>
        <span>7.4 · Sua conta</span>
        <h2 id="vip-profile-title">Meu perfil</h2>
        <p>Dados fornecidos pela comissão e vinculados ao seu acesso.</p>
      </div>

      {profile.loading && <ContentState kind="loading" message="Carregando perfil…" />}
      {profile.error && <ContentState kind="error" message={profile.error} />}

      {data && (
        <dl className={styles.profileCard}>
          <div><dt>Nome</dt><dd>{data.nome}</dd></div>
          <div><dt>Identificação</dt><dd>{data.numeroIdentificacao}</dd></div>
          <div><dt>E-mail</dt><dd>{data.email}</dd></div>
          <div><dt>Telefone</dt><dd>{data.telefone}</dd></div>
          <div><dt>Cadastro</dt><dd>{formatFullDate(data.dataCadastro)}</dd></div>
          <div><dt>Validade</dt><dd>{data.dataValidade ? formatFullDate(data.dataValidade) : 'Não informada'}</dd></div>
          <div><dt>Situação</dt><dd>{data.ativo ? 'Acesso ativo' : 'Acesso inativo'}</dd></div>
        </dl>
      )}
    </section>
  )
}
