# Odonto Comissão

Plataforma web para centralizar a comunicação e a operação de uma comissão de turma de Odontologia. O projeto reúne publicações, cronograma, comissão, catálogo de produtos, reservas, área VIP e painel administrativo responsivo.

## Arquitetura

```text
React + Vite (Vercel)
        |
        | HTTPS + cookies HttpOnly
        v
ASP.NET Core 10 (Docker no Render)
        |
        | Npgsql / Session Pooler
        v
PostgreSQL (Supabase)
```

| Camada | Tecnologia | Hospedagem |
|---|---|---|
| Front-end | React 19, TypeScript e Vite 8 | Vercel |
| API | ASP.NET Core 10 e Entity Framework Core | Render, via Docker |
| Banco | PostgreSQL e Npgsql | Supabase |
| Autenticação | JWT em cookie `HttpOnly`, `Secure` e `SameSite=None` | API |

## Funcionalidades

- página inicial com destaques, avisos e próximos eventos;
- cronograma de eventos futuros e anteriores;
- apresentação dos membros da comissão;
- catálogo público e produtos exclusivos para membros VIP;
- reserva de produtos sem pagamento online;
- login, perfil e histórico de reservas VIP;
- painel administrativo de publicações, eventos, comissão, produtos, estoque, reservas, VIPs e administradores;
- layouts públicos, VIP e administrativos responsivos.

## Estrutura

```text
OdontoComissao/
|-- back-end/ApiOdonto/          API ASP.NET Core
|-- front-end/OdontoFront/       aplicação React
`-- README.md                    visão geral
```

Documentação detalhada:

- [API e banco](back-end/ApiOdonto/README.md)
- [Front-end](front-end/OdontoFront/README.md)

## Desenvolvimento local

API:

```powershell
cd back-end/ApiOdonto
dotnet restore
dotnet run
```

Front-end, em outro terminal:

```powershell
cd front-end/OdontoFront
npm ci
npm run dev
```

Endereços locais:

```text
Front-end: http://localhost:5173
API:       https://localhost:7103
Swagger:   https://localhost:7103/swagger
```

O backend exige PostgreSQL e as variáveis descritas em `back-end/ApiOdonto/.env.example`. O frontend usa `VITE_API_URL`, descrita em `front-end/OdontoFront/.env.example`.

## Qualidade

Antes de publicar alterações:

```powershell
dotnet build back-end/ApiOdonto/ApiOdonto.csproj --configuration Release

cd front-end/OdontoFront
npm run lint
npm test
npm run build
```

## Deploy

O fluxo de produção adotado é:

1. PostgreSQL gratuito no Supabase;
2. API Docker no Render;
3. frontend Vite na Vercel;
4. `FRONTEND_URL` no Render apontando para a Vercel;
5. `VITE_API_URL` na Vercel apontando para o Render.

Segredos, senhas, JWT e connection strings são configurados nas plataformas e nunca enviados ao GitHub.

## Autor

Desenvolvido por Miguel Amores Ramos.
