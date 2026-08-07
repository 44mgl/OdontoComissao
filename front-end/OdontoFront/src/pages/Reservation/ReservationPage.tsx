import { useMemo, useState, type FormEvent } from 'react'
import { Link, useSearchParams } from 'react-router-dom'
import { getPublicCatalog, getVipCatalog } from '../../api/products'
import { createReservation } from '../../api/reservations'
import { ApiError } from '../../api/client'
import { ContentState } from '../../components/ContentState/ContentState'
import { useApiList } from '../../hooks/useApiList'
import { useAuth } from '../../hooks/useAuth'
import type { Produto, VariacaoProduto } from '../../types/content'
import type { ReservationResponse } from '../../types/reservation'
import { formatCurrency } from '../../utils/currency'
import { validateReservationForm } from '../../utils/reservation'
import styles from './ReservationPage.module.css'

type CartItem = {
  product: Produto
  variation: VariacaoProduto
  quantity: number
}

type ReservationPageProps = {
  mode?: 'public' | 'vip'
}

export function ReservationPage({ mode = 'public' }: ReservationPageProps) {
  const [searchParams] = useSearchParams()
  const { user } = useAuth()
  const isVip = mode === 'vip'
  const catalog = useApiList(isVip ? getVipCatalog : getPublicCatalog)
  const [productId, setProductId] = useState(searchParams.get('produto') ?? '')
  const [variationId, setVariationId] = useState('')
  const [quantity, setQuantity] = useState(1)
  const [items, setItems] = useState<CartItem[]>([])
  const [nomeCliente, setNomeCliente] = useState(isVip ? user?.nome ?? '' : '')
  const [contato, setContato] = useState('')
  const [observacoes, setObservacoes] = useState('')
  const [errors, setErrors] = useState<Record<string, string>>({})
  const [submitting, setSubmitting] = useState(false)
  const [submitError, setSubmitError] = useState<string | null>(null)
  const [created, setCreated] = useState<ReservationResponse | null>(null)

  const selectedProduct = useMemo(
    () => catalog.data.find((product) => product.id === Number(productId)),
    [catalog.data, productId],
  )
  const selectedVariation = selectedProduct?.variacoes.find(
    (variation) => variation.id === Number(variationId) && variation.ativo,
  )
  const estimatedTotal = items.reduce(
    (total, item) => total + item.product.preco * item.quantity,
    0,
  )

  function changeProduct(value: string) {
    setProductId(value)
    setVariationId('')
    setQuantity(1)
    setErrors((current) => ({ ...current, selection: '' }))
  }

  function addItem() {
    if (!selectedProduct || !selectedVariation) {
      setErrors((current) => ({ ...current, selection: 'Escolha um produto e um tamanho.' }))
      return
    }
    if (quantity < 1 || quantity > selectedVariation.quantidadeDisponivel) {
      setErrors((current) => ({ ...current, selection: 'A quantidade deve respeitar o estoque disponível.' }))
      return
    }
    if (items.some((item) => item.variation.id === selectedVariation.id)) {
      setErrors((current) => ({ ...current, selection: 'Essa variação já está na reserva.' }))
      return
    }

    setItems((current) => [...current, { product: selectedProduct, variation: selectedVariation, quantity }])
    setVariationId('')
    setQuantity(1)
    setErrors((current) => ({ ...current, selection: '', itemCount: '' }))
  }

  function removeItem(variationIdToRemove: number) {
    setItems((current) => current.filter((item) => item.variation.id !== variationIdToRemove))
  }

  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const validationErrors = validateReservationForm({ nomeCliente, contato, itemCount: items.length })
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors as Record<string, string>)
      return
    }

    setSubmitting(true)
    setSubmitError(null)
    try {
      const response = await createReservation({
        nomeCliente: nomeCliente.trim(),
        contato: contato.trim(),
        observacoes: observacoes.trim() || null,
        itens: items.map((item) => ({
          variacaoProdutoId: item.variation.id,
          quantidade: item.quantity,
        })),
      })
      setCreated(response)
    } catch (error) {
      setSubmitError(error instanceof ApiError ? error.message : 'Não foi possível criar a reserva.')
    } finally {
      setSubmitting(false)
    }
  }

  if (created) {
    return (
      <main className={styles.successPage}>
        <div className={styles.successCard}>
          <span className={styles.successMark} aria-hidden="true">
            ✓
          </span>
          <p>{isVip ? 'Reserva VIP solicitada' : 'Reserva solicitada'}</p>
          <h1>Guarde seu código</h1>
          <strong>{created.codigoReserva}</strong>
          <span>
            Você precisará desse código para consultar o andamento da reserva.
          </span>
          <div>
            <Link to={`/reserva/${encodeURIComponent(created.codigoReserva)}`}>
              Consultar reserva
            </Link>
            <Link to={isVip ? '/vip' : '/'}>
              {isVip ? 'Voltar à área VIP' : 'Voltar ao início'}
            </Link>
            <Link to={isVip ? '/vip/produtos' : '/shop'}>
              Continuar no Shop
            </Link>
          </div>
        </div>
      </main>
    )
  }

  return (
    <main>
      <section className={styles.hero}>
        <div>
          <p>{isVip ? '7.3 · Reserva autenticada' : 'Sem pagamento on-line'}</p>
          <h1>
            Solicitar
            <br />
            <em>reserva</em>
          </h1>
          <span>
            {isVip
              ? 'Sua reserva será vinculada automaticamente à identidade da sessão VIP.'
              : 'Monte seu pedido e confirme seus dados. A disponibilidade final será validada pela comissão.'}
          </span>
        </div>
      </section>

      <form className={styles.form} onSubmit={submit} noValidate>
        <section className={styles.builder} aria-labelledby="products-title">
          <div className={styles.sectionTitle}>
            <span>01</span>
            <div>
              <p>Escolha dos itens</p>
              <h2 id="products-title">Produtos</h2>
            </div>
          </div>

          {catalog.loading && (
            <ContentState kind="loading" message="Carregando produtos…" />
          )}
          {catalog.error && <ContentState kind="error" message={catalog.error} />}

          {!catalog.loading && !catalog.error && (
            <div className={styles.selector}>
              <label>
                Produto
                <select
                  value={productId}
                  onChange={(event) => changeProduct(event.target.value)}
                >
                  <option value="">Selecione</option>
                  {catalog.data.map((product) => (
                    <option value={product.id} key={product.id}>
                      {product.nome} — {formatCurrency(product.preco)}
                    </option>
                  ))}
                </select>
              </label>

              <label>
                Tamanho
                <select
                  value={variationId}
                  disabled={!selectedProduct}
                  onChange={(event) => setVariationId(event.target.value)}
                >
                  <option value="">Selecione</option>
                  {selectedProduct?.variacoes
                    .filter((item) => item.ativo && item.quantidadeDisponivel > 0)
                    .map((item) => (
                      <option value={item.id} key={item.id}>
                        {item.tamanho} — {item.quantidadeDisponivel} disponíveis
                      </option>
                    ))}
                </select>
              </label>

              <label>
                Quantidade
                <input
                  type="number"
                  min="1"
                  max={selectedVariation?.quantidadeDisponivel ?? 1}
                  value={quantity}
                  onChange={(event) => setQuantity(Number(event.target.value))}
                />
              </label>

              <button type="button" onClick={addItem}>
                Adicionar item
              </button>

              {errors.selection && (
                <p className={styles.fieldError} role="alert">
                  {errors.selection}
                </p>
              )}
            </div>
          )}

          <div className={styles.cart}>
            {items.length === 0 ? (
              <p className={styles.emptyCart}>Nenhum produto adicionado.</p>
            ) : (
              items.map((item) => (
                <article key={item.variation.id}>
                  <div>
                    <span>{item.product.categoria}</span>
                    <h3>{item.product.nome}</h3>
                    <p>
                      Tamanho {item.variation.tamanho} · {item.quantity} unidade(s)
                    </p>
                  </div>
                  <strong>
                    {formatCurrency(item.product.preco * item.quantity)}
                  </strong>
                  <button
                    type="button"
                    onClick={() => removeItem(item.variation.id)}
                    aria-label={`Remover ${item.product.nome}, tamanho ${item.variation.tamanho}`}
                  >
                    Remover
                  </button>
                </article>
              ))
            )}

            {errors.itemCount && (
              <p className={styles.fieldError} role="alert">
                {errors.itemCount}
              </p>
            )}
          </div>
        </section>

        <section className={styles.customer} aria-labelledby="customer-title">
          <div className={styles.sectionTitle}>
            <span>02</span>
            <div>
              <p>Seus dados</p>
              <h2 id="customer-title">Contato</h2>
            </div>
          </div>

          <div className={styles.fields}>
            <label>
              Nome completo
              <input
                value={nomeCliente}
                onChange={(event) => setNomeCliente(event.target.value)}
                aria-invalid={Boolean(errors.nomeCliente)}
              />
              {errors.nomeCliente && <span role="alert">{errors.nomeCliente}</span>}
            </label>

            <label>
              WhatsApp ou telefone
              <input
                value={contato}
                onChange={(event) => setContato(event.target.value)}
                aria-invalid={Boolean(errors.contato)}
              />
              {errors.contato && <span role="alert">{errors.contato}</span>}
            </label>

            <label className={styles.fullField}>
              Observações <small>(opcional)</small>
              <textarea
                rows={4}
                value={observacoes}
                onChange={(event) => setObservacoes(event.target.value)}
              />
            </label>
          </div>
        </section>

        <aside className={styles.summary}>
          <div>
            <span>Resumo</span>
            <strong>
              {items.reduce((total, item) => total + item.quantity, 0)} item(ns)
            </strong>
          </div>
          <div>
            <span>Valor estimado</span>
            <strong>{formatCurrency(estimatedTotal)}</strong>
          </div>
          <p>O preço e o estoque serão confirmados pelo sistema ao enviar.</p>
          {submitError && (
            <p className={styles.submitError} role="alert">
              {submitError}
            </p>
          )}
          <button type="submit" disabled={submitting}>
            {submitting ? 'Enviando…' : 'Confirmar reserva'}
          </button>
        </aside>
      </form>
    </main>
  )
}
