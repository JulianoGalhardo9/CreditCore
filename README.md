### Distributed Credit Analysis Platform · Microservices · Event-Driven Architecture

<br/>

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C# 12](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-MassTransit-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)](https://www.rabbitmq.com/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-EF_Core-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![xUnit](https://img.shields.io/badge/Tests-xUnit-green?style=for-the-badge&logo=testcafe&logoColor=white)](https://xunit.net/)

</div>

---

## 🎯 Visão Geral

**CreditCore** é uma plataforma distribuída de **alta performance** que simula fluxos críticos de análise e concessão de crédito bancário, do zero ao deploy. O projeto demonstra domínio sobre os padrões arquiteturais mais exigidos no mercado financeiro: **CQRS, Event-Driven Architecture, Domain-Driven Design e Database-per-Service**.

> **Por que isso importa?** Sistemas de crédito são dos mais sensíveis em produção — exigem consistência eventual, rastreabilidade total e resiliência a falhas. Este projeto aborda cada um desses desafios com soluções reais.

---

## 🏗️ Arquitetura

┌──────────────────────────────────────────┐
                    │           API GATEWAY (YARP)             │
                    │     Port :5050 · Swagger Aggregation      │
                    └──────────┬────────────┬──────────────────┘
                               │            │
          ┌────────────────────▼──┐    ┌────▼─────────────────┐
          │    IdentityService    │    │    CreditService      │
          │  JWT · BCrypt · IAM   │    │  Loan Lifecycle Core  │
          │    Port :5001         │    │    Port :5002         │
          └───────────────────────┘    └────────┬─────────────┘
                                                │
                                     ┌──────────▼──────────────┐
                                     │    RabbitMQ Message Bus  │
                                     │      (MassTransit)       │
                                     └────┬──────────────┬──────┘
                                          │              │
                         ┌────────────────▼──┐  ┌────────▼──────────────┐
                         │  RuleEngineService │  │     AuditService      │
                         │  Async Risk Eval   │  │  Immutable Audit Log  │
                         │    Port :5003      │  │    Port :5004         │
                         └───────────────────┘  └───────────────────────┘

                 [ SQL Server ]  [ SQL Server ]  [ SQL Server ]  [ SQL Server ]
                 Identity DB     Credit DB        RuleEngine DB   Audit DB
                 (Isolated)      (Isolated)       (Isolated)      (Isolated)

### Decisões arquiteturais

| Decisão | Solução Adotada | Justificativa |
|---|---|---|
| Comunicação assíncrona | RabbitMQ + MassTransit | Desacoplamento entre serviços; resiliência a falhas parciais |
| Isolamento de dados | Database-per-Service | Bounded Contexts independentes; sem acoplamento de schema |
| Roteamento unificado | YARP (API Gateway) | Ponto único de entrada; Swagger agregado |
| Comandos e queries | CQRS com MediatR | Segregação de responsabilidades de leitura/escrita |
| Autenticação | JWT + BCrypt | Stateless; padrão de mercado para APIs distribuídas |

---

## 🧩 Microserviços

### 🔐 IdentityService
Gerencia o ciclo de vida de identidades e sessões. Implementa autenticação **JWT** com refresh tokens e hashing seguro de credenciais via **BCrypt**. Expõe endpoints de registro, login e validação de token.

### 💳 CreditService
O núcleo de negócio da plataforma. Orquestra o ciclo completo de uma proposta de crédito:
`Criada → Em Análise → Aprovada / Rejeitada`
Publica eventos no barramento para consumo assíncrono pelos demais serviços.

### ⚙️ RuleEngineService
Motor de regras que consome eventos de novas propostas e executa análise de risco de forma assíncrona. Avalia critérios configuráveis (score, renda, exposição) e emite o resultado de volta ao barramento.

### 📋 AuditService
Garante **rastreabilidade imutável** de todas as operações críticas. Consome eventos do barramento e persiste logs de auditoria para fins de compliance e observabilidade regulatória.

### 🌐 ApiGateway (YARP)
Ponto de entrada unificado com roteamento inteligente, transformação de paths e agregação de Swagger UI — permitindo navegar pela documentação de todos os serviços em um único portal.

---

## 🛠️ Stack Tecnológica

Backend
├── Runtime         → .NET 9 / C# 12
├── API             → ASP.NET Core Minimal APIs + Controllers
├── ORM             → Entity Framework Core (Code First + Auto-Migrations)
├── Mensageria      → RabbitMQ + MassTransit (Publisher/Subscriber)
├── CQRS            → MediatR (Commands, Queries, Notifications)
├── Auth            → JWT Bearer + BCrypt.Net
└── Gateway         → YARP (Yet Another Reverse Proxy)
Persistência
├── Banco           → SQL Server (instâncias isoladas por serviço)
└── Padrão          → Repository Pattern + Unit of Work
Qualidade
├── Testes          → xUnit + FluentAssertions
├── Test DB         → In-Memory Database (integração)
└── Cobertura       → Domain Tests + Infrastructure Tests
Infraestrutura
├── Containers      → Docker + Docker Compose
├── Networking      → Internal Docker network entre serviços
└── Config          → Options Pattern + Environment Variables

