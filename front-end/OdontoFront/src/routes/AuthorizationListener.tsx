import { useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { AUTH_FORBIDDEN_EVENT } from '../api/client'

export function AuthorizationListener() {
  const navigate = useNavigate()

  useEffect(() => {
    const handleForbidden = () => navigate('/acesso-negado')

    window.addEventListener(AUTH_FORBIDDEN_EVENT, handleForbidden)
    return () => window.removeEventListener(AUTH_FORBIDDEN_EVENT, handleForbidden)
  }, [navigate])

  return null
}
