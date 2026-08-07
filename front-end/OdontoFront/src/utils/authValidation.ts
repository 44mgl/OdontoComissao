export type AdminLoginErrors = {
  email?: string
  password?: string
}

export type VipLoginErrors = {
  identificationNumber?: string
  password?: string
}

const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

export function validateAdminLogin(email: string, password: string) {
  const errors: AdminLoginErrors = {}
  const normalizedEmail = email.trim()

  if (!normalizedEmail) {
    errors.email = 'Informe o e-mail.'
  } else if (!EMAIL_PATTERN.test(normalizedEmail)) {
    errors.email = 'Informe um e-mail válido.'
  }

  if (!password) {
    errors.password = 'Informe a senha.'
  } else if (password.length < 6) {
    errors.password = 'A senha deve ter pelo menos 6 caracteres.'
  }

  return errors
}

export function validateVipLogin(identificationNumber: string, password: string) {
  const errors: VipLoginErrors = {}

  if (!identificationNumber.trim()) {
    errors.identificationNumber = 'Informe o número de identificação.'
  }

  if (!password) {
    errors.password = 'Informe a senha.'
  } else if (password.length < 6) {
    errors.password = 'A senha deve ter pelo menos 6 caracteres.'
  }

  return errors
}
