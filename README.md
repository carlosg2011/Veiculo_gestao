# Vehicle Guard — API

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-9.0-512BD4?logo=dotnet&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-8-4479A1?logo=mysql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-ready-2496ED?logo=docker&logoColor=white)
![JWT](https://img.shields.io/badge/Auth-JWT-F7B93E?logo=jsonwebtokens&logoColor=black)

API REST do sistema **Vehicle Guard** — plataforma de gestão de veículos, propostas de seguro, vistorias e termos de adesão.

---

## Sobre o projeto

A VehicleGuard API fornece os endpoints consumidos pelo [frontend React](../../vehicle_guard_front/). A arquitetura é organizada em camadas (Controllers → Services → Repositório via EF Core) com autenticação JWT e controle de acesso por roles (`Admin` / `User`).

---

## Tecnologias

| Pacote | Versão | Finalidade |
|---|---|---|
| ASP.NET Core | 9.0 | Framework web |
| Entity Framework Core | 9.0.5 | ORM e migrations |
| Pomelo.EntityFrameworkCore.MySql | 9.0.0 | Provider MySQL |
| BCrypt.Net-Next | 4.1.0 | Hash de senhas |
| Microsoft.AspNetCore.Authentication.JwtBearer | 9.0.5 | Autenticação JWT |
| Swashbuckle.AspNetCore | 8 | Swagger / OpenAPI |

---

## Pré-requisitos

- **.NET SDK 9.0+**
- **MySQL 8+** (local ou remoto)
- Docker (opcional, para execução em container)

---

## Instalação e execução local

```bash
# Clone o repositório
git clone https://github.com/carlosg2011/Veiculo_gestao.git
cd Veiculo_gestao

# Restaure as dependências
dotnet restore

# Configure as variáveis de ambiente (veja seção abaixo)
# Aplique as migrations
dotnet ef database update

# Inicie a API
dotnet run
```

A API estará disponível em `http://localhost:5128` e o Swagger em `http://localhost:5128/swagger`.

---

## Configuração

### appsettings.json

O arquivo `appsettings.json` contém apenas as chaves (sem valores sensíveis). Os valores reais devem ser fornecidos via `appsettings.Development.json` (local) ou variáveis de ambiente (produção).

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "Jwt": {
    "Key": "",
    "Issuer": "VeiculoGuard",
    "Audience": "VeiculoGuardUsers",
    "ExpirationHours": 8
  }
}
```

### Variáveis de ambiente (produção / Render)

| Variável | Descrição |
|---|---|
| `ConnectionStrings__DefaultConnection` | String de conexão MySQL completa |
| `Jwt__Key` | Chave secreta para assinar os tokens JWT (mínimo 32 chars) |
| `Jwt__Issuer` | Emissor do token (padrão: `VeiculoGuard`) |
| `Jwt__Audience` | Audiência do token (padrão: `VeiculoGuardUsers`) |
| `Jwt__ExpirationHours` | Tempo de expiração em horas (padrão: `8`) |
| `ASPNETCORE_ENVIRONMENT` | `Production` em produção |

### Exemplo de string de conexão MySQL

```
Server=<host>;Port=3306;Database=vehicleguard;User=<user>;Password=<senha>;
```

---

## Docker

```bash
# Build da imagem
docker build -t vehicleguard-api .

# Execução
docker run -p 8080:8080 \
  -e ConnectionStrings__DefaultConnection="..." \
  -e Jwt__Key="..." \
  vehicleguard-api
```

A imagem usa build multi-stage (.NET SDK → ASP.NET Runtime) e expõe a porta `8080`.

---

## Migrations (Entity Framework Core)

```bash
# Criar nova migration
dotnet ef migrations add <NomeDaMigration>

# Aplicar ao banco
dotnet ef database update

# Reverter uma migration
dotnet ef database update <MigrationAnterior>
```

### Histórico de migrations

| Migration | Descrição |
|---|---|
| `20260515162340_InitialCreate` | Schema inicial: usuários, veículos, proprietários, propostas, vistorias, termos |
| `20260518172457_AddRoleToUsuario` | Adiciona coluna `Role` na tabela de usuários |
| `20260519162021_AddStatusEnums` | Adiciona enums de status para todas as entidades |
| `20260520000001_VistoriaRandomId` | Gera IDs aleatórios para vistorias |

---

## Referência da API

Todas as rotas (exceto `/api/auth/login`) exigem o header:

```
Authorization: Bearer <token>
```

### Autenticação

| Método | Rota | Descrição |
|---|---|---|
| POST | `/api/auth/login` | Retorna JWT. Body: `{ "email": "", "senha": "" }` |

### Dashboard

| Método | Rota | Query params | Descrição |
|---|---|---|---|
| GET | `/api/dashboard` | `userId` (opcional) | Contadores de veículos, propostas, vistorias e termos do usuário (ou global se omitido) |

### Usuários

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/user` | Lista usuários paginados |
| GET | `/api/user/{id}` | Busca usuário por ID |
| POST | `/api/user` | Cria usuário |
| PUT | `/api/user/{id}` | Atualiza usuário |
| DELETE | `/api/user/{id}` | Remove usuário |

### Proprietários

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/proprietario` | Lista proprietários paginados |
| GET | `/api/proprietario/{id}` | Busca por ID |
| GET | `/api/proprietario/cpf/{cpf}` | Busca por CPF |
| POST | `/api/proprietario` | Cria proprietário |
| PUT | `/api/proprietario/{id}` | Atualiza proprietário |
| DELETE | `/api/proprietario/{id}` | Remove proprietário |

### Veículos

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/veiculo` | Lista veículos paginados |
| GET | `/api/veiculo/{id}` | Busca por ID |
| POST | `/api/veiculo` | Cria veículo |
| PUT | `/api/veiculo/{id}` | Atualiza veículo |
| DELETE | `/api/veiculo/{id}` | Remove veículo |

