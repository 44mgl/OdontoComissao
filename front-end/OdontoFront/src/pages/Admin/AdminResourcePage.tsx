import { useCallback, useEffect, useState, type FormEvent } from 'react'
import { createAdminResource, deleteAdminResource, updateAdminResource } from '../../api/admin'
import { ApiError } from '../../api/client'
import type { FormValues, ResourceConfig, ResourceItem } from './resourceConfig'
import styles from './AdminPages.module.css'

export function AdminResourcePage({ config }: { config: ResourceConfig }) {
  const [items, setItems] = useState<ResourceItem[]>([])
  const getInitialValues = useCallback(() => Object.fromEntries(
    config.fields.map((field) => [
      field.name,
      field.type === 'checkbox'
        ? field.name === 'ativo'
        : field.type === 'select'
          ? field.options?.[0]?.value ?? ''
          : '',
    ]),
  ) as FormValues, [config])
  const [values, setValues] = useState<FormValues>(() => getInitialValues())
  const [editing, setEditing] = useState<ResourceItem | null>(null)
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [message, setMessage] = useState<string | null>(null)
  const [error, setError] = useState<string | null>(null)

  const load = useCallback(async (signal?: AbortSignal) => {
    setLoading(true)
    try { setItems(await config.load(signal)); setError(null) }
    catch (requestError) { if (!(requestError instanceof DOMException && requestError.name === 'AbortError')) setError(requestError instanceof Error ? requestError.message : 'Erro ao carregar registros.') }
    finally { setLoading(false) }
  }, [config])

  useEffect(() => { const controller = new AbortController(); void load(controller.signal); return () => controller.abort() }, [load])

  function startCreate() { setEditing(null); setValues(getInitialValues()); setMessage(null); setError(null) }
  function startEdit(item: ResourceItem) { setEditing(item); setValues(config.toValues(item)); setMessage(null); setError(null) }
  function change(name: string, value: string | boolean) { setValues((current) => ({ ...current, [name]: value })) }

  async function submit(event: FormEvent) {
    event.preventDefault(); setSaving(true); setError(null); setMessage(null)
    try {
      const body = config.toBody(values, Boolean(editing))
      if (editing) await updateAdminResource(`${config.endpoint}/${editing.id}`, body)
      else await createAdminResource<ResourceItem, typeof body>(config.endpoint, body)
      setMessage(`${config.singular} ${editing ? 'atualizado' : 'criado'} com sucesso.`); setEditing(null); setValues(getInitialValues()); await load()
    } catch (requestError) { setError(requestError instanceof ApiError ? requestError.message : 'Não foi possível salvar.') }
    finally { setSaving(false) }
  }

  async function remove(item: ResourceItem) {
    if (!window.confirm(`Excluir ${config.getTitle(item)}?`)) return
    try { await deleteAdminResource(`${config.endpoint}/${item.id}`); setMessage(`${config.singular} removido com sucesso.`); await load() }
    catch (requestError) { setError(requestError instanceof Error ? requestError.message : 'Não foi possível excluir.') }
  }

  return (
    <section className={styles.page}>
      <header className={styles.heading}><div><span>{config.section}</span><h1>{config.title}</h1><p>{config.description}</p></div><button type="button" onClick={startCreate}>Novo registro</button></header>
      {message && <p className={styles.success} role="status">{message}</p>}
      {error && <p className={styles.error} role="alert">{error}</p>}
      <div className={styles.workspace}>
        <div className={styles.list}>
          {loading && <p>Carregando...</p>}
          {!loading && items.length === 0 && <p>Nenhum registro encontrado.</p>}
          {items.map((item) => <article key={item.id}><div><strong>{config.getTitle(item)}</strong><span>{config.getSubtitle(item)}</span></div><div><button type="button" onClick={() => startEdit(item)}>Editar</button>{config.canDelete && <button type="button" onClick={() => remove(item)}>Excluir</button>}</div></article>)}
        </div>
        <form className={styles.form} onSubmit={submit}>
          <h2>{editing ? `Editar ${config.singular}` : `Novo ${config.singular}`}</h2>
          {config.fields.map((field) => field.type === 'checkbox' ? (
            <label className={styles.check} key={field.name}><input type="checkbox" checked={Boolean(values[field.name])} onChange={(e) => change(field.name, e.target.checked)} />{field.label}</label>
          ) : (
            <label key={field.name}>{field.label}
              {field.type === 'textarea' ? <textarea rows={4} required={field.required} value={String(values[field.name] ?? '')} onChange={(e) => change(field.name, e.target.value)} /> : field.type === 'select' ? <select value={String(values[field.name] ?? field.options?.[0]?.value ?? '')} onChange={(e) => change(field.name, e.target.value)}>{field.options?.map((option) => <option key={option.value} value={option.value}>{option.label}</option>)}</select> : <input type={field.type ?? 'text'} required={field.required && !(editing && field.type === 'password')} min={field.type === 'number' ? 0 : undefined} step={field.type === 'number' ? 'any' : undefined} value={String(values[field.name] ?? '')} onChange={(e) => change(field.name, e.target.value)} />}
            </label>
          ))}
          <button type="submit" disabled={saving}>{saving ? 'Salvando...' : 'Salvar'}</button>
        </form>
      </div>
    </section>
  )
}
