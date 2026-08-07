# Contexto do projeto para o front-end

Este documento registra o contexto funcional e técnico do **Odonto Comissão** para orientar a criação do front-end e manter as decisões alinhadas com o backend existente.

Fonte técnica detalhada da API: [`back-end/ApiOdonto/README.md`](back-end/ApiOdonto/README.md).

## 1. Objetivo do sistema

O site representa uma comissão de turma de Odontologia e deve oferecer:

- página inicial com informações e destaques;
- cronograma de eventos atualizado;
- apresentação dos integrantes da comissão;
- loja para visualizar produtos e fazer reservas, sem pagamento online;
- área VIP com produtos exclusivos;
- painel administrativo para a comissão gerenciar o conteúdo sem alterar código.

O backend já está estruturado para esses fluxos. O front-end será responsável pela interface, navegação, formulários, consumo da API e apresentação dos erros.

## 2. Perfis e áreas da interface

### Público

Não precisa fazer login para:

- visualizar destaques;
- visualizar próximos eventos;
- visualizar membros da comissão;
- visualizar o catálogo público;
- criar uma reserva de produtos públicos;
- consultar uma reserva pelo código.

### Membro VIP

O membro VIP é cadastrado pela administração. Não existe cadastro VIP público.

Depois do login por número de identificação e senha, pode:

- visualizar o catálogo VIP;
- reservar produtos exclusivos;
- utilizar o vínculo VIP obtido do token.

O front-end nunca deve pedir ou enviar `membroVipId` no formulário de reserva.

### Administrador

Depois do login por e-mail e senha, pode gerenciar:

- administradores;
- eventos;
- publicações e destaques;
- membros da comissão;
- membros VIP e validade de acesso;
- produtos;
- variações e estoque;
- reservas e seus status.

## 3. Backend e ambiente local

Tecnologias atuais do backend:

- ASP.NET Core 10;
- Entity Framework Core 10;
- SQL Server LocalDB no desenvolvimento;
- JWT Bearer em cookie;
- BCrypt para senhas;
- Swagger/OpenAPI.

URLs locais:

```text
API HTTPS: https://localhost:7103
API HTTP:  http://localhost:5255
Swagger:   https://localhost:7103/swagger
```

Para o front-end local com Vite, a origem esperada é:

```text
http://localhost:5173
```

O `.env` do backend deve conter:

```env
FRONTEND_URL=http://localhost:5173
```

Se a porta do front-end mudar, essa variável também precisa mudar. Uma origem CORS considera protocolo, domínio e porta.

## 4. Autenticação pelo front-end

O JWT é gravado pelo backend no cookie `access_token` com `HttpOnly`, `Secure` e `SameSite=None`.

Consequências para o front-end:

- não ler o cookie com JavaScript;
- não salvar o token em `localStorage` ou `sessionStorage`;
- enviar `credentials: "include"` nas requisições;
- manter o estado do usuário a partir da resposta do login e de `/api/Auth/me`;
- redirecionar para o login ao receber `401`;
- mostrar acesso negado ao receber `403`.

Exemplo com `fetch`:

```ts
const response = await fetch(`${API_URL}/api/Auth/me`, {
  credentials: "include"
});
```

Uma função central de acesso à API deve aplicar `credentials: "include"` em todas as requisições.

### Login administrativo

```http
POST /api/Auth/login
```

```json
{
  "email": "admin@exemplo.com",
  "senha": "senha-do-administrador"
}
```

Resposta principal:

```ts
interface LoginAdministradorResponse {
  token: string;
  expiracao: string;
  administradorId: number;
  nome: string;
  email: string;
}
```

Embora o token também esteja na resposta, o front-end deve preferir o cookie `HttpOnly` e não persistir o token manualmente.

### Login VIP

Rota atual:

```http
POST /VipAuth/login
```

```json
{
  "numeroIdentificacao": "VIP-001",
  "senha": "senha-do-membro"
}
```

```ts
interface LoginVipResponse {
  token: string;
  membroVipId: number;
  nome: string;
  numeroIdentificacao: string;
}
```

> Atenção: esta é a única rota de autenticação sem o prefixo `/api`. Antes de consolidar o cliente HTTP, é recomendável padronizar o backend para `/api/VipAuth/login`.

### Sessão e logout

```http
GET  /api/Auth/me
POST /api/Auth/logout
```

`/me` devolve as claims disponíveis:

```ts
interface UsuarioAtual {
  id: string | null;
  nome: string | null;
  email: string | null;
  perfil: "Administrador" | "VIP" | null;
}
```

O campo `email` pode ser nulo no token VIP.

## 5. Endpoints necessários por tela

### Página inicial

