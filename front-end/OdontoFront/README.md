# Odonto Comissão — Front-end

Front-end do portal da comissão da turma de Odontologia. A aplicação reúne uma área
pública, uma experiência exclusiva para membros VIP e um painel administrativo para
gerenciar o conteúdo e os processos do projeto.

A identidade visual utiliza preto, dourado e vermelho bordô, com uma interface
minimalista, elegante e responsiva para computadores, tablets e celulares.

## Funcionalidades

### Área pública

- página inicial com destaques, próximos eventos e comissão;
- cronograma de eventos;
- apresentação dos integrantes da comissão;
- catálogo público com tamanhos e estoque visível;
- solicitação de reserva sem pagamento on-line;
- consulta de reserva pelo código;
- estados visuais de carregamento, erro e ausência de dados.

### Área VIP

- login por número de identificação e senha;
- restauração automática da sessão;
- dashboard VIP;
- catálogo de produtos exclusivos;
- reserva autenticada vinculada ao membro pelo token;
- consulta do próprio perfil;
- histórico de reservas com detalhes dos itens;
- acesso administrativo ao modo de visualização da área VIP.

### Painel administrativo

- dashboard com indicadores da API;
- gestão de publicações e destaques;
- gestão de eventos;
- gestão dos integrantes da comissão;
- gestão de produtos, variações e estoque;
- gestão de reservas e transições de status;
- gestão de membros VIP e validade de acesso;
- gestão de administradores;
- visualização da experiência VIP.

## Tecnologias

- React 19;
- TypeScript 6;
- Vite 8;
- React Router 7;
- CSS Modules;
- Fetch API;
- Vitest;
- Testing Library;
- Oxlint.

O projeto não utiliza uma biblioteca externa de componentes, formulários ou cliente
HTTP. Os formulários são controlados pelo React, a comunicação usa `fetch` e os
estilos são escritos em CSS Modules.

## Pré-requisitos

Antes de executar o front-end, instale:

- Node.js `20.19.0` ou superior, ou Node.js `22.12.0` ou superior;
- npm, incluído com o Node.js;
- API do projeto executando em HTTPS;
- certificado HTTPS local do ASP.NET Core confiável no sistema.

Confira as versões instaladas:

```powershell
node --version
npm --version
```

## Instalação

Abra o terminal dentro desta pasta:

```powershell
cd C:\OdontoComissao\front-end\OdontoFront
```

Instale exatamente as versões registradas no `package-lock.json`:

```powershell
npm ci
```

Durante o desenvolvimento também é possível usar `npm install`, mas `npm ci` oferece
uma instalação reproduzível e é a opção recomendada quando o lockfile já existe.

## Variável de ambiente

Copie o arquivo `.env.example` para `.env`:

```powershell
Copy-Item .env.example .env
```

Configuração local:

```env
VITE_API_URL=https://localhost:7103
```

Na Vercel, a mesma chave deve apontar somente para a origem pública do Render, sem repetir `VITE_API_URL=` no valor:

```text
Key:   VITE_API_URL
Value: https://seu-servico.onrender.com
```

Variáveis iniciadas com `VITE_` são incorporadas ao código enviado ao navegador.
Nunca coloque senhas, tokens, chaves privadas ou outros segredos nessas variáveis.

## Executando o projeto

Inicie primeiro a API e depois execute:

```powershell
npm run dev
```

Endereços utilizados no desenvolvimento:

```text
Front-end: http://localhost:5173
API:       https://localhost:7103
Swagger:   https://localhost:7103/swagger
```

O Vite está configurado em HTTP local. Abrir `https://localhost:5173` sem configurar
um certificado para o Vite resulta em `ERR_SSL_PROTOCOL_ERROR`.

## Scripts disponíveis

### Desenvolvimento

```powershell
npm run dev
```

Inicia o servidor do Vite com atualização automática durante o desenvolvimento.

### Build de produção

```powershell
npm run build
```

Executa a verificação do TypeScript e gera a versão otimizada em `dist/`:

```text
tsc -b
vite build
```

A pasta `dist` é um resultado gerado. Ela não deve ser editada manualmente.

