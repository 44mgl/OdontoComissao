# Caminho de desenvolvimento do front-end

Este documento define a ordem recomendada para construir o front-end do **Odonto
Comissão** com entregas pequenas, testáveis e integradas ao back-end.

Documentos relacionados:

- `CONTEXTO_FRONTEND.md`: contratos e integração com o back-end;
- `ESPECIFICACOES_FRONTEND.md`: requisitos visuais e funcionais;
- `METODO_APRENDIZADO.md`: método de mentoria usado durante o projeto.

Projeto atual: `front-end/OdontoFront`, criado com React 19, TypeScript e Vite.

Ambiente local confirmado em 5 de agosto de 2026:

- front-end: `http://localhost:5173`;
- API: `https://localhost:7103`;
- cookie de autenticação: `Secure=true`.

## Princípio de execução

Não construir todas as telas estáticas para só depois integrar a API. Cada etapa deve
terminar com um fluxo pequeno funcionando de ponta a ponta: interface, estados de
carregamento e erro, API e validação.

Para cada tarefa:

1. entender o contrato e o comportamento esperado;
2. desenhar os estados da interface;
3. implementar a menor versão funcional;
4. testar o caminho feliz e os erros previsíveis;
5. executar lint e build;
6. revisar acessibilidade e responsividade;
7. registrar decisões ou pendências antes de seguir.

## Etapa 0 — Confirmar o ambiente — concluída

Antes de alterar o template:

- executar o front-end em `http://localhost:5173`;
- executar a API em `https://localhost:7103`;
- confiar no certificado HTTPS local de desenvolvimento;
- confirmar `FRONTEND_URL=http://localhost:5173` no back-end;
- testar no Swagger os endpoints públicos;
- testar login, cookie e `GET /api/Auth/me`;
- confirmar ou corrigir a rota `/VipAuth/login`;
- verificar os contratos ainda ausentes para perfil e histórico VIP.

**Situação:** ambiente confirmado com a decisão de usar HTTPS também no
desenvolvimento e manter `Secure=true`. Ao implementar a autenticação, confirmar no
DevTools que o navegador grava e reenvia o cookie nas requisições à API.

## Etapa 1 — Definir as decisões mínimas da arquitetura

Antes de instalar dependências, responder e registrar:

- qual roteador será usado;
- como formulários e validações serão tratados;
- qual estratégia de testes será adotada;
- se o CSS será puro, por módulos ou apoiado por outra solução;
- como serão exibidos avisos temporários, modais e confirmações;
- como imagens serão fornecidas pelo back-end.

Dependências devem entrar apenas quando resolverem uma necessidade conhecida. Para
este projeto, vale pesquisar:

- React Router;
- React Hook Form e Zod;
- Vitest, Testing Library e Mock Service Worker;
- TanStack Query, comparando seus benefícios com hooks próprios para consumo da API.

**Concluída quando:** as escolhas essenciais e os motivos estão registrados e o
projeto continua passando no build.

## Etapa 2 — Limpar o template e criar a fundação

Organizar `src` sem antecipar abstrações desnecessárias:

```text
src/
|-- api/          cliente HTTP e chamadas por recurso
|-- assets/       imagens e arquivos estáticos
|-- components/   componentes compartilhados
|-- contexts/     sessão e autenticação, se necessário
|-- layouts/      público, VIP e administrativo
|-- pages/        páginas ligadas às rotas
|-- routes/       definição e proteção de rotas
|-- styles/       tokens e estilos globais
|-- types/        contratos de dados
|-- utils/        moeda, data e mensagens
`-- main.tsx
```

Nesta etapa:

- remover o conteúdo demonstrativo do Vite;
- configurar `.env.example` com `VITE_API_URL=https://localhost:7103`;
- criar tokens de cor, tipografia, espaçamento, raio e sombra;
- aplicar reset e estilos globais mínimos;
- criar os layouts básicos e uma página de rota não encontrada;
- definir convenções de nomes e importações.

Não colocar segredos em variáveis `VITE_*`: elas são expostas ao navegador.

**Concluída quando:** existe uma aplicação vazia, navegável e responsiva, usando a
paleta bordô, dourado e preto sem conteúdo do template.

## Etapa 3 — Criar os componentes visuais essenciais

Começar somente pelo que será reutilizado nas primeiras páginas:

- botão e link visual;
- campo, área de texto e seleção;
- mensagem de erro de campo;
- card;
- cabeçalho e rodapé;
- indicador de carregamento;
- estado vazio;
- alerta de sucesso ou erro;
- modal de confirmação, apenas quando surgir o primeiro uso real.

Cada componente deve contemplar foco por teclado, estado desabilitado, contraste,
texto alternativo quando aplicável e comportamento em telas pequenas.

**Concluída quando:** os componentes necessários para a primeira tela real funcionam
isoladamente e não contêm regras de negócio.

## Etapa 4 — Construir a camada de API

Criar um cliente HTTP único que:

- use a URL do ambiente;
- envie `credentials: "include"` em todas as chamadas;
- trate corretamente respostas sem conteúdo (`204`);
- normalize `message`, `Message` e erros de validação;
- diferencie `400`, `401`, `403`, `404` e `500`;
- permita cancelamento de requisições quando a tela for desmontada;
- não armazene o JWT no `localStorage` ou `sessionStorage`.

