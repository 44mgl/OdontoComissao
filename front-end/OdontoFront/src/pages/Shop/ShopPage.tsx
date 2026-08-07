import { useMemo, useState } from 'react'
import { Link } from 'react-router-dom'
import { getPublicCatalog } from '../../api/products'
import { ContentState } from '../../components/ContentState/ContentState'
import { RemoteImage } from '../../components/RemoteImage/RemoteImage'
import { useApiList } from '../../hooks/useApiList'
import { formatCurrency } from '../../utils/currency'
import styles from './ShopPage.module.css'

const ALL_CATEGORIES = 'Todos'

export function ShopPage() {
  const catalog = useApiList(getPublicCatalog)
  const [category, setCategory] = useState(ALL_CATEGORIES)
  const categories = useMemo(
    () => [
      ALL_CATEGORIES,
      ...new Set(catalog.data.map((product) => product.categoria)),
    ],
    [catalog.data],
  )
  const products = useMemo(
    () =>
      category === ALL_CATEGORIES
        ? catalog.data
        : catalog.data.filter((product) => product.categoria === category),
    [catalog.data, category],
  )

  return (
    <main>
      <section className={styles.hero}>
        <div className={styles.heroInner}>
          <p>Feito para a nossa história</p>
          <h1>
            Odonto
            <br />
            <em>shop</em>
          </h1>
          <span>
            Produtos da turma disponíveis para reserva. Sem pagamento on-line.
          </span>
        </div>
        <div className={styles.heroWord} aria-hidden="true">
          Shop
        </div>
      </section>

      <section className={styles.catalog} aria-labelledby="catalog-title">
        <div className={styles.heading}>
          <div>
            <span>Catálogo público</span>
            <h2 id="catalog-title">Escolha o seu</h2>
          </div>
          <p>Selecione um produto e continue para solicitar sua reserva.</p>
        </div>

        {!catalog.loading && !catalog.error && catalog.data.length > 0 && (
          <div className={styles.filters} aria-label="Filtrar por categoria">
            {categories.map((item) => (
              <button
                type="button"
                key={item}
                aria-pressed={category === item}
                onClick={() => setCategory(item)}
              >
                {item}
              </button>
            ))}
          </div>
        )}

        <div className={styles.grid}>
          {catalog.loading && (
            <ContentState kind="loading" message="Carregando o catálogo…" />
          )}

          {catalog.error && (
            <ContentState kind="error" message={catalog.error} />
          )}

          {!catalog.loading && !catalog.error && catalog.data.length === 0 && (
            <ContentState
              kind="empty"
              message="Ainda não há produtos disponíveis."
            />
          )}

          {!catalog.loading &&
            !catalog.error &&
            catalog.data.length > 0 &&
            products.length === 0 && (
              <ContentState
                kind="empty"
                message="Nenhum produto nesta categoria."
              />
            )}

          {products.map((product) => {
            const variations = product.variacoes.filter(
              (variation) => variation.ativo,
            )
            const totalStock = variations.reduce(
              (total, variation) => total + variation.quantidadeDisponivel,
              0,
            )

            return (
              <article className={styles.product} key={product.id}>
                <div className={styles.productImage}>
                  <RemoteImage
                    src={product.imagemUrl}
                    alt={`Produto ${product.nome}`}
                    fallback={(
                      <span
                        className={styles.productFallback}
                        aria-hidden="true"
                      >
                        {product.nome.charAt(0)}
                      </span>
                    )}
                  />
                  <span className={styles.category}>{product.categoria}</span>
                  {totalStock === 0 && (
                    <span className={styles.soldOut}>Esgotado</span>
                  )}
                </div>

                <div className={styles.productContent}>
                  <div className={styles.productTitle}>
                    <h3>{product.nome}</h3>
                    <strong>{formatCurrency(product.preco)}</strong>
                  </div>
                  <p>{product.descricao}</p>

                  <div className={styles.variations}>
                    <span>Tamanhos e estoque</span>
                    {variations.length > 0 ? (
                      <ul>
                        {variations.map((variation) => (
                          <li
                            key={variation.id}
                            data-empty={variation.quantidadeDisponivel === 0}
                          >
                            <strong>{variation.tamanho}</strong>
                            <span>{variation.quantidadeDisponivel} un.</span>
                          </li>
                        ))}
                      </ul>
                    ) : (
                      <p>Sem variações disponíveis.</p>
                    )}
                  </div>

                  {totalStock > 0 ? (
                    <Link
                      className={styles.reserve}
                      to={`/reserva?produto=${product.id}`}
                    >
                      Solicitar reserva <span aria-hidden="true">→</span>
                    </Link>
                  ) : (
                    <span className={styles.disabled}>Produto indisponível</span>
                  )}
                </div>
              </article>
            )
          })}
        </div>
      </section>
    </main>
  )
}
