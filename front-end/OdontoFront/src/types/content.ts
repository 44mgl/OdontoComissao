export type Publicacao = {
  id: number
  titulo: string
  descricao: string
  imagemUrl: string | null
  tipo: string
  ordemExibicao: number
  destaque: boolean
  ativo: boolean
  dataPublicacao: string
}

export type Evento = {
  id: number
  titulo: string
  descricao: string
  dataHora: string
  local: string
  categoria: string
  status: 1 | 2 | 3 | 4
  imagemUrl: string | null
  ativo: boolean
  dataCriacao: string
  dataAtualizacao: string
}

export type MembroComissao = {
  id: number
  nome: string
  cargo: string
  descricao: string | null
  fotoUrl: string | null
  ordemExibicao: number
  ativo: boolean
}

export type VariacaoProduto = {
  id: number
  produtoId: number
  tamanho: string
  quantidadeDisponivel: number
  ativo: boolean
}

export type Produto = {
  id: number
  nome: string
  descricao: string
  preco: number
  imagemUrl: string | null
  categoria: string
  exclusivoVip: boolean
  ativo: boolean
  dataCriacao: string
  dataAtualizacao: string
  variacoes: VariacaoProduto[]
}
