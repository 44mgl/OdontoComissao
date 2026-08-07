import { useCallback, useEffect, useState } from 'react'
import { getAdminReservations, updateReservationStatus } from '../../api/admin'
import type { AdminReservation } from '../../types/admin'
import { formatDateTime } from '../../utils/date'
import { reservationStatus } from '../../utils/reservationStatus'
import styles from './AdminPages.module.css'

const allowedTransitions: Record<AdminReservation['status'], AdminReservation['status'][]> = {
  1: [2, 5],
  2: [3, 5],
  3: [4, 5],
  4: [],
  5: [],
}

export function AdminReservationsPage() {
  const [items, setItems] = useState<AdminReservation[]>([])
  const [message, setMessage] = useState<string | null>(null)
  const [error, setError] = useState<string | null>(null)
  const load = useCallback(async () => setItems(await getAdminReservations()), [])
  useEffect(() => { void load().catch((e: unknown) => setError(e instanceof Error ? e.message : 'Erro ao carregar.')) }, [load])
  async function change(id: number, status: number) { try { await updateReservationStatus(id, status); setMessage('Status atualizado.'); setError(null); await load() } catch (e) { setError(e instanceof Error ? e.message : 'Transição não permitida.') } }
  return <section className={styles.page}><header className={styles.heading}><div><span>8.6 · Operação</span><h1>Reservas</h1><p>Acompanhe solicitações e avance o status conforme o fluxo permitido.</p></div></header>{message && <p className={styles.success}>{message}</p>}{error && <p className={styles.error}>{error}</p>}<div className={styles.list}>{items.map((item) => <article key={item.id}><div><strong>{item.codigoReserva}</strong><span>{item.nomeCliente} · {formatDateTime(item.dataReserva)} · {item.itens.length} item(ns)</span></div><select aria-label={`Status da reserva ${item.codigoReserva}`} value={item.status} disabled={allowedTransitions[item.status].length === 0} onChange={(e) => { const next = Number(e.target.value); if (next !== item.status) void change(item.id, next) }}><option value={item.status}>{reservationStatus[item.status]}</option>{allowedTransitions[item.status].map((status) => <option key={status} value={status}>{reservationStatus[status]}</option>)}</select></article>)}</div></section>
}