### Propostas

| Método | Rota | Query params | Descrição |
|---|---|---|---|
| GET | `/api/proposta` | `page`, `pageSize`, `userId` | Lista propostas paginadas (filtra por usuário se `userId` informado) |
| GET | `/api/proposta/busca` | `page`, `pageSize`, + filtros | Busca com filtros avançados (nome, CPF, placa, status) |
| GET | `/api/proposta/{id}` | — | Busca proposta completa por ID |
| POST | `/api/proposta` | — | Cria proposta |
| PUT | `/api/proposta/{id}` | — | Atualiza proposta |
| DELETE | `/api/proposta/{id}` | — | Remove proposta |

### Vistorias

| Método | Rota | Query params | Descrição |
|---|---|---|---|
| GET | `/api/vistoria` | `page`, `pageSize`, `userId`, `propostaId`, `excludeCancelled` | Lista vistorias paginadas |
| GET | `/api/vistoria/{id}` | — | Busca vistoria por ID |
| POST | `/api/vistoria` | — | Cria vistoria |
| PUT | `/api/vistoria/{id}` | — | Atualiza vistoria (status, datas) |
| DELETE | `/api/vistoria/{id}` | — | Remove vistoria |

### Termos

| Método | Rota | Query params | Descrição |
|---|---|---|---|
| GET | `/api/termos` | `page`, `pageSize`, `userId` | Lista termos paginados |
| GET | `/api/termos/{id}` | — | Busca termo por ID |
| GET | `/api/termos/proposta/{idProposta}` | — | Busca termo vinculado a uma proposta |
| POST | `/api/termos` | — | Cria termo |
| PUT | `/api/termos/{id}` | — | Atualiza termo |
| DELETE | `/api/termos/{id}` | — | Remove termo |

### Parâmetros de paginação

| Parâmetro | Padrão | Limite |
|---|---|---|
| `page` | 1 | — |
| `pageSize` | 10 | 500 |

### Enums de status

**Proposta:** `Pendente` · `EmAnalise` · `Aprovada` · `Recusada` · `Cancelada`

**Vistoria:** `NaoIniciada` · `Pendente` · `Concluida` · `Aprovada` · `Recusada` · `Cancelada` · `Expirada`

**Termo:** `Ativo` · `Assinado` · `Expirado` · `Cancelado`

**Veículo:** `Ativo` · `Inativo` · `Bloqueado`

---

## Estrutura de pastas

```
Veiculo_gestao/
├── Controllers/          # Endpoints HTTP
│   ├── AuthController.cs
│   ├── DashboardController.cs
│   ├── UserController.cs
│   ├── ProprietarioController.cs
│   ├── VeiculoController.cs
│   ├── PropostaController.cs
│   ├── VistoriaController.cs
│   └── TermoController.cs
│
├── Services/             # Lógica de negócio
│   ├── Interfaces/       # Contratos (IPropostaService, etc.)
│   └── Implementations/  # Implementações concretas
│
├── Models/               # Entidades do domínio
│   ├── User.cs
│   ├── Veiculo.cs
│   ├── Proprietario.cs
│   ├── Proposta.cs
│   ├── Vistoria.cs
│   └── Termo.cs
│
├── DTOs/                 # Objetos de transferência de dados
│   ├── Create*.cs        # DTOs de entrada (criação/atualização)
│   ├── Response*.cs      # DTOs de saída
│   ├── PagedResultDto.cs # Wrapper de paginação
│   └── PaginationParams.cs
│
├── Enums/                # Enumerações de status
├── Data/
│   └── AppDbContext.cs   # Contexto EF Core
├── Migrations/           # Histórico de migrations
├── Middleware/
│   └── GlobalExceptionMiddleware.cs
├── Dockerfile
├── appsettings.json
├── appsettings.Development.json   # Não versionado (gitignore)
└── appsettings.Production.json
```

---

## Segurança

- Senhas armazenadas com **BCrypt** (cost factor padrão)
- Autenticação via **JWT Bearer** com expiração configurável
- Rate limiting habilitado na rota de login (`/api/auth/login`)
- Middleware global de tratamento de exceções (evita stack traces na resposta)
- Nullable Reference Types habilitado (`<Nullable>enable</Nullable>`)

---

## Deploy — Render

O projeto está configurado para deploy via Docker no [Render](https://render.com):

1. O Render detecta o `Dockerfile` na raiz do repositório
2. Build multi-stage: compila com SDK 9.0, executa com ASP.NET Runtime 9.0
3. Porta exposta: `8080` (configurada via `ENV ASPNETCORE_URLS=http://+:8080`)
4. Variáveis de ambiente configuradas no painel do Render (Settings → Environment)
5. Auto-deploy ativado: push no branch `master` dispara novo deploy automaticamente

---

## CI/CD — GitHub Actions

O workflow `.github/workflows/master_gestaodeveiculosapi.yml` realiza build e deploy para **Azure Web App** a cada push no branch `main`. O deploy principal de produção é feito via **Render** (Docker).

---

## Integrantes

- Andrey Yan
- Felipe Biver
- Carlos Gabriel - Responsável pela arquitetura da API, implementação dos serviços de negócio, autenticação JWT, integração com Cloudinary e configuração do deploy com Docker no Render.
