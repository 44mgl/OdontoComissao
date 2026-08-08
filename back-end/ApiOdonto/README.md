# Odonto Comissão — API

API REST do projeto **Odonto Comissão**, criada para gerenciar as informações públicas de uma turma de Odontologia, o catálogo de produtos, os acessos VIP e as reservas sem pagamento pelo site.

O backend foi desenvolvido com **ASP.NET Core 10**, **Entity Framework Core** e **PostgreSQL**. A autenticação de administradores e membros VIP utiliza JWT armazenado em cookie seguro.

## Sumário

- [Funcionalidades](#funcionalidades)
- [Arquitetura](#arquitetura)
- [Tecnologias](#tecnologias)
- [Como executar](#como-executar)
- [Banco de dados](#banco-de-dados)
- [Autenticação e autorização](#autenticação-e-autorização)
- [CORS e consumo pelo front-end](#cors-e-consumo-pelo-front-end)
- [Regras de negócio importantes](#regras-de-negócio-importantes)
- [Endpoints](#endpoints)
- [Exemplos de requisições](#exemplos-de-requisições)
- [Respostas HTTP e middleware](#respostas-http-e-middleware)
- [Estrutura do projeto](#estrutura-do-projeto)

## Funcionalidades

- página inicial alimentada por publicações e destaques;
- cronograma de próximos eventos;
- apresentação dos membros da comissão;
- catálogo público de produtos ativos;
- catálogo exclusivo para membros VIP autenticados;
- reservas de produtos sem pagamento pela aplicação;
- consulta pública de uma reserva por código;
- controle de estoque por variação de produto;
- autenticação de administradores e membros VIP;
- painel administrativo preparado para gerenciar administradores, eventos, publicações, comissão, membros VIP, produtos, variações e reservas.

## Arquitetura

A API utiliza uma arquitetura em camadas:

```text
Front-end / Cliente HTTP
          |
          v
     Controllers       endpoints, autorização e respostas HTTP
          |
          v
       Services        regras de negócio e mapeamento dos DTOs
          |
          v
     Repositories      consultas e persistência
          |
          v
 Entity Framework Core
          |
          v
       PostgreSQL
```

Os **DTOs** definem os dados aceitos e devolvidos pela API. As entidades do banco não são expostas diretamente, especialmente nas consultas públicas.

## Tecnologias

| Tecnologia | Uso |
|---|---|
| .NET 10 / ASP.NET Core | Construção da API REST |
| Entity Framework Core 10 | ORM, relacionamentos e migrations |
| PostgreSQL | Banco de dados relacional |
| JWT Bearer | Autenticação e autorização por perfil |
| BCrypt | Hash e verificação de senhas |
| Swagger / OpenAPI | Exploração e testes manuais dos endpoints |
| DotNetEnv | Leitura das configurações do arquivo `.env` |

### Pacotes NuGet principais

| Pacote | Versão | Finalidade |
|---|---:|---|
| `BCrypt.Net-Next` | 4.2.0 | Hash seguro de senhas |
| `DotNetEnv` | 3.2.0 | Leitura do `.env` |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 10.0.10 | Autenticação JWT |
| `Microsoft.EntityFrameworkCore` | 10.0.10 | ORM |
| `Microsoft.EntityFrameworkCore.Design` | 10.0.10 | Suporte às migrations |
| `Npgsql.EntityFrameworkCore.PostgreSQL` | 10.0.3 | Provider do PostgreSQL |
| `Microsoft.EntityFrameworkCore.Tools` | 10.0.10 | Ferramentas do EF Core |
| `Swashbuckle.AspNetCore` | 10.2.3 | Swagger UI |

## Como executar

### Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download);
- PostgreSQL local ou uma instância hospedada, como o Supabase;
- ferramenta `dotnet-ef` para gerenciar migrations.

### 1. Entre na pasta da API

```powershell
cd back-end/ApiOdonto
```

### 2. Configure o banco

O ambiente local está preparado no `appsettings.json` para utilizar:

```text
Host=localhost
Port=5432
Database=OdontoComissaoDb
Username=postgres
```

Para outro servidor, altere a connection string `DefaultConnection` de acordo com o ambiente.

### 3. Configure as variáveis de ambiente

Copie `.env.example` para `.env` e preencha os valores:

```env
JWT_KEY=uma-chave-secreta-longa-e-aleatoria
JWT_ISSUER=ApiOdonto
JWT_AUDIENCE=OdontoComissaoFrontend
JWT_EXPIRES_MINUTES=120

ADMIN_INICIAL_NOME=Administrador Inicial
ADMIN_INICIAL_EMAIL=admin@exemplo.com
ADMIN_INICIAL_SENHA=uma-senha-forte

FRONTEND_URL=http://localhost:5173
ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=OdontoComissaoDb;Username=postgres;Password=postgres
```

O arquivo `.env` é ignorado pelo Git e não deve ser publicado. O `.env.example` deve conter apenas valores vazios ou exemplos que não sejam segredos reais.

Em produção, `ConnectionStrings__DefaultConnection` deve usar o **Session Pooler**
do Supabase na porta `5432`. Para o pooler, o usuário possui o formato
`postgres.<project-ref>`, e não apenas `postgres`.

### 4. Restaure as dependências

```powershell
dotnet restore
```

Caso `dotnet-ef` não esteja instalado:

```powershell
dotnet tool install --global dotnet-ef
```

### 5. Aplique as migrations

```powershell
dotnet ef database update
```

O `DbInitializer` também executa `MigrateAsync()` na inicialização da API. Em um banco vazio, ele cria o primeiro administrador com as variáveis `ADMIN_INICIAL_*`. Se já existir algum administrador, nenhum novo registro é criado.

### 6. Inicie a API

```powershell
dotnet run
```

Perfis locais:

- HTTP: `http://localhost:5255`
- HTTPS: `https://localhost:7103`
- Swagger: `https://localhost:7103/swagger`

O Swagger é habilitado no ambiente `Development`.

## Banco de dados

O projeto utiliza PostgreSQL e possui uma migration inicial própria do provider Npgsql. A migration cria as tabelas, chaves estrangeiras e índices do sistema.

Restrições importantes no banco:

- e-mail de administrador único;
- e-mail e número de identificação VIP únicos;
- código de reserva único;
- combinação de produto e tamanho da variação única;
- `Produto.Preco` e `ItemReserva.PrecoUnitario` com precisão `decimal(18,2)`.

Os dados antigos do SQLite não são utilizados pela configuração atual. A migração realizada substituiu o provider e a estrutura das migrations, não transferiu registros do arquivo SQLite.

## Autenticação e autorização

Existem dois perfis autenticáveis:

| Perfil | Credencial de login | Role JWT |
|---|---|---|
| Administrador | E-mail e senha | `Administrador` |
| Membro VIP | Número de identificação e senha | `VIP` |

Após um login válido, a API:

1. verifica a senha com BCrypt;
2. gera um JWT assinado;
3. devolve informações do login no corpo;
4. grava o JWT no cookie `access_token`.

O cookie utiliza:

- `HttpOnly=true`: JavaScript não consegue ler o token;
- `Secure=true`: o cookie é enviado por HTTPS;
- `SameSite=None`: permite comunicação entre front-end e API em origens diferentes;
- prazo de expiração definido por `JWT_EXPIRES_MINUTES`.

Os endpoints administrativos exigem:

```csharp
[Authorize(Roles = "Administrador")]
```

O catálogo VIP exige:

```csharp
[Authorize(Roles = "VIP")]
```

## CORS e consumo pelo front-end

A política CORS chamada `Frontend` permite somente a origem informada por `FRONTEND_URL`, com métodos, headers e credenciais.

No front-end, requisições que dependem do cookie devem usar `credentials: "include"`:

```js
const resposta = await fetch("https://localhost:7103/api/Auth/me", {
  credentials: "include"
});
```

Não combine `AllowAnyOrigin()` com `AllowCredentials()`. Em produção, configure `FRONTEND_URL` com o endereço real do site, sem barra no final.

## Regras de negócio importantes

### Reservas

- uma reserva deve possuir pelo menos um item;
- quantidades devem ser maiores que zero;
- itens repetidos da mesma variação são agrupados;
- todos os itens são validados antes da gravação;
- se qualquer item for inválido, a reserva inteira é rejeitada;
- produto e variação precisam existir e estar ativos;
- o estoque precisa atender à quantidade total solicitada;
- o preço é obtido do banco, nunca recebido do cliente;
- o estoque é reduzido quando a reserva é criada;
- o estoque é devolvido quando uma reserva elegível é cancelada;
- produtos exclusivos exigem um membro VIP autenticado, ativo e dentro da validade;
- o vínculo VIP vem do JWT, não de um ID enviado no corpo;
- cada reserva recebe um código aleatório no formato `RES-...`.

Transições de status permitidas:

```text
Pendente   -> Confirmada ou Cancelada
Confirmada -> Separada ou Cancelada
Separada   -> Retirada ou Cancelada
```

### Produtos e variações

- o catálogo público devolve somente produtos ativos e não exclusivos;
- o catálogo VIP devolve produtos exclusivos ativos;
- somente variações ativas aparecem nos catálogos;
- não pode existir mais de uma variação com o mesmo tamanho para o mesmo produto;
- tamanho é normalizado para letras maiúsculas;
- preço e estoque são controlados no backend.

### Membros VIP

- e-mail e identificação são normalizados e não podem se repetir;
- membros ativos precisam possuir validade futura;
- senhas são armazenadas somente como hash BCrypt;
- o login falha para membro inexistente, inativo, vencido ou com senha incorreta.

### Administradores

- e-mails são normalizados e não podem se repetir;
- senhas possuem hash BCrypt;
- o último administrador ativo não pode ser desativado;
- somente administradores autenticados podem gerenciar outros administradores.

### Eventos

O endpoint público de próximos eventos devolve somente eventos ativos, futuros e com status `Agendado`.

## Endpoints

Salvo quando indicado, as rotas abaixo usam a base:

```text
https://localhost:7103/api
```

Legenda:

- **Público**: não exige autenticação;
- **Admin**: exige role `Administrador`;
- **VIP**: exige role `VIP`.

### Autenticação administrativa — `/Auth`

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| `POST` | `/Auth/login` | Público | Autentica um administrador e cria o cookie JWT |
| `POST` | `/Auth/logout` | Autenticado | Remove o cookie de autenticação |
| `GET` | `/Auth/me` | Autenticado | Retorna as claims do usuário autenticado |

### Autenticação VIP — fora do prefixo `/api`

| Método | URL completa | Acesso | Descrição |
|---|---|---|---|
| `POST` | `https://localhost:7103/VipAuth/login` | Público | Autentica um membro VIP e cria o cookie JWT |

### Administradores — `/Administrador`

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| `GET` | `/Administrador` | Admin | Lista os administradores |
| `GET` | `/Administrador/{id}` | Admin | Busca um administrador |
| `POST` | `/Administrador` | Admin | Cadastra um administrador |
| `PUT` | `/Administrador/{id}` | Admin | Atualiza dados, senha opcional e estado ativo |

### Eventos — `/Evento`

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| `GET` | `/Evento/proximos` | Público | Lista eventos ativos, agendados e futuros |
| `GET` | `/Evento` | Admin | Lista todos os eventos |
| `GET` | `/Evento/{id}` | Admin | Busca um evento |
| `POST` | `/Evento` | Admin | Cadastra um evento |
| `PUT` | `/Evento/{id}` | Admin | Atualiza um evento |
| `DELETE` | `/Evento/{id}` | Admin | Exclui um evento |

Status: `1` Agendado, `2` Em andamento, `3` Concluído e `4` Cancelado.

### Publicações — `/Publicacao`

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| `GET` | `/Publicacao/destaques` | Público | Lista publicações ativas em destaque |
| `GET` | `/Publicacao` | Admin | Lista todas as publicações |
| `GET` | `/Publicacao/{id}` | Admin | Busca uma publicação |
| `POST` | `/Publicacao` | Admin | Cadastra uma publicação |
| `PUT` | `/Publicacao/{id}` | Admin | Atualiza uma publicação |
| `DELETE` | `/Publicacao/{id}` | Admin | Exclui uma publicação |

### Membros da comissão — `/MembroComissao`

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| `GET` | `/MembroComissao/ordenados` | Público | Lista membros ativos na ordem de exibição |
| `GET` | `/MembroComissao` | Admin | Lista todos os membros |
| `GET` | `/MembroComissao/{id}` | Admin | Busca um membro |
| `POST` | `/MembroComissao` | Admin | Cadastra um membro |
| `PUT` | `/MembroComissao/{id}` | Admin | Atualiza um membro |
| `DELETE` | `/MembroComissao/{id}` | Admin | Exclui um membro |

### Membros VIP — `/MembroVip`

Todos os endpoints desta seção exigem role `Administrador`.

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/MembroVip` | Lista todos os membros VIP |
| `GET` | `/MembroVip/{id}` | Busca por ID |
| `GET` | `/MembroVip/email?email={email}` | Busca por e-mail |
| `GET` | `/MembroVip/identificacao?numeroIdentificacao={numero}` | Busca por identificação |
| `POST` | `/MembroVip` | Cadastra um membro VIP |
| `PUT` | `/MembroVip/{id}` | Atualiza um membro VIP |
| `DELETE` | `/MembroVip/{id}` | Exclui um membro VIP |

### Produtos — `/Produto`

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| `GET` | `/Produto/catalogo` | Público | Catálogo de produtos não exclusivos ativos |
| `GET` | `/Produto/vip` | VIP | Catálogo de produtos VIP ativos |
| `GET` | `/Produto` | Admin | Lista todos os produtos |
| `GET` | `/Produto/categoria?categoria={categoria}` | Admin | Filtra por categoria |
| `GET` | `/Produto/{id}` | Admin | Busca um produto |
| `POST` | `/Produto` | Admin | Cadastra um produto |
| `PUT` | `/Produto/{id}` | Admin | Atualiza um produto |
| `PATCH` | `/Produto/{id}/status` | Admin | Ativa ou desativa um produto |
| `DELETE` | `/Produto/{id}` | Admin | Exclui um produto |

### Variações de produto — `/VariacaoProduto`

Todos os endpoints desta seção exigem role `Administrador`.

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/VariacaoProduto` | Lista todas as variações |
| `GET` | `/VariacaoProduto/produto/{produtoId}` | Lista variações de um produto |
| `GET` | `/VariacaoProduto/{id}` | Busca uma variação |
| `POST` | `/VariacaoProduto` | Cadastra uma variação |
| `PUT` | `/VariacaoProduto/{id}` | Atualiza uma variação |
| `PATCH` | `/VariacaoProduto/{id}/estoque` | Atualiza o estoque |
| `DELETE` | `/VariacaoProduto/{id}` | Exclui uma variação |

### Reservas — `/Reserva`

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| `POST` | `/Reserva` | Público ou VIP | Cria uma reserva; associa o VIP autenticado quando houver |
| `GET` | `/Reserva/codigo?codigoReserva={codigo}` | Público | Consulta segura pelo código |
| `GET` | `/Reserva` | Admin | Lista todas as reservas e dados administrativos |
| `GET` | `/Reserva/{id}` | Admin | Busca uma reserva pelo ID |
| `PATCH` | `/Reserva/{id}/status` | Admin | Altera o status conforme as transições permitidas |

Status: `1` Pendente, `2` Confirmada, `3` Separada, `4` Retirada e `5` Cancelada.

## Exemplos de requisições

### Login administrativo

```http
POST /api/Auth/login
Content-Type: application/json

{
  "email": "admin@exemplo.com",
  "senha": "uma-senha-forte"
}
```

### Login VIP

```http
POST /VipAuth/login
Content-Type: application/json

{
  "numeroIdentificacao": "VIP-001",
  "senha": "senhaSegura"
}
```

### Criar administrador

```json
{
  "nome": "Administrador",
  "email": "novo-admin@exemplo.com",
  "senha": "senhaComOitoCaracteres"
}
```

### Criar membro VIP

```json
{
  "numeroIdentificacao": "VIP-001",
  "nome": "João Silva",
  "email": "joao@exemplo.com",
  "telefone": "11999999999",
  "senha": "senhaSegura",
  "dataValidade": "2027-12-31T23:59:59Z"
}
```

### Criar produto

```json
{
  "nome": "Camiseta da turma",
  "descricao": "Camiseta oficial",
  "preco": 59.90,
  "imagemUrl": "https://exemplo.com/camiseta.jpg",
  "categoria": "Vestuário",
  "exclusivoVip": false
}
```

### Criar variação

```json
{
  "produtoId": 1,
  "tamanho": "M",
  "quantidadeDisponivel": 20
}
```

### Criar reserva

O cliente não envia preço nem `membroVipId`:

```json
{
  "nomeCliente": "Maria Oliveira",
  "contato": "11988888888",
  "observacoes": "Retirada no período da tarde",
  "itens": [
    {
      "variacaoProdutoId": 1,
      "quantidade": 2
    }
  ]
}
```

### Atualizar status da reserva

```json
{
  "status": 2
}
```

## Respostas HTTP e middleware

| Código | Situação |
|---:|---|
| `200 OK` | Consulta, login ou atualização com resposta |
| `201 Created` | Recurso criado |
| `204 No Content` | Atualização, exclusão ou logout concluído |
| `400 Bad Request` | DTO inválido ou regra de negócio não atendida |
| `401 Unauthorized` | Login inválido, token ausente ou identidade inválida |
| `403 Forbidden` | Usuário autenticado sem a role necessária |
| `404 Not Found` | Recurso não encontrado |
| `500 Internal Server Error` | Falha inesperada ou configuração interna incorreta |

O `ExceptionMiddleware` trata globalmente:

```text
RegraNegocioException      -> 400
UnauthorizedAccessException -> 401
KeyNotFoundException       -> 404
Exception não esperada     -> 500
```

Erros 500 recebem uma mensagem genérica, evitando expor detalhes internos. As violações esperadas de regra de negócio devolvem uma mensagem própria para o front-end apresentar ao usuário.

## Estrutura do projeto

```text
ApiOdonto/
|-- Controllers/       endpoints HTTP e autorização
|-- Data/              AppDbContext e inicialização do banco
|-- DTOs/              contratos de entrada e saída
|-- Enums/             status de eventos e reservas
|-- Exceptions/        exceções de regras de negócio
|-- Middlewares/       tratamento global de erros
|-- Migrations/        estrutura versionada do PostgreSQL
|-- Models/            entidades persistidas
|-- Repositories/      acesso ao banco
|-- Services/          regras de negócio
|-- Program.cs         serviços e pipeline HTTP
|-- appsettings.json   connection string e configurações gerais
|-- .env.example       modelo das variáveis de ambiente
|-- Dockerfile         build e runtime .NET 10 no Render
|-- .dockerignore      exclusões do contexto Docker
`-- ApiOdonto.csproj   framework e dependências
```

## Deploy com Supabase e Render

### Supabase

Crie um projeto PostgreSQL e selecione `Connect > Session pooler > .NET`. O formato
de produção é equivalente a:

```text
Host=<pooler>;Port=5432;Database=postgres;Username=postgres.<project-ref>;Password=<senha>;SSL Mode=Require;Timeout=30
```

Nunca envie essa string ao GitHub ou ao front-end.

### Render

Crie um `Web Service` com:

```text
Language: Docker
Root Directory: back-end/ApiOdonto
Dockerfile Path: ./Dockerfile
Health Check Path: /health
```

Variáveis necessárias:

```env
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=<connection-string-do-supabase>

JWT_KEY=<chave-secreta-longa>
JWT_ISSUER=ApiOdonto
JWT_AUDIENCE=OdontoComissaoFrontend
JWT_EXPIRES_MINUTES=120

ADMIN_INICIAL_NOME=<nome>
ADMIN_INICIAL_EMAIL=<email>
ADMIN_INICIAL_SENHA=<senha>

FRONTEND_URL=https://seu-projeto.vercel.app
```

Na inicialização, `DbInitializer` aplica migrations pendentes e cria o primeiro
administrador quando o banco ainda não possui nenhum. O endpoint público `/health`
retorna `200 OK` para a verificação do Render.

O Render encerra o HTTPS no proxy e encaminha HTTP para a porta `10000` do container.
Por isso, `UseHttpsRedirection()` é usado somente no ambiente local de desenvolvimento.

### Validação

```powershell
dotnet restore
dotnet build --configuration Release
dotnet ef migrations script --configuration Release
```

Depois do deploy, verifique:

```text
https://seu-servico.onrender.com/health
```

## Observações

- o projeto não processa pagamentos; a reserva serve para separar produtos;
- datas são armazenadas e comparadas em UTC no backend;
- o banco local é voltado ao desenvolvimento; a publicação pode usar uma instância PostgreSQL do Supabase;
- o front-end deverá respeitar os perfis público, VIP e administrador e enviar cookies com `credentials: "include"`.

---

Desenvolvido por Miguel Amores Ramos.
