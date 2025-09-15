-- Criação da tabela de ALUNOS
CREATE TABLE Alunos (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    DataNascimento DATE NOT NULL,
    Cpf NVARCHAR(14) NOT NULL UNIQUE,         -- formato: XXX.XXX.XXX-XX
    Email NVARCHAR(255) NOT NULL UNIQUE,
    SenhaHash NVARCHAR(255) NOT NULL          -- hash da senha
);

-- Criação da tabela de TURMAS
CREATE TABLE Turmas (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Descricao NVARCHAR(500) NULL
);

-- Criação da tabela de MATRICULAS
CREATE TABLE Matriculas (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    AlunoId UNIQUEIDENTIFIER NOT NULL,
    TurmaId UNIQUEIDENTIFIER NOT NULL,
    DataMatricula DATETIME NOT NULL,

    CONSTRAINT FK_Matriculas_Alunos FOREIGN KEY (AlunoId)
        REFERENCES Alunos(Id) ON DELETE CASCADE,

    CONSTRAINT FK_Matriculas_Turmas FOREIGN KEY (TurmaId)
        REFERENCES Turmas(Id) ON DELETE CASCADE,

    CONSTRAINT UQ_Matricula_Aluno_Turma UNIQUE (AlunoId, TurmaId)
);

-- Criação da tabela de USUARIOS
CREATE TABLE Usuarios (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    SenhaHash NVARCHAR(255) NOT NULL,
    Role TINYINT NOT NULL
);

-- Inserção de um usuário administrador
INSERT INTO Usuarios (Id, Email, SenhaHash, Role)
VALUES (
    NEWID(),
    'admin@fiap.com',
    CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', 'Admin@@2025'), 2),
    1
);