import type { Evento, MembroComissao, Publicacao } from '../types/content'
import { apiGet } from './client'

export const getDestaques = (signal?: AbortSignal) =>
  apiGet<Publicacao[]>('/api/Publicacao/destaques', signal)

export const getProximosEventos = (signal?: AbortSignal) =>
  apiGet<Evento[]>('/api/Evento/proximos', signal)

export const getComissao = (signal?: AbortSignal) =>
  apiGet<MembroComissao[]>('/api/MembroComissao/ordenados', signal)