---

## 🔁 Fluxo de Crédito (Event-Driven)

Cliente                 CreditService            RabbitMQ              RuleEngineService
│                          │                      │                         │
│──POST /loans/apply──────▶│                      │                         │
│                          │──Publish(LoanCreated)▶│                         │
│◀── 202 Accepted ─────────│                      │──LoanCreated ──────────▶│
│                          │                      │                         │── Avalia Risco
│                          │                      │◀── LoanEvaluated ────────│
│                          │◀── LoanEvaluated ────│                         │
│                          │── Atualiza Status     │                         │
│                          │──Publish(LoanApproved/Rejected)▶│              │
│                          │                      │──────────────────────▶ AuditService
│                          │                      │                       (Persiste log)

---

## 🧪 Estratégia de Testes

A cobertura de testes protege as duas camadas mais críticas da aplicação:

**Domain Tests** — Validam regras de negócio puras sem dependências externas:
- Invariantes de entidade (`LoanProposal`, `CreditLimit`)
- Transições de estado válidas e inválidas (`Pending → Approved`, `Pending → Rejected`)
- Value Objects e cálculos financeiros

**Infrastructure Tests** — Garantem integridade da persistência:
- Repositórios com banco In-Memory (EF Core)
- Mapeamentos ORM e comportamento de queries
- Consistência de migrations
```bash
# Executar todos os testes
dotnet test

# Com relatório detalhado
dotnet test --logger "console;verbosity=detailed"
```

---

## 🚀 Getting Started

### Pré-requisitos
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) 4.x+
- [Git](https://git-scm.com/)

### Setup em 3 comandos
```bash
# 1. Clone o repositório
git clone https://github.com/seu-usuario/CreditCore.git && cd CreditCore

# 2. Suba todo o ecossistema (build + migrations automáticas incluídas)
docker compose up -d --build

# 3. Acesse o portal de documentação unificado
open http://localhost:5050
```

> O projeto utiliza **Auto-Migrations** — o schema do banco é provisionado automaticamente no startup de cada serviço. Zero configuração manual.

### Endpoints principais

| Serviço | Base URL | Descrição |
|---|---|---|
| API Gateway / Swagger Hub | `http://localhost:5050` | Documentação unificada |
| IdentityService | `http://localhost:5001` | Auth endpoints |
| CreditService | `http://localhost:5002` | Loan management |
| RuleEngineService | `http://localhost:5003` | Risk evaluation |
| AuditService | `http://localhost:5004` | Audit logs |
| RabbitMQ Management | `http://localhost:15672` | Message broker UI |

---

## 📐 Princípios e Padrões Aplicados

| Princípio | Aplicação no Projeto |
|---|---|
| **Single Responsibility** | Cada microserviço possui uma única responsabilidade de domínio |
| **Bounded Contexts (DDD)** | Cada serviço é "dono" do seu schema e modelo de domínio |
| **Eventual Consistency** | Fluxo de aprovação assíncrono via eventos no barramento |
| **Fail Fast** | Resiliência configurada para falhas transitórias em SQL e mensageria |
| **Clean Architecture** | Separação estrita entre Domain, Application e Infrastructure |
| **Immutable Audit Log** | Eventos de auditoria são append-only, nunca alterados |

---

## 📁 Estrutura do Repositório

CreditCore/
├── src/
│   ├── ApiGateway/              # YARP reverse proxy + Swagger aggregation
│   ├── IdentityService/         # Authentication & authorization
│   │   ├── Domain/
│   │   ├── Application/         # CQRS handlers (MediatR)
│   │   └── Infrastructure/      # EF Core + SQL Server
│   ├── CreditService/           # Core business logic
│   │   ├── Domain/              # Entities, Value Objects, Domain Events
│   │   ├── Application/         # Use cases, CQRS, Event publishing
│   │   └── Infrastructure/      # Persistence + MassTransit consumers
│   ├── RuleEngineService/       # Async risk evaluation engine
│   └── AuditService/            # Immutable compliance log
├── tests/
│   ├── CreditService.Domain.Tests/
│   └── CreditService.Infrastructure.Tests/
├── docker-compose.yml
└── README.md

---

## 👨‍💻 Autor

<div align="center">

**Juliano Galhardo de Oliveira**

*Software Engineer · Financial Systems & Distributed Architecture*

[![LinkedIn](https://img.shields.io/badge/LinkedIn-Connect-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://linkedin.com/in/julianogalhardo9)
[![GitHub](https://img.shields.io/badge/GitHub-Portfolio-181717?style=for-the-badge&logo=github&logoColor=white)](https://github.com/JulianoGalhardo9)

</div>

---

<div align="center">

*Built with precision for high-stakes financial systems.*

</div>
