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

## Etapa 1 — Definir as decisões mínimas da arquitetura — concluída

Antes de instalar dependências, responder e registrar:

- qual roteador será usado;
- como formulários e validações serão tratados;
- qual estratégia de testes será adotada;
- se o CSS será puro, por módulos ou apoiado por outra solução;
- como serão exibidos avisos temporários, modais e confirmações;
- como imagens serão fornecidas pelo back-end.

### Perguntas e decisões registradas

#### A identidade visual já está definida?

Sim. A especificação define dourado, vermelho bordô e preto, com estilo minimalista,
elegante, moderno e glamoroso. Ainda será necessário escolher os tons exatos da
paleta e as cores auxiliares de fundo, texto, borda, sucesso e erro. Essa escolha
deverá preservar contraste, legibilidade, consistência e responsividade.

#### Qual roteador será usado e por quê?

Será usado o React Router. Como o sistema terá páginas públicas, VIP e
administrativas, associar cada tela a uma URL facilitará a navegação e a manutenção.
Também permitirá usar normalmente os controles de voltar e avançar do navegador,
compartilhar endereços e proteger separadamente as rotas VIP e administrativas.

As telas continuarão separadas em componentes e arquivos próprios; o roteador não é
o responsável por essa separação, mas por relacioná-las às URLs e controlar a
navegação entre elas.

#### Como formulários e validações serão tratados?

No início, os formulários simples serão construídos apenas com os recursos do React,
usando useState, onChange, onSubmit, campos controlados, validação básica,
mensagens de erro e estado de envio. O objetivo é compreender primeiro como os
formulários funcionam no React sem esconder esse mecanismo atrás de uma biblioteca.

O React Hook Form não será instalado agora. Ele será reavaliado quando surgir um
formulário grande ou repetitivo, como reserva, produtos ou variações. Nesse momento,
a implementação poderá ser comparada com a abordagem feita somente com React para
entender concretamente o problema resolvido pela biblioteca.

O Zod também ficará para uma etapa posterior, caso as regras de validação se
tornem complexas. Não serão introduzidas React Hook Form e Zod simultaneamente apenas
por antecipação.

#### Qual estratégia de testes será adotada?

Os testes priorizarão comportamentos importantes para o usuário e para as regras do
sistema. Por exemplo, terá mais valor verificar que uma reserva sem nome ou contato
é rejeitada do que testar pequenas diferenças de cor ou espaçamento.

Será usado o Vitest como executor de testes, por sua boa integração com o projeto
criado com Vite. Vite e Vitest possuem funções diferentes: o Vite executa e gera a
aplicação, enquanto o Vitest executa os testes automatizados.

A Testing Library será usada para testar componentes pela maneira como a pessoa
interage com eles, como preencher campos, clicar em botões e observar mensagens. No
início, serão criados poucos testes, ligados a componentes e comportamentos reais já
implementados. Testes de ponta a ponta e ferramentas mais complexas ficarão para
quando existir um fluxo completo que justifique seu uso.

A ordem inicial será:

1. testes unitários para funções pequenas e regras puras;
2. testes de componentes para formulários, validações e estados da interface;
3. testes de integração quando as páginas começarem a consumir a API;
4. testes de ponta a ponta depois que os principais fluxos estiverem completos.

#### Como o CSS será organizado?

Serão usados CSS Modules nos componentes e páginas. Cada arquivo de estilos será
associado ao componente correspondente, reduzindo conflitos entre nomes de classes e
facilitando encontrar o código responsável por cada parte da interface.

Um arquivo CSS global continuará sendo usado somente para:

- reset e normalização básica do navegador;
- tokens de cores, tipografia, espaçamento, raios e sombras;
- estilos gerais do documento, como body, links e foco visível;
- regras realmente compartilhadas por toda a aplicação.

Essa estratégia permitirá praticar CSS diretamente, sem introduzir uma biblioteca de
estilos, e manterá os estilos locais organizados conforme o projeto crescer.

#### Como serão exibidos avisos, modais e confirmações?

