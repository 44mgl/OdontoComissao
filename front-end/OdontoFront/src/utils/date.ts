const asDate = (value: string) => new Date(value)

export function formatDate(value: string, part: 'day' | 'month') {
  return new Intl.DateTimeFormat('pt-BR',
    part === 'day' ? { day: '2-digit' } : { month: 'short' },
  ).format(asDate(value)).replace('.', '')
}

export function formatTime(value: string) {
  return new Intl.DateTimeFormat('pt-BR', {
    hour: '2-digit',
    minute: '2-digit',
  }).format(asDate(value))
}
