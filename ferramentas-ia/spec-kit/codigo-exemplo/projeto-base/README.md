# 🏫 MonitoriaBase — Projeto Base para os Exercícios de spec-kit

Este projeto é o ponto de partida para os exercícios de Spec-Driven Development com spec-kit. Ele representa um sistema simples de gestão de monitoria já funcional — e é nele que você vai aplicar o ciclo SDD para adicionar novas features.

## 🏗️ O que está implementado

| Componente | Descrição |
|---|---|
| `Student` | Modelo com Id, Name, Email |
| `Exercise` | Modelo com Id, Title, Topic, DifficultyLevel |
| `ExerciseAttempt` | Registro de conclusão: StudentId, ExerciseId, CompletedAt, Status |
| `IMonitoriaService` | Interface com 5 operações básicas |
| `MonitoriaService` | Implementação in-memory com exercícios pré-carregados |

### Operações disponíveis

```csharp
Student RegisterStudent(string name, string email)
IReadOnlyList<Student> GetStudents()
IReadOnlyList<Exercise> GetExercises()
ExerciseAttempt MarkExerciseAsCompleted(Guid studentId, Guid exerciseId)
IReadOnlyList<ExerciseAttempt> GetStudentProgress(Guid studentId)
```

## 🚀 Como rodar

```bash
# Executar a aplicação
dotnet run --project src/MonitoriaBase

# Rodar os testes
dotnet test

# Build da solução
dotnet build
```

**Saída esperada do `dotnet run`:**
```
Exercícios disponíveis: 5
  [      Iniciante] Pipeline Básico CI/CD — CI/CD
  [ Intermediário] Pipeline Multi-Stage — CI/CD
  [      Iniciante] Sistema de Logging com Singleton — Design Patterns
  [ Intermediário] E-commerce com Decorator — Design Patterns
  [       Avançado] Sistema de Notificações — Design Patterns

Alice Silva: 2 exercício(s) concluído(s)
Bob Santos: 1 exercício(s) concluído(s)
```

## 📂 Estrutura

```
projeto-base/
├── MonitoriaBase.sln
├── src/
│   └── MonitoriaBase/
│       ├── MonitoriaBase.csproj
│       ├── Program.cs
│       ├── Models/
│       │   ├── Student.cs
│       │   ├── Exercise.cs
│       │   └── ExerciseAttempt.cs
│       └── Services/
│           ├── IMonitoriaService.cs
│           └── MonitoriaService.cs
└── tests/
    └── MonitoriaBase.Tests/
        ├── MonitoriaBase.Tests.csproj
        └── MonitoriaServiceTests.cs
```

## 🎯 O que os exercícios vão adicionar via SDD

Este projeto é **intencionalmente incompleto** — ele tem o suficiente para funcionar, mas deixa espaço para as features que os exercícios de spec-kit vão especificar e implementar:

| Exercício | Feature a adicionar |
|---|---|
| [Exercício 2](../../exercicios/exercicio-02-especificando-uma-feature.md) | Sistema de avaliação e feedback de exercícios |
| [Exercício 3](../../exercicios/exercicio-03-ciclo-completo-sdd.md) | Notificação de alunos com fallback automático de canal |

Em cada exercício, você vai usar spec-kit para **especificar, planejar e implementar** essas features — passando pelo ciclo completo do Spec-Driven Development.