### Testes

```powershell
npm test
```

Executa todos os testes uma vez usando o Vitest.

### Lint

```powershell
npm run lint
```

Analisa o código com Oxlint em busca de problemas e padrões inconsistentes.

### Pré-visualização do build

```powershell
npm run preview
```

Serve localmente o conteúdo já gerado em `dist`. Execute `npm run build` antes.

## Dependências de produção

### `react` — `^19.2.8`

Biblioteca principal da interface. Fornece componentes, JSX, hooks e gerenciamento
de estado.

Hooks usados no projeto incluem:

- `useState` para estado local;
- `useEffect` para chamadas e listeners;
- `useMemo` para valores derivados;
- `useCallback` para funções estáveis;
- `useContext` para a sessão compartilhada.

### `react-dom` — `^19.2.8`

Integra o React ao DOM do navegador. O arquivo `main.tsx` usa `createRoot()` para
renderizar a aplicação dentro de `#root`.

### `react-router-dom` — `7.18.2`

Controla as rotas da SPA, layouts aninhados, parâmetros e redirecionamentos.

Recursos usados:

- `BrowserRouter`;
- `Routes` e `Route`;
- `Outlet`;
- `Link` e `NavLink`;
- `Navigate`;
- `useNavigate`;
- `useLocation`;
- `useSearchParams`.

## Dependências de desenvolvimento

### `typescript` — `~6.0.2`

Adiciona tipagem estática e valida os contratos usados pelo front-end. O build não
continua quando há incompatibilidades de tipo.

### `vite` — `^8.2.0`

Servidor de desenvolvimento e ferramenta responsável pelo build otimizado.

### `@vitejs/plugin-react` — `^6.0.4`

Integra o React ao Vite e fornece transformação JSX e Fast Refresh.

### `vitest` — `^4.1.10`

Executor dos testes unitários e de integração. Possui integração direta com a
configuração do Vite.

### `@testing-library/react` — `^16.3.2`

Fornece utilitários para testar componentes React pelo comportamento observável da
interface.

### `@testing-library/jest-dom` — `^7.0.0`

Disponibiliza matchers voltados ao DOM para testes de interface. O projeto preserva
essa dependência para os testes de componentes.

### `jsdom` — `^29.1.1`

Simula um ambiente de navegador para testes que precisam do DOM.

### `oxlint` — `^1.75.0`

Ferramenta rápida de análise estática usada pelo script `npm run lint`.

### `@types/react` — `^19.2.17`

Tipos TypeScript do React.

### `@types/react-dom` — `^19.2.3`

Tipos TypeScript do React DOM.

### `@types/node` — `^24.13.3`

Tipos das APIs do Node.js usados pelas ferramentas e arquivos de configuração.

## Estrutura de pastas

