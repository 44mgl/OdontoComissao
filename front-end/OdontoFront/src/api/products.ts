import type { Produto } from '../types/content'
import { apiGet } from './client'

export const getPublicCatalog = (signal?: AbortSignal) =>
  apiGet<Produto[]>('/api/Produto/catalogo', signal)

export const getVipCatalog = (signal?: AbortSignal) =>
  apiGet<Produto[]>('/api/Produto/vip', signal)
