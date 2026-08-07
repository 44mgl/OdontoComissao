import { getAdministrators, getAdminCommission, getAdminEvents, getAdminProducts, getAdminPublications, getAdminVips } from '../../api/admin'
import { toBrazilDateTimeInput } from '../../utils/date'

export type FormValue = string | boolean
export type FormValues = Record<string, FormValue>
export type ResourceItem = { id: number; [key: string]: unknown }
export type FieldConfig = {
  name: string
  label: string
  type?: 'text' | 'email' | 'password' | 'number' | 'textarea' | 'checkbox' | 'datetime-local' | 'date' | 'select'
  required?: boolean
  options?: { value: string; label: string }[]
}
export type ResourceConfig = {
  section: string
  title: string
  description: string
  endpoint: string
  singular: string
  fields: FieldConfig[]
  load: (signal?: AbortSignal) => Promise<ResourceItem[]>
  getTitle: (item: ResourceItem) => string
  getSubtitle: (item: ResourceItem) => string
  toValues: (item: ResourceItem) => FormValues
  toBody: (values: FormValues, editing: boolean) => Record<string, unknown>
  canDelete?: boolean
}

const text = (values: FormValues, name: string) => String(values[name] ?? '').trim()
const bool = (values: FormValues, name: string) => Boolean(values[name])
const number = (values: FormValues, name: string) => Number(values[name] ?? 0)
const nullable = (value: string) => value || null
const asItems = <T extends { id: number }>(promise: Promise<T[]>) => promise as Promise<ResourceItem[]>

export const publicationConfig: ResourceConfig = {
  section: '8.2 · Conteúdo', title: 'Publicações', description: 'Gerencie avisos e destaques exibidos na página inicial.', endpoint: '/api/Publicacao', singular: 'publicação', canDelete: true,
  fields: [
    { name: 'titulo', label: 'Título', required: true }, { name: 'descricao', label: 'Descrição', type: 'textarea', required: true },
    { name: 'imagemUrl', label: 'URL da imagem' }, { name: 'tipo', label: 'Tipo', required: true },
    { name: 'ordemExibicao', label: 'Ordem', type: 'number', required: true }, { name: 'destaque', label: 'É destaque', type: 'checkbox' },
    { name: 'ativo', label: 'Ativa', type: 'checkbox' },
  ],
  load: (signal) => asItems(getAdminPublications(signal)), getTitle: (i) => String(i.titulo), getSubtitle: (i) => `${i.tipo} · ordem ${i.ordemExibicao}`,
  toValues: (i) => ({ titulo: String(i.titulo), descricao: String(i.descricao), imagemUrl: String(i.imagemUrl ?? ''), tipo: String(i.tipo), ordemExibicao: String(i.ordemExibicao), destaque: Boolean(i.destaque), ativo: Boolean(i.ativo) }),
  toBody: (v, editing) => ({ titulo: text(v, 'titulo'), descricao: text(v, 'descricao'), imagemUrl: nullable(text(v, 'imagemUrl')), tipo: text(v, 'tipo'), ordemExibicao: number(v, 'ordemExibicao'), destaque: bool(v, 'destaque'), ...(editing ? { ativo: bool(v, 'ativo') } : {}) }),
}

export const eventConfig: ResourceConfig = {
  section: '8.3 · Agenda', title: 'Eventos', description: 'Cadastre o cronograma, local e situação de cada evento.', endpoint: '/api/Evento', singular: 'evento', canDelete: true,
  fields: [
    { name: 'titulo', label: 'Título', required: true }, { name: 'descricao', label: 'Descrição', type: 'textarea', required: true },
    { name: 'dataHora', label: 'Data e horário', type: 'datetime-local', required: true }, { name: 'local', label: 'Local', required: true },
    { name: 'categoria', label: 'Categoria', required: true }, { name: 'status', label: 'Status', type: 'select', options: [{ value: '1', label: 'Agendado' }, { value: '2', label: 'Em andamento' }, { value: '3', label: 'Concluído' }, { value: '4', label: 'Cancelado' }] },
    { name: 'imagemUrl', label: 'URL da imagem' }, { name: 'ativo', label: 'Ativo', type: 'checkbox' },
  ],
  load: (signal) => asItems(getAdminEvents(signal)), getTitle: (i) => String(i.titulo), getSubtitle: (i) => `${i.categoria} · ${i.local}`,
  toValues: (i) => ({ titulo: String(i.titulo), descricao: String(i.descricao), dataHora: toBrazilDateTimeInput(String(i.dataHora)), local: String(i.local), categoria: String(i.categoria), status: String(i.status), imagemUrl: String(i.imagemUrl ?? ''), ativo: Boolean(i.ativo) }),
  toBody: (v, editing) => ({ titulo: text(v, 'titulo'), descricao: text(v, 'descricao'), dataHora: new Date(text(v, 'dataHora')).toISOString(), local: text(v, 'local'), categoria: text(v, 'categoria'), status: number(v, 'status'), imagemUrl: nullable(text(v, 'imagemUrl')), ...(editing ? { ativo: bool(v, 'ativo') } : {}) }),
}

