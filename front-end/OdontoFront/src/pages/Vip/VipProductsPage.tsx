import { Link } from 'react-router-dom'
import { getVipCatalog } from '../../api/products'
import { ContentState } from '../../components/ContentState/ContentState'
import { RemoteImage } from '../../components/RemoteImage/RemoteImage'
import { useApiList } from '../../hooks/useApiList'
import { formatCurrency } from '../../utils/currency'
import styles from './VipPages.module.css'

export function VipProductsPage() {
  const catalog = useApiList(getVipCatalog)

  return (
    <section className={styles.page} aria-labelledby="vip-products-title">
      <div className={styles.heading}>
        <span>7.2 · Seleção exclusiva</span>
        <h2 id="vip-products-title">Produtos VIP</h2>
        <p>Este catálogo exige uma sessão VIP válida.</p>
      </div>

      {catalog.loading && <ContentState kind="loading" message="Carregando produtos VIP…" />}
      {catalog.error && <ContentState kind="error" message={catalog.error} />}
      {!catalog.loading && !catalog.error && catalog.data.length === 0 && (
        <ContentState kind="empty" message="Ainda não há produtos exclusivos disponíveis." />
      )}

      <div className={styles.productGrid}>
        {catalog.data.map((product) => {
          const stock = product.variacoes
            .filter((variation) => variation.ativo)
            .reduce((total, variation) => total + variation.quantidadeDisponivel, 0)

          return (
            <article className={styles.product} key={product.id}>
              <div className={styles.productImage}>
                <RemoteImage
                  src={product.imagemUrl}
                  alt={`Produto ${product.nome}`}
                  fallback={<span aria-hidden="true">{product.nome.charAt(0)}</span>}
                />
                <small>Exclusivo VIP</small>
              </div>
              <div className={styles.productBody}>
                <span>{product.categoria}</span>
                <h3>{product.nome}</h3>
                <p>{product.descricao}</p>
                <div>
                  <strong>{formatCurrency(product.preco)}</strong>
                  <small>{stock} unidade(s)</small>
                </div>
                {stock > 0 ? (
                  <Link to={`/vip/reserva?produto=${product.id}`}>Reservar produto</Link>
                ) : (
                  <span className={styles.unavailable}>Indisponível</span>
                )}
              </div>
            </article>
          )
        })}
      </div>
    </section>
  )
}