| Conteúdo | Requisição |
|---|---|
| Destaques | `GET /api/Publicacao/destaques` |
| Próximos eventos | `GET /api/Evento/proximos` |
| Comissão | `GET /api/MembroComissao/ordenados` |

### Loja pública

```http
GET /api/Produto/catalogo
```

Retorna apenas produtos públicos ativos e suas variações ativas.

### Área VIP

```http
GET /api/Produto/vip
```

Exige role `VIP` e cookie válido.

### Reserva

Criar:

```http
POST /api/Reserva
```

Consultar pelo código:

```http
GET /api/Reserva/codigo?codigoReserva=RES-...
```

### Painel administrativo

| Recurso | Base da API |
|---|---|
| Administradores | `/api/Administrador` |
| Eventos | `/api/Evento` |
| Publicações | `/api/Publicacao` |
| Comissão | `/api/MembroComissao` |
| Membros VIP | `/api/MembroVip` |
| Produtos | `/api/Produto` |
| Variações | `/api/VariacaoProduto` |
| Reservas | `/api/Reserva` |

Os endpoints administrativos exigem role `Administrador`. A descrição completa de GET, POST, PUT, PATCH e DELETE está no README da API e no Swagger.

## 6. Contratos principais para TypeScript

Os nomes JSON do ASP.NET Core são enviados normalmente em `camelCase`.

### Produto e variação

```ts
interface VariacaoProduto {
  id: number;
  produtoId: number;
  tamanho: string;
  quantidadeDisponivel: number;
  ativo: boolean;
}

interface Produto {
  id: number;
  nome: string;
  descricao: string;
  preco: number;
  imagemUrl: string | null;
  categoria: string;
  exclusivoVip: boolean;
  ativo: boolean;
  dataCriacao: string;
  dataAtualizacao: string;
  variacoes: VariacaoProduto[];
}
```

No front-end, `number` representa o `decimal` recebido no JSON. Para exibição monetária, usar:

```ts
new Intl.NumberFormat("pt-BR", {
  style: "currency",
  currency: "BRL"
}).format(produto.preco);
```

### Evento

```ts
type StatusEvento = 1 | 2 | 3 | 4;

interface Evento {
  id: number;
  titulo: string;
  descricao: string;
  dataHora: string;
  local: string;
  categoria: string;
  status: StatusEvento;
  imagemUrl: string | null;
  ativo: boolean;
  dataCriacao: string;
  dataAtualizacao: string;
}
```

Mapeamento:

```ts
const statusEvento = {
  1: "Agendado",
  2: "Em andamento",
  3: "Concluído",
  4: "Cancelado"
} as const;
```

### Publicação

```ts
interface Publicacao {
  id: number;
  titulo: string;
  descricao: string;
  imagemUrl: string | null;
  tipo: string;
  ordemExibicao: number;
  destaque: boolean;
  ativo: boolean;
  dataPublicacao: string;
}
```

### Criar reserva

```ts
interface CriarItemReserva {
  variacaoProdutoId: number;
  quantidade: number;
}

interface CriarReserva {
  nomeCliente: string;
  contato: string;
  observacoes?: string | null;
  itens: CriarItemReserva[];
}
```

Não enviar os campos abaixo:

```text
preco
precoUnitario
membroVipId
codigoReserva
status
```

Esses valores são definidos pelo backend.

### Consulta pública da reserva

```ts
type StatusReserva = 1 | 2 | 3 | 4 | 5;

interface ItemReservaPublica {
  produto: string;
  tamanho: string;
  quantidade: number;
  precoUnitario: number;
  subtotal: number;
}

interface ReservaPublica {
  codigoReserva: string;
  dataReserva: string;
  status: StatusReserva;
  dataAtualizacao: string;
  valorTotal: number;
  itens: ItemReservaPublica[];
}
```

Mapeamento:

```ts
const statusReserva = {
  1: "Pendente",
  2: "Confirmada",
  3: "Separada",
  4: "Retirada",
  5: "Cancelada"
} as const;
```

## 7. Regras que pertencem ao backend

O front-end pode orientar e validar para melhorar a experiência, mas o backend é a fonte final destas regras:

- preço atual do produto;
- disponibilidade e redução do estoque;
- validade e estado do membro VIP;
- exclusividade VIP;
- geração do código da reserva;
- agrupamento de variações repetidas;
- rejeição integral da reserva se qualquer item for inválido;
- transições permitidas de status;
- devolução do estoque no cancelamento;
- unicidade de e-mail, identificação e variações;
- autorização de cada endpoint.

Não duplicar essas regras como se o resultado do front-end fosse confiável. Dados do navegador podem ser alterados pelo usuário.

