import { apiGet } from './client'
import { apiPost, apiPostNoContent } from './client'
import type {
  AdminLoginRequest,
  AdminLoginResponse,
  SessionUser,
  VipLoginRequest,
  VipLoginResponse,
} from '../types/auth'

export function getCurrentUser(signal?: AbortSignal) {
  return apiGet<SessionUser>('/api/Auth/me', signal)
}

export function authenticateAdmin(credentials: AdminLoginRequest) {
  return apiPost<AdminLoginResponse, AdminLoginRequest>(
    '/api/Auth/login',
    credentials,
  )
}

export function authenticateVip(credentials: VipLoginRequest) {
  return apiPost<VipLoginResponse, VipLoginRequest>(
    '/VipAuth/login',
    credentials,
  )
}

export function endSession() {
  return apiPostNoContent('/api/Auth/logout')
}
