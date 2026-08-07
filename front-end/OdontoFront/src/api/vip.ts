import type { VipProfile, VipReservation } from '../types/vip'
import { apiGet } from './client'

export const getVipProfile = (signal?: AbortSignal) =>
  apiGet<VipProfile>('/api/VipArea/perfil', signal)

export const getVipProfileList = async (signal?: AbortSignal) => [
  await getVipProfile(signal),
]

export const getVipReservations = (signal?: AbortSignal) =>
  apiGet<VipReservation[]>('/api/VipArea/reservas', signal)