export const commissionConfig: ResourceConfig = {
  section: '8.4 · Equipe', title: 'Comissão', description: 'Defina integrantes, cargos e ordem de exibição.', endpoint: '/api/MembroComissao', singular: 'integrante', canDelete: true,
  fields: [{ name: 'nome', label: 'Nome', required: true }, { name: 'cargo', label: 'Cargo', required: true }, { name: 'descricao', label: 'Descrição', type: 'textarea' }, { name: 'fotoUrl', label: 'URL da foto' }, { name: 'ordemExibicao', label: 'Ordem', type: 'number' }, { name: 'ativo', label: 'Ativo', type: 'checkbox' }],
  load: (signal) => asItems(getAdminCommission(signal)), getTitle: (i) => String(i.nome), getSubtitle: (i) => `${i.cargo} · ordem ${i.ordemExibicao}`,
  toValues: (i) => ({ nome: String(i.nome), cargo: String(i.cargo), descricao: String(i.descricao ?? ''), fotoUrl: String(i.fotoUrl ?? ''), ordemExibicao: String(i.ordemExibicao), ativo: Boolean(i.ativo) }),
  toBody: (v, editing) => ({ nome: text(v, 'nome'), cargo: text(v, 'cargo'), descricao: nullable(text(v, 'descricao')), fotoUrl: nullable(text(v, 'fotoUrl')), ordemExibicao: number(v, 'ordemExibicao'), ...(editing ? { ativo: bool(v, 'ativo') } : {}) }),
}

export const vipConfig: ResourceConfig = {
  section: '8.7 · Acessos', title: 'Membros VIP', description: 'Gerencie identificação, validade e situação dos membros.', endpoint: '/api/MembroVip', singular: 'membro VIP', canDelete: true,
  fields: [{ name: 'numeroIdentificacao', label: 'Identificação', required: true }, { name: 'nome', label: 'Nome', required: true }, { name: 'email', label: 'E-mail', type: 'email', required: true }, { name: 'telefone', label: 'Telefone', required: true }, { name: 'senha', label: 'Senha (preencha para alterar)', type: 'password', required: true }, { name: 'dataValidade', label: 'Validade', type: 'date' }, { name: 'ativo', label: 'Ativo', type: 'checkbox' }],
  load: (signal) => asItems(getAdminVips(signal)), getTitle: (i) => String(i.nome), getSubtitle: (i) => `${i.numeroIdentificacao} · ${i.email}`,
  toValues: (i) => ({ numeroIdentificacao: String(i.numeroIdentificacao), nome: String(i.nome), email: String(i.email), telefone: String(i.telefone), senha: '', dataValidade: String(i.dataValidade ?? '').slice(0, 10), ativo: Boolean(i.ativo) }),
  toBody: (v, editing) => ({ numeroIdentificacao: text(v, 'numeroIdentificacao'), nome: text(v, 'nome'), email: text(v, 'email'), telefone: text(v, 'telefone'), senha: editing ? nullable(text(v, 'senha')) : text(v, 'senha'), dataValidade: nullable(text(v, 'dataValidade')), ...(editing ? { ativo: bool(v, 'ativo') } : {}) }),
}

export const productConfig: ResourceConfig = {
  section: '8.5 · Catálogo', title: 'Produtos e estoque', description: 'Gerencie produtos e depois ajuste tamanhos e quantidades.', endpoint: '/api/Produto', singular: 'produto', canDelete: true,
  fields: [{ name: 'nome', label: 'Nome', required: true }, { name: 'descricao', label: 'Descrição', type: 'textarea', required: true }, { name: 'preco', label: 'Preço', type: 'number', required: true }, { name: 'imagemUrl', label: 'URL da imagem' }, { name: 'categoria', label: 'Categoria', required: true }, { name: 'exclusivoVip', label: 'Exclusivo VIP', type: 'checkbox' }, { name: 'ativo', label: 'Ativo', type: 'checkbox' }],
  load: (signal) => asItems(getAdminProducts(signal)), getTitle: (i) => String(i.nome), getSubtitle: (i) => `${i.categoria} · R$ ${Number(i.preco).toFixed(2)}`,
  toValues: (i) => ({ nome: String(i.nome), descricao: String(i.descricao), preco: String(i.preco), imagemUrl: String(i.imagemUrl ?? ''), categoria: String(i.categoria), exclusivoVip: Boolean(i.exclusivoVip), ativo: Boolean(i.ativo) }),
  toBody: (v, editing) => ({ nome: text(v, 'nome'), descricao: text(v, 'descricao'), preco: number(v, 'preco'), imagemUrl: nullable(text(v, 'imagemUrl')), categoria: text(v, 'categoria'), exclusivoVip: bool(v, 'exclusivoVip'), ...(editing ? { ativo: bool(v, 'ativo') } : {}) }),
}

export const administratorConfig: ResourceConfig = {
  section: '8.8 · Segurança', title: 'Administradores', description: 'Cadastre e mantenha os acessos administrativos.', endpoint: '/api/Administrador', singular: 'administrador',
  fields: [{ name: 'nome', label: 'Nome', required: true }, { name: 'email', label: 'E-mail', type: 'email', required: true }, { name: 'senha', label: 'Senha (preencha para alterar)', type: 'password', required: true }, { name: 'ativo', label: 'Ativo', type: 'checkbox' }],
  load: (signal) => asItems(getAdministrators(signal)), getTitle: (i) => String(i.nome), getSubtitle: (i) => String(i.email),
  toValues: (i) => ({ nome: String(i.nome), email: String(i.email), senha: '', ativo: Boolean(i.ativo) }),
  toBody: (v, editing) => ({ nome: text(v, 'nome'), email: text(v, 'email'), senha: editing ? nullable(text(v, 'senha')) : text(v, 'senha'), ...(editing ? { ativo: bool(v, 'ativo') } : {}) }),
}