## 8. Tratamento de respostas e erros

| HTTP | Ação esperada no front-end |
|---:|---|
| `200` | Ler e apresentar os dados |
| `201` | Confirmar criação e usar o recurso devolvido |
| `204` | Confirmar operação sem tentar ler JSON |
| `400` | Mostrar validação ou mensagem da regra de negócio |
| `401` | Limpar estado local da sessão e solicitar login |
| `403` | Mostrar que o perfil não possui permissão |
| `404` | Mostrar recurso não encontrado |
| `500` | Mostrar mensagem genérica e permitir nova tentativa |

O middleware usa um DTO com propriedade `Message`. Dependendo da forma de serialização, a resposta pode aparecer como `Message`, enquanto outras respostas do ASP.NET usam `message`. O cliente pode normalizar temporariamente:

```ts
const mensagem = body.message ?? body.Message ?? "Não foi possível concluir a operação.";
```

Respostas automáticas de validação de DTO podem possuir uma coleção `errors`. O cliente HTTP deve tratar esse formato separadamente.

Nunca executar `response.json()` automaticamente para uma resposta `204 No Content`.

## 9. Datas e horário

O backend trabalha com `DateTime.UtcNow`. Horários recebidos com sufixo `Z` estão em UTC.

Para mostrar no horário local do navegador:

```ts
new Intl.DateTimeFormat("pt-BR", {
  dateStyle: "short",
  timeStyle: "short"
}).format(new Date(dataUtc));
```

Ao enviar datas de formulário, decidir explicitamente se o valor digitado representa horário de Brasília e convertê-lo para ISO/UTC. Não remover três horas manualmente.

## 10. Estrutura inicial sugerida

A tecnologia do front-end ainda será decidida. Caso seja React com TypeScript e Vite, uma separação inicial possível é:

```text
front-end/
|-- src/
|   |-- api/          cliente HTTP e endpoints
|   |-- components/   componentes reutilizáveis
|   |-- contexts/     autenticação e sessão
|   |-- layouts/      público, VIP e administrativo
|   |-- pages/        telas por rota
|   |-- routes/       rotas e proteções por perfil
|   |-- types/        interfaces dos DTOs
|   |-- utils/        moeda, datas e mensagens
|   `-- main.tsx
|-- .env.example
`-- package.json
```

Possíveis rotas visuais:

```text
/
/eventos
/comissao
/loja
/reserva/:codigo
/vip/login
/vip/produtos
/admin/login
/admin
/admin/eventos
/admin/publicacoes
/admin/comissao
/admin/vips
/admin/produtos
/admin/reservas
/admin/administradores
```

Essa estrutura é uma sugestão, não uma decisão já implementada.

## 11. Checklist antes de iniciar o front-end

- [ ] Padronizar ou confirmar a rota de login VIP (`/VipAuth/login`).
- [ ] Confirmar `FRONTEND_URL=http://localhost:5173` no `.env` do backend.
- [ ] Iniciar o SQL Server LocalDB.
- [ ] Iniciar a API em HTTPS e confiar no certificado local.
- [ ] Confirmar login administrativo com o administrador inicial.
- [ ] Definir stack visual, identidade e responsividade.
- [ ] Criar uma variável `VITE_API_URL=https://localhost:7103` no front-end.
- [ ] Criar um cliente HTTP único com `credentials: "include"`.

## 12. Estado atual e próximos passos

O backend está pronto para iniciar a integração do front-end, mas ainda precisa de uma rodada final de testes integrados durante a construção das telas.

Pontos conhecidos para acompanhar:

- padronização do prefixo da rota VIP;
- teste funcional final do middleware de regras de negócio;
- normalização do formato das mensagens de erro;
- testes dos cookies entre as origens do front-end e backend;
- configurações definitivas de hospedagem, domínio, HTTPS e SQL Server de produção;
- testes automatizados ainda não foram implementados.

Ao implementar cada módulo do front-end, testar o fluxo correspondente de ponta a ponta no backend.

## 13. Contratos adicionados para a área VIP

Implementados em 7 de agosto de 2026:

- `GET /api/Produto/vip`: catálogo protegido pelo perfil `VIP`;
- `POST /api/Reserva`: quando a sessão é VIP, associa a reserva ao ID obtido da
  claim `NameIdentifier`; o frontend não envia `membroVipId`;
- `GET /api/VipArea/perfil`: devolve somente o perfil associado ao token VIP;
- `GET /api/VipArea/reservas`: devolve somente as reservas associadas ao token VIP.

Os endpoints de autoatendimento não recebem ID de membro por rota ou query string.
Essa decisão impede que um VIP tente consultar os dados de outro apenas alterando um
identificador no navegador.
