# 🦷 Odonto Comissão — API

API REST responsável pelo back-end da aplicação **Odonto Comissão**. Ela centraliza o gerenciamento de eventos, publicações, integrantes da comissão, membros VIP, produtos, variações de estoque e reservas.

O projeto foi construído com **ASP.NET Core 10**, utiliza **Entity Framework Core** para acesso aos dados, **SQLite** como banco de dados e autenticação de administradores por **JWT**.

## Sumário

- [Como a aplicação funciona](#como-a-aplicação-funciona)
- [Tecnologias e pacotes](#tecnologias-e-pacotes)
- [Como executar](#como-executar)
- [Autenticação](#autenticação)
- [Endpoints](#endpoints)
- [Corpos das requisições](#corpos-das-requisições)
- [Status e respostas](#status-e-respostas)
- [Estrutura do projeto](#estrutura-do-projeto)

## Como a aplicação funciona

A API segue uma arquitetura em camadas:

```text
Cliente / Front-end
        │
        ▼
   Controllers      recebem e validam as requisições HTTP
        │
        ▼
    Services        aplicam as regras de negócio
        │
        ▼
  Repositories      realizam o acesso aos dados
        │
        ▼
 Entity Framework Core + SQLite
```

Os dados recebidos e devolvidos pela API são definidos por **DTOs**, evitando expor diretamente as entidades do banco. Erros não tratados passam pelo `ExceptionMiddleware`, que padroniza a resposta enviada ao cliente.

Principais funcionalidades:

- autenticação de administradores;
- cadastro e divulgação de eventos;
- gerenciamento de publicações e destaques;
- apresentação dos membros da comissão;
- cadastro de membros VIP;
- catálogo de produtos com variações e estoque;
- criação e acompanhamento de reservas.

> [!NOTE]
> Atualmente, somente `logout` e `me` possuem o atributo `[Authorize]`. Os demais endpoints estão acessíveis sem autenticação até que regras de autorização sejam adicionadas aos seus controllers.

## Tecnologias e pacotes

### Stack

| Tecnologia | Uso |
|---|---|
| .NET 10 / ASP.NET Core | Plataforma e construção da API REST |
| Entity Framework Core | ORM e persistência dos dados |
| SQLite | Banco de dados local |
| JWT Bearer | Autenticação e validação de tokens |
| Swagger / OpenAPI | Documentação e teste interativo dos endpoints |

### Pacotes NuGet

| Pacote | Versão | Finalidade |
|---|---:|---|
| `BCrypt.Net-Next` | 4.2.0 | Criação e verificação segura de hashes de senha |
| `DotNetEnv` | 3.2.0 | Leitura das variáveis do arquivo `.env` |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 10.0.10 | Autenticação JWT Bearer |
| `Microsoft.AspNetCore.OpenApi` | 10.0.8 | Suporte à especificação OpenAPI |
| `Microsoft.EntityFrameworkCore` | 10.0.10 | ORM e acesso ao banco |
| `Microsoft.EntityFrameworkCore.Design` | 10.0.10 | Recursos de design e migrations do EF Core |
| `Microsoft.EntityFrameworkCore.Sqlite` | 10.0.10 | Provider SQLite para o EF Core |
| `Microsoft.EntityFrameworkCore.Tools` | 10.0.10 | Ferramentas de migrations do EF Core |
| `Swashbuckle.AspNetCore` | 10.2.3 | Geração do Swagger e Swagger UI |

## Como executar

### Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download);
- ferramenta `dotnet-ef`, caso queira criar ou aplicar migrations.

### 1. Entre na pasta da API

```bash
cd back-end/ApiOdonto
```

### 2. Configure as variáveis de ambiente

Copie `.env.example` para um novo arquivo chamado `.env` e preencha a chave JWT:

```env
JWT_KEY=coloque-aqui-uma-chave-secreta-longa-e-segura
JWT_ISSUER=ApiOdonto
JWT_AUDIENCE=OdontoComissaoFrontend
JWT_EXPIRES_MINUTES=120
```

> [!IMPORTANT]
> Não publique o arquivo `.env` nem utilize a chave de exemplo em produção.

### 3. Restaure os pacotes

```bash
dotnet restore
```

### 4. Aplique as migrations

O repositório já possui um banco `ApiOdonto.db`. Para criar ou atualizar o banco a partir das migrations:

```bash
dotnet ef database update
```

Caso a ferramenta ainda não esteja instalada:

```bash
dotnet tool install --global dotnet-ef
```

### 5. Inicie a API

```bash
dotnet run
```

Por padrão, os perfis locais utilizam:

- HTTP: `http://localhost:5255`
- HTTPS: `https://localhost:7103`
- Swagger: `https://localhost:7103/swagger`

O Swagger é habilitado quando `ASPNETCORE_ENVIRONMENT=Development`.

## Autenticação

O login procura um administrador ativo pelo e-mail e compara a senha enviada com o hash BCrypt armazenado no banco. Quando as credenciais são válidas, a API:

1. gera um JWT assinado com `HMAC-SHA256`;
2. devolve o token no corpo da resposta;
3. grava o mesmo token no cookie `access_token`.

O cookie é configurado como `HttpOnly`, `Secure` e `SameSite=None`. Nas rotas protegidas, o middleware JWT lê o token desse cookie.

Ao consumir a API pelo navegador em outra origem, envie credenciais na requisição:

```js
fetch("https://localhost:7103/api/Auth/me", {
  credentials: "include"
});
```

### Exemplo de login

```http
POST /api/Auth/login
Content-Type: application/json

{
  "email": "admin@exemplo.com",
  "senha": "minhaSenha"
}
```

## Endpoints

A URL-base usada nos exemplos é:

```text
https://localhost:7103/api
```

### Autenticação — `/Auth`

| Método | Rota | Protegida | Descrição |
|---|---|:---:|---|
| `POST` | `/Auth/login` | Não | Autentica um administrador e cria o cookie JWT |
| `POST` | `/Auth/logout` | Sim | Remove o cookie de autenticação |
| `GET` | `/Auth/me` | Sim | Retorna os dados do administrador autenticado |

### Eventos — `/Evento`

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/Evento` | Lista todos os eventos |
| `GET` | `/Evento/proximos` | Lista os próximos eventos |
| `GET` | `/Evento/{id}` | Busca um evento pelo ID |
| `POST` | `/Evento` | Cadastra um evento |
| `PUT` | `/Evento/{id}` | Atualiza um evento |
| `DELETE` | `/Evento/{id}` | Exclui um evento |

Status de evento: `1` Agendado, `2` Em andamento, `3` Concluído e `4` Cancelado.

### Membros da comissão — `/MembroComissao`

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/MembroComissao` | Lista todos os membros |
| `GET` | `/MembroComissao/ordenados` | Lista os membros na ordem de exibição |
| `GET` | `/MembroComissao/{id}` | Busca um membro pelo ID |
| `POST` | `/MembroComissao` | Cadastra um membro |
| `PUT` | `/MembroComissao/{id}` | Atualiza um membro |
| `DELETE` | `/MembroComissao/{id}` | Exclui um membro |

### Membros VIP — `/MembroVip`

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/MembroVip` | Lista todos os membros VIP |
| `GET` | `/MembroVip/{id}` | Busca um membro VIP pelo ID |
| `GET` | `/MembroVip/email?email={email}` | Busca pelo e-mail |
| `GET` | `/MembroVip/identificacao?numeroIdentificacao={numero}` | Busca pelo número de identificação |
| `POST` | `/MembroVip` | Cadastra um membro VIP |
| `PUT` | `/MembroVip/{id}` | Atualiza um membro VIP |
| `DELETE` | `/MembroVip/{id}` | Exclui um membro VIP |

### Produtos — `/Produto`

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/Produto` | Lista todos os produtos |
| `GET` | `/Produto/categoria?categoria={categoria}` | Filtra produtos por categoria |
| `GET` | `/Produto/{id}` | Busca um produto pelo ID |
| `POST` | `/Produto` | Cadastra um produto |
| `PUT` | `/Produto/{id}` | Atualiza um produto |
| `PATCH` | `/Produto/{id}/status` | Ativa ou desativa um produto; recebe `true` ou `false` |
| `DELETE` | `/Produto/{id}` | Exclui um produto |

### Variações de produtos — `/VariacaoProduto`

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/VariacaoProduto` | Lista todas as variações |
| `GET` | `/VariacaoProduto/produto/{produtoId}` | Lista as variações de um produto |
| `GET` | `/VariacaoProduto/{id}` | Busca uma variação pelo ID |
| `POST` | `/VariacaoProduto` | Cadastra uma variação |
| `PUT` | `/VariacaoProduto/{id}` | Atualiza uma variação |
| `PATCH` | `/VariacaoProduto/{id}/estoque` | Atualiza a quantidade disponível |
| `DELETE` | `/VariacaoProduto/{id}` | Exclui uma variação |

### Publicações — `/Publicacao`

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/Publicacao` | Lista todas as publicações |
| `GET` | `/Publicacao/destaques` | Lista as publicações em destaque |
| `GET` | `/Publicacao/{id}` | Busca uma publicação pelo ID |
| `POST` | `/Publicacao` | Cadastra uma publicação |
| `PUT` | `/Publicacao/{id}` | Atualiza uma publicação |
| `DELETE` | `/Publicacao/{id}` | Exclui uma publicação |

### Reservas — `/Reserva`

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/Reserva` | Lista todas as reservas |
| `GET` | `/Reserva/{id}` | Busca uma reserva pelo ID |
| `GET` | `/Reserva/codigo?codigoReserva={codigo}` | Busca pelo código da reserva |
| `POST` | `/Reserva` | Cria uma reserva com um ou mais itens |
| `PATCH` | `/Reserva/{id}/status` | Atualiza o status da reserva |

Status de reserva: `1` Pendente, `2` Confirmada, `3` Separada, `4` Retirada e `5` Cancelada.

## Corpos das requisições

Os exemplos abaixo mostram os campos aceitos nas operações de criação. Nas atualizações completas via `PUT`, também é enviado o campo `ativo`.

<details>
<summary><strong>Criar evento</strong></summary>

```json
{
  "titulo": "Baile de formatura",
  "descricao": "Celebração da turma de Odontologia",
  "dataHora": "2027-01-23T20:00:00",
  "local": "Salão Principal",
  "categoria": "Formatura",
  "status": 1,
  "imagemUrl": "https://exemplo.com/evento.jpg"
}
```
</details>

<details>
<summary><strong>Criar membro da comissão</strong></summary>

```json
{
  "nome": "Ana Souza",
  "cargo": "Presidente",
  "descricao": "Responsável pela organização geral",
  "fotoUrl": "https://exemplo.com/ana.jpg",
  "ordemExibicao": 1
}
```
</details>

<details>
<summary><strong>Criar membro VIP</strong></summary>

```json
{
  "numeroIdentificacao": "VIP-001",
  "nome": "João Silva",
  "email": "joao@exemplo.com",
  "telefone": "11999999999",
  "senha": "senhaSegura",
  "dataValidade": "2027-12-31T23:59:59"
}
```
</details>

<details>
<summary><strong>Criar produto</strong></summary>

```json
{
  "nome": "Camiseta da turma",
  "descricao": "Camiseta oficial da comissão",
  "preco": 59.90,
  "imagemUrl": "https://exemplo.com/camiseta.jpg",
  "categoria": "Vestuário",
  "exclusivoVip": false
}
```
</details>

<details>
<summary><strong>Criar variação de produto</strong></summary>

```json
{
  "produtoId": 1,
  "tamanho": "M",
  "quantidadeDisponivel": 20
}
```

Para alterar somente o estoque:

```json
{
  "quantidadeDisponivel": 15
}
```
</details>

<details>
<summary><strong>Criar publicação</strong></summary>

```json
{
  "titulo": "Novidades da comissão",
  "descricao": "Confira as novidades deste mês.",
  "imagemUrl": "https://exemplo.com/publicacao.jpg",
  "tipo": "Notícia",
  "ordemExibicao": 1,
  "destaque": true
}
```
</details>

<details>
<summary><strong>Criar reserva</strong></summary>

```json
{
  "nomeCliente": "Maria Oliveira",
  "contato": "11988888888",
  "membroVipId": null,
  "observacoes": "Retirada no período da tarde",
  "itens": [
    {
      "variacaoProdutoId": 1,
      "quantidade": 2
    }
  ]
}
```

Para alterar somente o status:

```json
{
  "status": 2
}
```
</details>

## Status e respostas

| Código | Significado |
|---:|---|
| `200 OK` | Consulta ou login realizado com sucesso |
| `201 Created` | Recurso criado com sucesso |
| `204 No Content` | Atualização, exclusão ou logout concluído |
| `400 Bad Request` | Dados inválidos ou regra de negócio não atendida |
| `401 Unauthorized` | Credenciais inválidas ou autenticação ausente |
| `404 Not Found` | Recurso não encontrado |
| `500 Internal Server Error` | Erro inesperado no servidor |

As validações dos DTOs verificam campos obrigatórios, formatos de e-mail, tamanhos mínimos, valores não negativos e IDs válidos.

## Estrutura do projeto

```text
ApiOdonto/
├── Authentication/   # Elementos relacionados à autenticação
├── Controllers/      # Endpoints HTTP
├── Data/             # DbContext do Entity Framework
├── DTOs/             # Contratos de entrada e saída da API
├── Enums/            # Status de eventos e reservas
├── Middlewares/      # Tratamento global de exceções
├── Migrations/       # Histórico de alterações do banco
├── Models/           # Entidades persistidas
├── Repositories/     # Acesso e consultas ao banco
├── Services/         # Regras de negócio
├── Program.cs        # Configuração e inicialização da aplicação
├── appsettings.json  # Conexão e configurações gerais
└── ApiOdonto.csproj  # Framework e dependências NuGet
```

---

Desenvolvido por Miguel Amores Ramos
