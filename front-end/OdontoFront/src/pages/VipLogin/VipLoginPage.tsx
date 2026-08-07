import { useState, type FormEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { ApiError } from '../../api/client'
import { useAuth } from '../../hooks/useAuth'
import {
  validateVipLogin,
  type VipLoginErrors,
} from '../../utils/authValidation'
import styles from '../Auth/LoginPage.module.css'

export function VipLoginPage() {
  const { user, loginVip } = useAuth()
  const navigate = useNavigate()
  const [identificationNumber, setIdentificationNumber] = useState('')
  const [password, setPassword] = useState('')
  const [errors, setErrors] = useState<VipLoginErrors>({})
  const [submitError, setSubmitError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  const isVip = user?.perfil === 'VIP'

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const validationErrors = validateVipLogin(identificationNumber, password)
    setErrors(validationErrors)
    setSubmitError(null)

    if (Object.keys(validationErrors).length > 0) {
      return
    }

    setIsSubmitting(true)

    try {
      await loginVip(identificationNumber, password)
      setPassword('')
      navigate('/vip')
    } catch (requestError) {
      setSubmitError(
        requestError instanceof ApiError || requestError instanceof Error
          ? requestError.message
          : 'Não foi possível entrar. Tente novamente.',
      )
    } finally {
      setIsSubmitting(false)
    }
  }

  if (isVip) {
    return (
      <main className={styles.page}>
        <section className={styles.card} aria-labelledby="vip-success-title">
          <span className={styles.eyebrow}>Sessão VIP</span>
          <div className={styles.successMark} aria-hidden="true">✓</div>
          <h1 id="vip-success-title">Bem-vindo, {user.nome}</h1>
          <p>Seu acesso à área exclusiva foi confirmado com segurança.</p>
          <Link className={styles.primaryLink} to="/vip">Acessar a área VIP</Link>
        </section>
      </main>
    )
  }

  return (
    <main className={styles.page}>
      <section className={styles.introduction}>
        <span className={styles.eyebrow}>Experiência exclusiva</span>
        <h1>Área <em>VIP</em></h1>
        <p>
          Entre com sua identificação para acessar produtos e conteúdos exclusivos
          da turma.
        </p>
      </section>

      <section className={styles.card} aria-labelledby="vip-login-title">
        <span className={styles.number}>06.3</span>
        <h2 id="vip-login-title">Entrar na área VIP</h2>

        <form onSubmit={handleSubmit} noValidate>
          <label htmlFor="vip-identification">
            Número de identificação
            <input
              id="vip-identification"
              name="identificationNumber"
              type="text"
              autoComplete="username"
              value={identificationNumber}
              aria-invalid={Boolean(errors.identificationNumber)}
              aria-describedby={
                errors.identificationNumber ? 'vip-identification-error' : undefined
              }
              onChange={(event) => setIdentificationNumber(event.target.value)}
            />
            {errors.identificationNumber && (
              <span id="vip-identification-error">
                {errors.identificationNumber}
              </span>
            )}
          </label>

          <label htmlFor="vip-password">
            Senha
            <input
              id="vip-password"
              name="password"
              type="password"
              autoComplete="current-password"
              value={password}
              aria-invalid={Boolean(errors.password)}
              aria-describedby={errors.password ? 'vip-password-error' : undefined}
              onChange={(event) => setPassword(event.target.value)}
            />
            {errors.password && (
              <span id="vip-password-error">{errors.password}</span>
            )}
          </label>

          {submitError && (
            <p className={styles.submitError} role="alert">{submitError}</p>
          )}

          <button type="submit" disabled={isSubmitting}>
            {isSubmitting ? 'Verificando...' : 'Entrar na área VIP'}
          </button>
        </form>

        <Link className={styles.backLink} to="/">← Voltar ao site</Link>
      </section>
    </main>
  )
}