Mensagens de erro, sucesso, carregamento e ausência de dados serão apresentadas
inicialmente dentro da própria página, próximas do conteúdo ou da ação a que se
referem. Isso mantém o feedback visível e reduz a necessidade de novas dependências.

Avisos temporários serão introduzidos somente quando existir um caso real em que uma
mensagem global e breve seja mais adequada. Modais também serão criados apenas no
primeiro uso concreto, evitando componentes antecipados sem requisitos definidos.

Ações destrutivas, como excluir um produto, deverão pedir confirmação antes de serem
executadas. A confirmação mostrará claramente qual ação será realizada e qual item
será afetado. Quando possível, o botão principal usará uma descrição específica,
como Excluir produto, em vez de apenas Confirmar.

#### Como as imagens serão fornecidas?

A primeira versão manterá o contrato atual do back-end baseado em imagemUrl. O
painel administrativo receberá a URL de uma imagem já hospedada, sem implementar
upload de arquivos nesta etapa.

As imagens deverão estar em uma hospedagem controlada ou autorizada para esse uso.
Não serão usados links copiados diretamente de sites de terceiros, pois eles podem
expirar, mudar, impedir a exibição externa ou possuir restrições de uso.

Durante o desenvolvimento, imagens fixas de demonstração também poderão ficar na
pasta pública do front-end. Essa alternativa serve para dados conhecidos do projeto,
mas não permite que a administração adicione novos arquivos sem uma publicação do
site.

O upload direto pelo painel fica registrado como evolução futura. Ele exigirá que o
back-end valide, armazene e disponibilize os arquivos, mantendo no banco a URL
gerada. Se for implementado para a apresentação acadêmica, poderá começar com
armazenamento local; antes de uma implantação real, deverá ser avaliado um
armazenamento externo persistente.

#### TanStack Query será usado inicialmente?

Não. A primeira integração será feita com o cliente HTTP central e hooks próprios,
para tornar explícitos o carregamento, o erro, o cancelamento e a atualização dos
dados. TanStack Query será reavaliado se surgirem necessidades concretas de cache,
revalidação automática, deduplicação de requisições ou sincronização frequente entre
várias telas.

Dependências devem entrar apenas quando resolverem uma necessidade conhecida. Para
este projeto, vale pesquisar:

- React Router;
- React Hook Form e Zod;
- Vitest, Testing Library e Mock Service Worker;
- TanStack Query, comparando seus benefícios com hooks próprios para consumo da API.

**Situação:** decisões essenciais registradas e template inicial validado com lint e
build em 6 de agosto de 2026.

## Etapa 2 — Limpar o template e criar a fundação — concluída

**Concluída em 6 de agosto de 2026.** O template foi substituído por uma fundação
responsiva com rotas, layouts, CSS Modules, tokens visuais e configuração da API.

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

## Etapa 3 — Criar os componentes visuais essenciais — concluída

**Concluída em 6 de agosto de 2026 para o primeiro fluxo público.** Cabeçalho,
rodapé, navegação responsiva, estados de conteúdo e componentes visuais necessários
para a Home estão implementados. Novos componentes serão criados sob demanda nas
próximas telas, especialmente campos e confirmações quando surgirem formulários.

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

## Etapa 4 — Construir a camada de API — concluída

**Concluída em 6 de agosto de 2026.** O cliente HTTPS foi validado com a API real,
incluindo credenciais, dados públicos, cancelamento de chamadas e apresentação dos
estados de carregamento, sucesso, vazio e erro na Home.

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

**Em andamento desde 7 de agosto de 2026.**

Ordem sugerida:

1. **Página inicial — concluída:** destaques, próximos eventos e comissão resumida;
2. **Cronograma — parcialmente concluído:** próximos eventos, status e detalhes estão
   integrados; eventos anteriores aguardam um endpoint público no back-end;
3. **Comissão — concluída:** integrantes ordenados, descrição e fallback de imagem;
4. **Shop — concluído:** catálogo, categorias, variações, tamanhos e estoque;
5. **Reserva — concluída:** seleção de itens, dados do cliente, confirmação e código;
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
