import type { CreateReservation, ReservationResponse } from '../types/reservation'
import { apiPost } from './client'

export const createReservation = (reservation: CreateReservation, signal?: AbortSignal) =>
  apiPost<ReservationResponse, CreateReservation>('/api/Reserva', reservation, signal)
