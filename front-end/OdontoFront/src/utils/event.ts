import type { Evento } from '../types/content'

export const eventStatus: Record<Evento['status'], string> = {
  1: 'Agendado',
  2: 'Em andamento',
  3: 'Concluído',
  4: 'Cancelado',
}

export function formatLongDate(value: string) {
  return new Intl.DateTimeFormat('pt-BR', {
    weekday: 'long',
    day: '2-digit',
    month: 'long',
    year: 'numeric',
    timeZone: 'America/Sao_Paulo',
  }).format(new Date(value))
}
