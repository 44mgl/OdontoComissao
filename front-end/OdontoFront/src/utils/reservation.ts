export type ReservationFormDraft = {
  nomeCliente: string
  contato: string
  itemCount: number
}

export function validateReservationForm(draft: ReservationFormDraft) {
  const errors: Partial<Record<keyof ReservationFormDraft, string>> = {}

  if (draft.nomeCliente.trim().length < 2) {
    errors.nomeCliente = 'Informe o nome completo.'
  }
  if (draft.contato.trim().length < 5) {
    errors.contato = 'Informe um contato válido.'
  }
  if (draft.itemCount < 1) {
    errors.itemCount = 'Adicione pelo menos um produto.'
  }

  return errors
}
