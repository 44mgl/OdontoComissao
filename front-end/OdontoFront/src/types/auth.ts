export type UserRole = 'Administrador' | 'VIP'

export type SessionUser = {
  id: string
  nome: string
  email: string | null
  perfil: UserRole
}

export type SessionStatus = 'loading' | 'authenticated' | 'unauthenticated'

export type AdminLoginRequest = {
  email: string
  senha: string
}

export type AdminLoginResponse = {
  administradorId: number
  nome: string
  email: string
  expiracao: string
}

export type VipLoginRequest = {
  numeroIdentificacao: string
  senha: string
}

export type VipLoginResponse = {
  membroVipId: number
  nome: string
  numeroIdentificacao: string
}