Adicionar tipos apenas dos contratos usados pela etapa atual. Evitar criar dezenas de
interfaces especulativas de uma vez.

**Concluída quando:** uma página simples consome um endpoint público e apresenta
carregamento, sucesso, vazio e falha.

## Etapa 5 — Entregar o núcleo público por fatias verticais

Ordem sugerida:

1. **Página inicial:** destaques, próximos eventos e comissão resumida;
2. **Cronograma:** próximos e anteriores, status e detalhes do evento;
3. **Comissão:** integrantes ordenados;
4. **Shop:** catálogo, variações, tamanhos e estoque;
5. **Reserva:** seleção de itens, dados do cliente, confirmação e código;
6. **Consulta da reserva:** busca e apresentação pelo código.

A página inicial vem primeiro porque valida layout, responsividade e três integrações
somente de leitura. A reserva vem depois do catálogo porque depende da seleção de
produto e variação.

No envio da reserva, nunca confiar em preço ou estoque exibidos nem enviar preço,
`membroVipId`, código ou status. O back-end é a fonte dessas regras.

**Concluída quando:** uma pessoa sem login consegue navegar, reservar produtos
públicos e consultar a reserva pelo código.

## Etapa 6 — Implementar sessão e autorização

Somente após o cliente HTTP estar estável:

- criar o estado da sessão baseado em `/api/Auth/me`;
- implementar login e logout;
- proteger rotas VIP e administrativas por perfil;
- redirecionar `401` para o login adequado;
- apresentar acesso negado para `403`;
- evitar flashes de conteúdo protegido durante a restauração da sessão;
- não usar proteção de rota como substituta da autorização do back-end.

Testar atualização da página, cookie expirado, usuário sem permissão e logout.

**Concluída quando:** sessões VIP e administrativas sobrevivem à atualização da
página e usuários não acessam interfaces destinadas a outro perfil.

## Etapa 7 — Construir a área VIP

Ordem sugerida:

1. login VIP;
2. catálogo exclusivo;
3. reserva autenticada;
4. perfil;
5. histórico e detalhe de reservas.

Perfil e histórico só devem avançar depois da confirmação dos endpoints. O formulário
de reserva VIP não deve solicitar nem enviar `membroVipId`; o vínculo vem do token.

**Concluída quando:** o VIP entra, vê somente seu conteúdo autorizado, reserva e
consulta seus próprios dados.

## Etapa 8 — Construir o painel administrativo

Criar primeiro o layout administrativo e o dashboard com dados reais disponíveis.
Depois, desenvolver módulos nesta ordem:

1. publicações e destaques;
2. eventos;
3. integrantes da comissão;
4. produtos, variações e estoque;
5. reservas e transições de status;
6. membros VIP e validade de acesso;
7. administradores, se confirmado no escopo visual.

Essa ordem começa pelos cadastros mais simples e deixa estoque, reservas e acesso VIP
para quando formulários, tabelas, feedback e autenticação já estiverem maduros.

Cada gestão deve ser concluída antes da próxima: listagem, criação, edição, estados
vazios, erros, confirmação de exclusão e atualização da interface.

**Concluída quando:** a comissão gerencia o conteúdo e os fluxos operacionais sem
alterar código nem depender diretamente do banco de dados.

## Etapa 9 — Qualidade contínua

Estas atividades começam nas etapas anteriores e recebem uma revisão completa aqui:

- testes unitários para formatadores e regras puras;
- testes de componentes para formulários e estados;
- testes de integração para API, sessão e permissões;
- testes de ponta a ponta dos fluxos público, VIP e administrativo;
- navegação por teclado e foco visível;
- contraste e textos alternativos;
- responsividade em celular, tablet e desktop;
- imagens otimizadas e carregamento tardio;
- prevenção de envio duplicado de formulários;
- mensagens sem exposição de detalhes internos;
- lint e build sem erros.

## Etapa 10 — Preparar produção

- definir URLs e origens reais de front-end e API;
- configurar HTTPS e atributos de cookie para o ambiente final;
- revisar CORS;
- decidir hospedagem e estratégia de rotas do SPA;
- configurar logs e monitoramento sem dados sensíveis;
- executar testes de aceitação com dados próximos dos reais;
- documentar implantação e recuperação.

## Ordem resumida

```text
Ambiente
  -> decisões mínimas
  -> fundação visual e estrutural
  -> componentes essenciais
  -> cliente da API
  -> área pública
  -> autenticação e autorização
  -> área VIP
  -> painel administrativo
  -> revisão de qualidade
  -> produção
```

## Próximo passo recomendado

Como a **Etapa 0** foi confirmada, começar pelas decisões da **Etapa 1** antes de
instalar novas bibliotecas ou desenhar todas as telas. O comportamento real do cookie
deverá ser verificado no navegador quando implementarmos a autenticação.

A primeira pergunta de arquitetura é: quais necessidades concretas do projeto exigem
um roteador, uma solução de formulários e uma estratégia de testes, e qual é a opção
mais simples que atende a cada uma delas?
