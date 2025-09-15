Sistema administrativo para gestão acadêmica desenvolvido com Clean Architecture e Domain-Driven Design, proporcionando alta manutenibilidade, testabilidade e separação de responsabilidades.

## 📋 Funcionalidades
Autenticação JWT com controle de acesso baseado em roles (admin)

CRUD completo de Alunos, Turmas e Matrículas

Validações robustas com FluentValidation

Domínios ricos com Value Objects e Entidades bem definidas

Repositórios específicos para cada agregação

## 🏗️ Arquitetura
O projeto segue os princípios da Clean Architecture com separação em camadas:

**FIAPSecretariaDesafio.API:** Controladores, DTOs e configuração da aplicação

**FIAPSecretariaDesafio.Application:** Serviços de aplicação, interfaces e validadores

**FIAPSecretariaDesafio.Domain:** Entidades, value objects, enums e interfaces de repositório

**FIAPSecretariaDesafio.Infrastructure:** Implementações de repositório com Dapper, segurança e handlers

## 🚀 Tecnologias Utilizadas
.NET 8

Dapper (Micro-ORM para acesso a dados)

JWT (Autenticação por tokens)

FluentValidation (Validação de entradas)

SQL Server (Banco de dados)

## 📦 Instalação e Configuração
**Clone o repositório:**

`` git clone https://github.com/seu-usuario/FIAPSecretariaDesafio.git ``

**Configure a connection string no arquivo appsettings.json:**

``
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=seu-servidor;Database=FIAPSecretaria;Integrated Security=true;"
  }
}
`` 

**Execute o conteúdo do arquivo ``dump`` em sua base de dados.**

**Execute a aplicação.**

## 🔐 Autenticação
O sistema utiliza autenticação JWT com role de administrador. Para acessar os endpoints protegidos, inclua o token no header:


`` Authorization: {seu-token} ``

## 📚 Endpoints Principais
### Autenticação

POST /api/auth/login - Autentica usuários administradores

### Alunos

GET /api/Alunos - Lista todos os alunos (requer autenticação)

POST /api/Alunos - Cria um novo aluno (requer autenticação)

PUT /api/Alunos/{id} - Atualiza um aluno (requer autenticação)

DELETE /api/Alunos/{id} - Remove um aluno (requer autenticação)

GET /api/Alunos/{id} - Busca um aluno por id (requer autenticação)

GET /api/Alunos/{nome} - Busca um aluno por id (requer autenticação)

### Turmas
GET /api/Turmas - Lista todas as turmas (requer autenticação)

POST /api/Turmas - Cria uma nova turma (requer autenticação)

PUT /api/Turmas/{id} - Atualiza uma turmas (requer autenticação)

DELETE /api/Turmas/{id} - Remove uma turmas (requer autenticação)

GET /api/Turmas/{id} - Busca um turmas por id (requer autenticação)

### Matrículas
GET /api/Matriculas - Lista todas as matrículas (requer autenticação)

POST /api/Matriculas - Cria uma nova matrícula (requer autenticação)

PUT /api/Matriculas/{id} - Atualiza uma matrícula (requer autenticação)

DELETE /api/Matriculas/{id} - Remove uma matrícula (requer autenticação)
