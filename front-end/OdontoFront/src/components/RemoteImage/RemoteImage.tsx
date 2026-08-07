import { useEffect, useState, type ReactNode } from 'react'

type Props = {
  src: string | null
  alt: string
  className?: string
  fallback: ReactNode
}

export function RemoteImage({ src, alt, className, fallback }: Props) {
  const [failed, setFailed] = useState(false)

  useEffect(() => setFailed(false), [src])

  if (!src || failed) return fallback

  return (
    <img
      className={className}
      src={src}
      alt={alt}
      onError={() => setFailed(true)}
    />
  )
}