```text
src/
|-- api/          cliente HTTP e funções organizadas por recurso
|-- assets/       brasão, imagens e outros arquivos estáticos
|-- components/   cabeçalho, rodapé e componentes compartilhados
|-- contexts/     contexto e provedor de autenticação
|-- hooks/        hooks reutilizáveis
|-- layouts/      layouts público, VIP e administrativo
|-- pages/        páginas ligadas às rotas
|-- routes/       mapa de rotas, proteção e autorização
|-- types/        contratos TypeScript
|-- utils/        datas, moeda, validações e regras puras
|-- App.tsx       composição principal
|-- index.css     tokens, reset e estilos globais
|-- main.tsx      ponto de entrada
`-- ../vercel.json fallback das rotas SPA na Vercel
```

## Rotas públicas

| Rota | Função |
|---|---|
| `/` | Página inicial |
| `/eventos` | Cronograma |
| `/comissao` | Comissão |
| `/shop` | Catálogo público |
| `/reserva` | Solicitação pública de reserva |
| `/reserva/:codigo` | Detalhes públicos de uma reserva |
| `/consultar-reserva` | Busca por código |
| `/vip/login` | Login VIP |
| `/admin/login` | Login administrativo |
| `/acesso-negado` | Resposta visual para acesso sem permissão |

## Rotas VIP

| Rota | Função | Perfis permitidos |
|---|---|---|
| `/vip` | Dashboard VIP | VIP e Administrador |
| `/vip/produtos` | Catálogo exclusivo | VIP e Administrador |
| `/vip/reserva` | Reserva autenticada | VIP |
| `/vip/reservas` | Histórico do membro | VIP |
| `/vip/perfil` | Perfil do membro | VIP |

O administrador pode visualizar a experiência e o catálogo VIP, mas não assume a
identidade de um membro. Perfil, histórico e reserva vinculada continuam exclusivos
do usuário VIP.

## Rotas administrativas

| Rota | Função |
|---|---|
| `/admin` | Dashboard |
| `/admin/publicacoes` | Gestão de publicações |
| `/admin/eventos` | Gestão de eventos |
| `/admin/comissao` | Gestão da comissão |
| `/admin/produtos` | Produtos, variações e estoque |
| `/admin/reservas` | Reservas e status |
| `/admin/vips` | Membros VIP |
| `/admin/administradores` | Administradores |

Todas exigem o perfil `Administrador`.

## Autenticação e autorização

A API grava o JWT em um cookie chamado `access_token` com estas características:

- `HttpOnly=true`: o JavaScript não consegue ler o token;
- `Secure=true`: o cookie é enviado somente em conexão segura;
- `SameSite=None`: permite a comunicação entre as origens do front-end e da API;
- `Path=/`: o cookie pode acompanhar todas as rotas da API.

O cliente HTTP utiliza em todas as requisições:

```ts
credentials: 'include'
```

O token não é salvo em `localStorage` nem em `sessionStorage`.

### Restauração da sessão

Ao abrir a aplicação, o `AuthProvider` consulta:

```http
GET /api/Auth/me
```

Durante essa consulta, o estado da sessão é `loading`. As páginas protegidas não são
exibidas até que o backend confirme a identidade, evitando flashes de conteúdo.

### Respostas de autorização

- `401`: a sessão é considerada ausente ou expirada e o usuário volta ao login;
- `403`: a sessão continua ativa, mas a interface abre `/acesso-negado`;
- a proteção do React não substitui `[Authorize]` no backend.

## Comunicação com a API

O cliente central está em `src/api/client.ts` e oferece:

- `apiGet`;
- `apiPost`;
- `apiPostNoContent`;
- `apiPut`;
- `apiPatch`;
- `apiDelete`.

Ele também:

- usa `VITE_API_URL`;
- envia o cookie;
- trata respostas `204 No Content`;
- normaliza `message`, `Message`, `mensagem` e erros de validação;
- cria erros com status HTTP;
- emite eventos globais para `401` e `403`;
- aceita `AbortSignal` nas operações de leitura.

## Formulários

Os formulários utilizam componentes controlados do React:

```text
value
onChange
onSubmit
useState
```

As validações visuais melhoram a experiência, mas o backend continua sendo a fonte
final das regras.

O painel usa um componente configurável para cadastros com ciclos semelhantes. As
configurações ficam em `src/pages/Admin/resourceConfig.ts`, evitando duplicação entre
publicações, eventos, comissão, VIPs e administradores.

## Datas e fuso horário

Datas recebidas da API são interpretadas em UTC e exibidas usando:

```text
America/Sao_Paulo
```

O utilitário `src/utils/date.ts` formata datas públicas, horários, datas completas e
valores usados em campos `datetime-local`.

## Reservas

O front-end nunca confia no preço ou no estoque que exibe. Ao enviar uma reserva,
informa apenas:

- dados do cliente;
- observações;
- ID da variação;
- quantidade.

Preço, estoque, código e status são definidos ou validados pelo backend.

Na reserva VIP, o front-end não envia `membroVipId`. O backend obtém o membro pela
claim `NameIdentifier` do token.

Transições administrativas de status:

```text
Pendente -> Confirmada ou Cancelada
Confirmada -> Separada ou Cancelada
Separada -> Retirada ou Cancelada
Retirada -> final
Cancelada -> final
```

## Imagens

O contrato atual utiliza URLs de imagens. O componente `RemoteImage` apresenta um
fallback quando a URL está vazia, expirada ou bloqueada pelo servidor externo.

Evite links temporários de redes sociais. Prefira hospedagem controlada pelo projeto
ou imagens incluídas em `src/assets`.

O upload pelo painel ainda não foi implementado.

## Estilos e responsividade

O arquivo `src/index.css` contém:

- reset global;
- cores e tipografia;
- estilos de foco;
- comportamento para movimento reduzido.

Cada página ou componente possui seu próprio arquivo `.module.css`. Os principais
breakpoints ajustam a interface para tablet e celular.

O painel administrativo também adapta navegação, métricas, listas, formulários,
seletores de status e controles de estoque para telas a partir de 320 px.

Paleta principal:

```css
--color-black: #090909;
--color-surface: #111111;
--color-bordeaux: #80142e;
--color-gold: #c9a44c;
--color-gold-light: #e2c978;
--color-ivory: #f5f0e6;
```

## Testes e qualidade

Os testes atuais cobrem, entre outros pontos:

- formatação monetária;
- datas no horário brasileiro;
- status de eventos e reservas;
- validação da reserva;
- validação dos logins;
- decisão das rotas protegidas;
- resposta `204` do logout;
- contratos da API VIP.

Antes de entregar uma mudança, execute:

```powershell
npm run lint
npm test
npm run build
```

## Problemas comuns

### `ENOENT: Could not read package.json`

O comando foi executado na pasta errada. Entre em:

```powershell
cd C:\OdontoComissao\front-end\OdontoFront
```

### `ERR_SSL_PROTOCOL_ERROR` na porta 5173

Use:

```text
http://localhost:5173
```

O Vite local não está configurado para HTTPS.

### API responde `401`

- faça login novamente;
- confirme `GET /api/Auth/me`;
- confira o cookie `access_token` no DevTools;
- use sempre `localhost`, sem alternar para `127.0.0.1`;
- verifique se o cookie não expirou.

### API responde `403`

A sessão é válida, mas o perfil não possui permissão para o endpoint.

### API não conecta

- confirme `VITE_API_URL=https://localhost:7103`;
- confirme que a API está executando;
- abra o Swagger e aceite o certificado local;
- verifique a origem permitida no CORS do backend.

