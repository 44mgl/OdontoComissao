const asDate = (value: string) => new Date(value)
const BRAZIL_TIME_ZONE = 'America/Sao_Paulo'

export function formatDate(value: string, part: 'day' | 'month') {
  return new Intl.DateTimeFormat('pt-BR',
    part === 'day'
      ? { day: '2-digit', timeZone: BRAZIL_TIME_ZONE }
      : { month: 'short', timeZone: BRAZIL_TIME_ZONE },
  ).format(asDate(value)).replace('.', '')
}

export function formatTime(value: string) {
  return new Intl.DateTimeFormat('pt-BR', {
    hour: '2-digit',
    minute: '2-digit',
    timeZone: BRAZIL_TIME_ZONE,
  }).format(asDate(value))
}

export function formatDateTime(value: string) {
  return new Intl.DateTimeFormat('pt-BR', {
    dateStyle: 'short',
    timeStyle: 'short',
    timeZone: BRAZIL_TIME_ZONE,
  }).format(asDate(value))
}

export function formatFullDate(value: string) {
  return new Intl.DateTimeFormat('pt-BR', {
    dateStyle: 'long',
    timeZone: BRAZIL_TIME_ZONE,
  }).format(asDate(value))
}

export function toBrazilDateTimeInput(value: string) {
  const parts = new Intl.DateTimeFormat('en-CA', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
    hourCycle: 'h23',
    timeZone: BRAZIL_TIME_ZONE,
  }).formatToParts(asDate(value))
  const part = (type: Intl.DateTimeFormatPartTypes) =>
    parts.find((item) => item.type === type)?.value ?? ''

  return `${part('year')}-${part('month')}-${part('day')}T${part('hour')}:${part('minute')}`
}
