# Especificações do front-end

Este documento armazenará as decisões funcionais, visuais e de experiência do usuário
fornecidas para o front-end do Odonto Comissão.

Ele complementa, sem substituir:

- `CONTEXTO_FRONTEND.md`: contratos, regras e integração com o back-end;
- `METODO_APRENDIZADO.md`: forma de mentoria e colaboração durante o desenvolvimento.

## 1. Identidade visual

- Paleta principal: dourado, vermelho bordô e preto.
- Estilo: minimalista, elegante, moderno e glamoroso.
- A composição visual deve preservar legibilidade, contraste, consistência e
  responsividade, sem perder a sensação sofisticada da identidade.

## 2. Página inicial

A página inicial deverá oferecer:

- destaques inspirados na apresentação visual do Instagram;
- feed de publicações;
- avisos importantes;
- atalhos para as principais áreas do site.

## 3. Cronograma

O cronograma deverá separar ou permitir identificar:

- próximos eventos;
- eventos anteriores;
- status do evento;
- data e horário;
- local;
- imagem associada.

## 4. Comissão

Cada integrante da comissão deverá ser apresentado com:

- foto;
- nome;
- cargo;
- descrição;
- ordem de exibição definida pela administração.

## 5. Shop público

A loja pública deverá possuir:

- catálogo de produtos;
- seleção de tamanhos ou variações;
- estoque disponível visível;
- fluxo de solicitação de reserva;
- ausência de pagamento on-line.

## 6. Área VIP

A área VIP deverá possuir:

- login por identificação e senha;
- catálogo de produtos exclusivos;
- perfil do membro VIP;
- histórico de reservas do membro.

O cadastro de membros VIP não será público; ele pertence ao painel administrativo.

## 7. Painel administrativo

O painel administrativo deverá conter um dashboard e telas para gestão de:

- eventos;
- integrantes da comissão;
- produtos, variações e estoque;
- reservas;
- membros VIP;
- publicações e destaques.

A gestão de administradores consta no escopo técnico do back-end e deverá ser
confirmada no detalhamento das telas administrativas.

## 8. Navegação e comportamento

O front-end deverá possuir:

- rotas públicas;
- rotas protegidas para membros VIP;
- rotas protegidas para administradores;
- formulários adequados a cada operação;
- validações visuais claras;
- mensagens de sucesso e erro;
- tratamento visual para carregamento, ausência de dados e acesso negado.

## 9. Pontos de integração a validar

Antes de implementar os respectivos fluxos, confirmar no back-end:

- endpoint e contrato para exibir o perfil completo do membro VIP;
- endpoint e contrato para listar o histórico de reservas do VIP autenticado;
- forma final da rota de login VIP, atualmente registrada como `/VipAuth/login`;
- inclusão da gestão de administradores no escopo visual do painel.
