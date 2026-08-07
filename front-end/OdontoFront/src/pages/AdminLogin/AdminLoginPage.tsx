import { useState, type FormEvent } from 'react'
import { Link } from 'react-router-dom'
import { ApiError } from '../../api/client'
import { useAuth } from '../../hooks/useAuth'
import {
  validateAdminLogin,
  type AdminLoginErrors,
} from '../../utils/authValidation'
import styles from '../Auth/LoginPage.module.css'

export function AdminLoginPage() {
  const { user, loginAdmin } = useAuth()
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [errors, setErrors] = useState<AdminLoginErrors>({})
  const [submitError, setSubmitError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  const isAdmin = user?.perfil === 'Administrador'

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const validationErrors = validateAdminLogin(email, password)
    setErrors(validationErrors)
    setSubmitError(null)

    if (Object.keys(validationErrors).length > 0) {
      return
    }

    setIsSubmitting(true)

    try {
      await loginAdmin(email, password)
      setPassword('')
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

  if (isAdmin) {
    return (
      <main className={styles.page}>
        <section className={styles.card} aria-labelledby="admin-success-title">
          <span className={styles.eyebrow}>Sessão administrativa</span>
          <div className={styles.successMark} aria-hidden="true">✓</div>
          <h1 id="admin-success-title">Bem-vindo, {user.nome}</h1>
          <p>Seu acesso administrativo foi confirmado com segurança.</p>
          <Link className={styles.primaryLink} to="/admin">Acessar o painel</Link>
        </section>
      </main>
    )
  }

  return (
    <main className={styles.page}>
      <section className={styles.introduction}>
        <span className={styles.eyebrow}>Acesso restrito</span>
        <h1>Painel da <em>comissão</em></h1>
        <p>
          Entre com as credenciais administrativas para gerenciar o conteúdo e os
          próximos fluxos do projeto.
        </p>
      </section>

      <section className={styles.card} aria-labelledby="admin-login-title">
        <h2 id="admin-login-title">Entrar como administrador</h2>

        <form onSubmit={handleSubmit} noValidate>
          <label htmlFor="admin-email">
            E-mail
            <input
              id="admin-email"
              name="email"
              type="email"
              autoComplete="username"
              value={email}
              aria-invalid={Boolean(errors.email)}
              aria-describedby={errors.email ? 'admin-email-error' : undefined}
              onChange={(event) => setEmail(event.target.value)}
            />
            {errors.email && <span id="admin-email-error">{errors.email}</span>}
          </label>

          <label htmlFor="admin-password">
            Senha
            <input
              id="admin-password"
              name="password"
              type="password"
              autoComplete="current-password"
              value={password}
              aria-invalid={Boolean(errors.password)}
              aria-describedby={errors.password ? 'admin-password-error' : undefined}
              onChange={(event) => setPassword(event.target.value)}
            />
            {errors.password && (
              <span id="admin-password-error">{errors.password}</span>
            )}
          </label>

          {submitError && (
            <p className={styles.submitError} role="alert">{submitError}</p>
          )}

          <button type="submit" disabled={isSubmitting}>
            {isSubmitting ? 'Verificando...' : 'Entrar no painel'}
          </button>
        </form>

        <Link className={styles.backLink} to="/">← Voltar ao site</Link>
      </section>
    </main>
  )
}