### Imagem não aparece

- abra a URL diretamente no navegador;
- confirme que ela é pública e permanente;
- evite URLs temporárias do Instagram ou mecanismos de busca;
- verifique bloqueios de hotlink e expiração.

### Alterações do backend não aparecem

Pare e inicie novamente a API para carregar a nova compilação.

## Build e publicação na Vercel

Gere a aplicação:

```powershell
npm run build
```

Os arquivos finais estarão em `dist/`. A hospedagem precisa:

- servir conteúdo estático por HTTPS;
- redirecionar rotas desconhecidas para `index.html`, pois a aplicação é uma SPA;
- configurar `VITE_API_URL` para a API de produção durante o build;
- permitir a origem real do front-end no CORS da API;
- manter cookies e API em HTTPS;
- preservar corretamente os atributos do cookie de autenticação.

Configuração do projeto na Vercel:

```text
Framework Preset: Vite
Root Directory: front-end/OdontoFront
Build Command: npm run build
Output Directory: dist
```

O arquivo `vercel.json` redireciona rotas como `/admin/login`, `/vip` e
`/reserva/:codigo` para `index.html`, permitindo que o React Router resolva a página
mesmo após uma atualização direta do navegador.

Variável de produção:

```env
VITE_API_URL=https://seu-servico.onrender.com
```

Variáveis `VITE_` são incorporadas durante o build. Depois de alterar o valor no
painel da Vercel, gere um novo deploy.

Checklist após a publicação:

- abrir a Home e confirmar dados vindos da API;
- atualizar diretamente uma rota interna e confirmar que não ocorre `404`;
- testar login administrativo e VIP;
- conferir o cookie `access_token` enviado com `credentials: "include"`;
- testar as telas administrativas em 320, 375, 390 e 430 px.


## Autor

Desenvolvido por [Miguel Amores Ramos](https://github.com/44mgl).
