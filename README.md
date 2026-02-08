# Desafio Fullstack

Este projeto é um sistema fullstack para o gerenciamento de estacionamentos:

### Roles
**Admin**
- Gerencia Estacionametos e vagas
- Definie os Preços para Primeira Hora e Adicionais

**Operador**
- Gerencia carros cadastrados
- Realiza sessões para as vagas

---

## 🚀 Tecnologias

### BackEnd
- **Framework:** .NET 9
- **Banco de dados:** SQLite
- **ORM:** Dapper e EntityFramework
- **Arquitetura:** WebAPI com Layered Architecture
  - **Controller**
  - **Service**
  - **Repository**
- **DTOs:** 
  - Entities
  - Request
  - Response
- **Funcionalidades Adicionais:**
  - Correlation ID e TraceId para rastreamento de erros
  - Sistema de login e registro de usuário
  - Autenticação com roles

### FrontEnd
- **Framework:** Angular CLI 19.0.0
- **Node:** 22.13.0
- **Gerenciador de pacotes:** npm 11.6.2
- **Funcionalidades:** Consumo da API, autenticação e controle de acesso baseado em roles

---
## 📝 Execução do projeto:

Sequência de código para a migração gerar o banco de dados:

```
 dotnet ef
 dotnet ef migratrions add
 dotnet ef database update
```
Após o código um arquivo será criado no projeto com o nome: database.db

Ao iniciar o projeto do back end a rota para documentação estará disponível no link:
https://localhost:7030/scalar/v1

Para o front end apenas será necessário executar

```
  npm i 
  ng serve
```

Rotas presentes no Front-end para fácil locomoção:

- "" Home
- "/login" Tela de Login
- "/register" Tela de Registro
- "/dash-operator" Dashboard para o Operador
- "/dash-admin" Dashboard para o Admin
- "/account" Tela das informações pessoais do usuário logado
