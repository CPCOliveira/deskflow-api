# DeskFlow API

API RESTful para gerenciamento de chamados de suporte técnico (helpdesk), desenvolvida como projeto final do Módulo 01 do curso de Desenvolvimento de Software (SENAI/SCTEC).

Permite abrir, acompanhar e encerrar chamados de suporte, categorizá-los, e registrar o histórico de interações (atendimentos) de cada chamado.

## Tecnologias utilizadas

- C# / .NET Core 10
- Entity Framework Core 10
- SQL Server (SQL Server Express)
- Swagger / Swashbuckle (documentação e testes da API)

## Arquitetura

O projeto segue uma arquitetura em camadas:

```
Controllers/    -> recebem as requisições HTTP e retornam as respostas
Services/       -> regras de negócio e validações
Repositories/   -> acesso a dados (Entity Framework Core)
Models/
  Entities/     -> entidades do banco de dados
  Requests/     -> DTOs usados para receber dados do cliente
Data/           -> DbContext e configuração do EF Core
```

Fluxo de uma requisição: `Controller → Service → Repository → AppDbContext → SQL Server`

## Modelo de dados

**Categoria** - classifica os chamados (ex: Hardware, Software, Rede)
- `Id`, `Nome`

**Chamado** - representa um chamado de suporte aberto por um solicitante
- `Id`, `Titulo`, `Descricao`, `SolicitanteNome`, `Prioridade` (Baixa=0, Media=1, Alta=2), `Status` (Aberto=0, EmAndamento=1, Fechado=2), `CategoriaId`, `DataAbertura`, `DataFechamento`, `Solucao`

**Interacao** - registra o histórico de atendimento de um chamado
- `Id`, `ChamadoId`, `Autor`, `Mensagem`, `DataRegistro`

Relacionamentos:
- `Categoria 1:N Chamado` — uma categoria não pode ser excluída enquanto houver chamados vinculados a ela (`DeleteBehavior.Restrict`)
- `Chamado 1:N Interacao` — ao excluir um chamado, suas interações são excluídas junto (`DeleteBehavior.Cascade`)

## Como executar o projeto

### Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- SQL Server (local ou SQL Server Express)

### Passos

1. Clone o repositório:
```bash
git clone https://github.com/CPCOliveira/deskflow-api.git
cd deskflow-api
```

2. Configure a string de conexão em `appsettings.json` (ajuste `Server` para sua instância do SQL Server):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=DeskFlowDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

3. Aplique as migrations para criar o banco de dados:
```bash
dotnet ef database update
```

4. Rode a aplicação:
```bash
dotnet run
```

5. Acesse a documentação interativa (Swagger) no navegador:
```
http://localhost:5297/swagger
```

## Autenticação

A API usa **ASP.NET Core Identity + JWT**. Todos os endpoints abaixo, exceto os de autenticação, exigem um token válido no header `Authorization`.

1. Registre um usuário:
```http
POST /api/auth/registrar
Content-Type: application/json

{
  "email": "usuario@exemplo.com",
  "senha": "SenhaForte123"
}
```

2. Faça login para obter o token:
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@exemplo.com",
  "senha": "SenhaForte123"
}
```
Resposta:
```json
{ "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." }
```

3. Envie o token nas próximas requisições:
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

Pelo Swagger (`/swagger`), basta clicar no botão **Authorize** no topo da página e colar o token — não precisa prefixar com `Bearer`, o Swagger faz isso automaticamente.

## Endpoints da API

### Autenticação

| Método | Rota | Descrição |
|---|---|---|
| POST | `/api/auth/registrar` | Cadastra um novo usuário |
| POST | `/api/auth/login` | Autentica e retorna um token JWT |

### Categorias

> A partir daqui, todas as rotas exigem o header `Authorization: Bearer {token}` (ver seção [Autenticação](#autenticação)).

| Método | Rota | Descrição |
|---|---|---|
| POST | `/api/categorias` | Cadastra uma nova categoria |
| GET | `/api/categorias` | Lista todas as categorias |
| GET | `/api/categorias/{id}` | Busca uma categoria por id |
| PUT | `/api/categorias/{id}` | Atualiza o nome de uma categoria |
| DELETE | `/api/categorias/{id}` | Exclui uma categoria (bloqueado se houver chamados vinculados) |

### Chamados

| Método | Rota | Descrição |
|---|---|---|
| POST | `/api/chamados` | Abre um novo chamado |
| GET | `/api/chamados` | Lista os chamados, com filtros opcionais por `status`, `prioridade` e `categoriaId` |
| GET | `/api/chamados/{id}` | Busca um chamado por id (inclui categoria e interações) |
| POST | `/api/chamados/{id}/iniciar` | Inicia o atendimento de um chamado (Aberto → EmAndamento) |
| POST | `/api/chamados/{id}/encerrar` | Encerra um chamado, exigindo uma solução (→ Fechado) |

### Interações

| Método | Rota | Descrição |
|---|---|---|
| POST | `/api/chamados/{chamadoId}/interacoes` | Registra uma nova interação em um chamado |
| GET | `/api/chamados/{chamadoId}/interacoes` | Lista as interações de um chamado |

## Autor

Caio Oliveira - [GitHub](https://github.com/CPCOliveira) · [LinkedIn](https://linkedin.com/in/cpcoliveira)
