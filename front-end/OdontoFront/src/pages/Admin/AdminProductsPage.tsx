import { useCallback, useEffect, useState, type FormEvent } from 'react'
import { createVariation, deleteVariation, getAdminProducts, updateVariation, updateVariationStock } from '../../api/admin'
import type { Produto } from '../../types/content'
import { AdminResourcePage } from './AdminResourcePage'
import { productConfig } from './resourceConfig'
import styles from './AdminPages.module.css'

export function AdminProductsPage() {
  const [products, setProducts] = useState<Produto[]>([])
  const [productId, setProductId] = useState('')
  const [size, setSize] = useState('')
  const [stock, setStock] = useState(0)
  const [message, setMessage] = useState<string | null>(null)
  const [error, setError] = useState<string | null>(null)
  const load = useCallback(async () => setProducts(await getAdminProducts()), [])
  useEffect(() => { void load() }, [load])

  async function addVariation(event: FormEvent) {
    event.preventDefault(); try { await createVariation({ produtoId: Number(productId), tamanho: size, quantidadeDisponivel: stock }); setSize(''); setStock(0); setMessage('Variação criada com sucesso.'); setError(null); await load() } catch (e) { setError(e instanceof Error ? e.message : 'Não foi possível criar a variação.') }
  }
  async function changeStock(id: number, value: number) { try { await updateVariationStock(id, value); setMessage('Estoque atualizado.'); setError(null); await load() } catch (e) { setError(e instanceof Error ? e.message : 'Não foi possível atualizar o estoque.') } }
  async function changeSize(productIdValue: number, variationId: number, tamanho: string, quantidadeDisponivel: number) { try { await updateVariation(variationId, { produtoId: productIdValue, tamanho, quantidadeDisponivel }); setMessage('Tamanho atualizado.'); setError(null); await load() } catch (e) { setError(e instanceof Error ? e.message : 'Não foi possível atualizar o tamanho.') } }
  async function removeVariation(id: number) { if (!window.confirm('Excluir esta variação?')) return; try { await deleteVariation(id); setMessage('Variação removida.'); setError(null); await load() } catch (e) { setError(e instanceof Error ? e.message : 'Não foi possível excluir a variação.') } }

  return <><AdminResourcePage config={productConfig} /><section className={styles.page}><header className={styles.heading}><div><span>8.5 · Variações</span><h1>Estoque</h1><p>Crie tamanhos e atualize as quantidades disponíveis.</p></div></header>{message && <p className={styles.success}>{message}</p>}{error && <p className={styles.error}>{error}</p>}<form className={styles.inlineForm} onSubmit={addVariation}><select required value={productId} onChange={(e) => setProductId(e.target.value)}><option value="">Selecione o produto</option>{products.map((p) => <option key={p.id} value={p.id}>{p.nome}</option>)}</select><input required placeholder="Tamanho" value={size} onChange={(e) => setSize(e.target.value)} /><input type="number" min="0" value={stock} onChange={(e) => setStock(Number(e.target.value))} /><button>Adicionar variação</button></form><div className={styles.list}>{products.flatMap((product) => product.variacoes.map((variation) => <article key={variation.id}><div><strong>{product.nome}</strong><span>Variação #{variation.id}</span></div><div><label className={styles.stock}>Tamanho<input defaultValue={variation.tamanho} onBlur={(e) => { if (e.target.value !== variation.tamanho) void changeSize(product.id, variation.id, e.target.value, variation.quantidadeDisponivel) }} /></label><label className={styles.stock}>Estoque<input type="number" min="0" defaultValue={variation.quantidadeDisponivel} onBlur={(e) => changeStock(variation.id, Number(e.target.value))} /></label><button type="button" onClick={() => removeVariation(variation.id)}>Excluir</button></div></article>))}</div></section></>
}
