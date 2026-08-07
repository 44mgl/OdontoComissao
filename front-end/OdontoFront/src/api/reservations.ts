import type {
  CreateReservation,
  PublicReservation,
  ReservationResponse,
} from '../types/reservation'
import { apiGet, apiPost } from './client'

export const createReservation = (reservation: CreateReservation, signal?: AbortSignal) =>
  apiPost<ReservationResponse, CreateReservation>('/api/Reserva', reservation, signal)

export const getReservationByCode = (code: string, signal?: AbortSignal) =>
  apiGet<PublicReservation>(
    `/api/Reserva/codigo?codigoReserva=${encodeURIComponent(code)}`,
    signal,
  )
