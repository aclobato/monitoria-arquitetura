# 🚀 Exercício 1 - Sistema de Logging

## 🎯 Objetivo

Implementar um sistema de logging flexível que permita diferentes destinos (arquivo, console, banco de dados) usando os padrões **Singleton** e **Factory Method**.

## 📚 Padrões a Aplicar

- **🏗️ Singleton**: Para gerenciar uma instância única do logger
- **🏭 Factory Method**: Para criar diferentes tipos de loggers
- **📝 Thread-Safety**: Para garantir segurança em aplicações multi-thread

## ⏱️ Tempo Estimado
**45-60 minutos**

---

## 📋 Requisitos

### ✅ Funcionais
1. **Sistema deve suportar múltiplos níveis de log**: DEBUG, INFO, WARNING, ERROR
2. **Múltiplos destinos**: Console, Arquivo, "Banco de Dados" (simulado)
3. **Configuração dinâmica**: Escolher o tipo de logger em runtime
4. **Thread-safe**: Funcionar corretamente em aplicações multi-thread
5. **Formatação**: Incluir timestamp, nível e mensagem

### 🏗️ Não Funcionais
- **Performance**: Operações de log devem ser rápidas
- **Flexibilidade**: Fácil adicionar novos tipos de logger
- **Segurança**: Thread-safe para uso concurrent

---

## 🏛️ Arquitetura Proposta

```
LogManager (Singleton)
    ├── ILogger (Interface)
    ├── LoggerFactory (Factory Method)
    │   ├── CreateConsoleLogger()
    │   ├── CreateFileLogger()
    │   └── CreateDatabaseLogger()
    └── Implementações
        ├── ConsoleLogger
        ├── FileLogger
        └── DatabaseLogger
```

---

## 💻 Implementação Passo a Passo

### 🔧 **Passo 1: Estrutura do Projeto**

Crie a seguinte estrutura:
```
Exercicio01/
├── src/
│   ├── Enums/
│   │   └── LogLevel.cs
│   ├── Interfaces/
│   │   └── ILogger.cs
│   ├── Loggers/
│   │   ├── ConsoleLogger.cs
│   │   ├── FileLogger.cs
│   │   └── DatabaseLogger.cs
│   ├── Factories/
│   │   └── LoggerFactory.cs
│   ├── Managers/
│   │   └── LogManager.cs
│   └── Program.cs
├── tests/
│   └── LoggingTests.cs
└── Exercicio01.csproj
```

### 📝 **Passo 2: Definir Enum de Níveis**

```csharp
// src/Enums/LogLevel.cs
namespace Exercicio01.Enums
{
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }
}
```

### 🎯 **Passo 3: Interface do Logger**

```csharp
// src/Interfaces/ILogger.cs
using Exercicio01.Enums;

namespace Exercicio01.Interfaces
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);
        void Debug(string message);
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }
}
```

### 🏭 **Passo 4: Implementar Factory Method**

```csharp
// src/Factories/LoggerFactory.cs
using Exercicio01.Interfaces;
using Exercicio01.Loggers;

namespace Exercicio01.Factories
{
    public abstract class LoggerFactory
    {
        public abstract ILogger CreateLogger();
        
        // Método template que usa o factory method
        public void LogMessage(string message)
        {
            var logger = CreateLogger();
            logger.Info(message);
        }
    }
    
    public class ConsoleLoggerFactory : LoggerFactory
    {
        public override ILogger CreateLogger()
            => new ConsoleLogger();
    }
    
    public class FileLoggerFactory : LoggerFactory
    {
        private readonly string _filePath;
        
        public FileLoggerFactory(string filePath)
        {
            _filePath = filePath;
        }
        
        public override ILogger CreateLogger()
            => new FileLogger(_filePath);
    }
    
    public class DatabaseLoggerFactory : LoggerFactory
    {
        private readonly string _connectionString;
        
        public DatabaseLoggerFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public override ILogger CreateLogger()
            => new DatabaseLogger(_connectionString);
    }
}
```

### 🖥️ **Passo 5: Implementar ConsoleLogger**

```csharp
// src/Loggers/ConsoleLogger.cs
using Exercicio01.Enums;
using Exercicio01.Interfaces;

namespace Exercicio01.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var levelStr = level.ToString().ToUpper();
            
            // Colorir baseado no nível
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = GetColorForLevel(level);
            
            Console.WriteLine($"[{timestamp}] [{levelStr}] {message}");
            
            Console.ForegroundColor = originalColor;
        }
        
        public void Debug(string message) => Log(LogLevel.Debug, message);
        public void Info(string message) => Log(LogLevel.Info, message);
        public void Warning(string message) => Log(LogLevel.Warning, message);
        public void Error(string message) => Log(LogLevel.Error, message);
        
        private static ConsoleColor GetColorForLevel(LogLevel level)
        {
            return level switch
            {
                LogLevel.Debug => ConsoleColor.Gray,
                LogLevel.Info => ConsoleColor.White,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                _ => ConsoleColor.White
            };
        }
    }
}
```

### 📁 **Passo 6: Implementar FileLogger**

```csharp
// src/Loggers/FileLogger.cs
using Exercicio01.Enums;
using Exercicio01.Interfaces;

namespace Exercicio01.Loggers
{
    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        private readonly object _lockObject = new object();
        
        public FileLogger(string filePath)
        {
            _filePath = filePath;
            // Criar diretório se não existir
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        
        public void Log(LogLevel level, string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var levelStr = level.ToString().ToUpper();
            var logEntry = $"[{timestamp}] [{levelStr}] {message}";
            
            // Thread-safe escrita no arquivo
            lock (_lockObject)
            {
                File.AppendAllText(_filePath, logEntry + Environment.NewLine);
            }
        }
        
        public void Debug(string message) => Log(LogLevel.Debug, message);
        public void Info(string message) => Log(LogLevel.Info, message);
        public void Warning(string message) => Log(LogLevel.Warning, message);
        public void Error(string message) => Log(LogLevel.Error, message);
    }
}
```

### 💾 **Passo 7: Implementar DatabaseLogger (Simulado)**

```csharp
// src/Loggers/DatabaseLogger.cs
using Exercicio01.Enums;
using Exercicio01.Interfaces;

namespace Exercicio01.Loggers
{
    public class DatabaseLogger : ILogger
    {
        private readonly string _connectionString;
        private readonly List<LogEntry> _database = new(); // Simula banco de dados
        private readonly object _lockObject = new object();
        
        public DatabaseLogger(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public void Log(LogLevel level, string message)
        {
            var entry = new LogEntry
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.Now,
                Level = level,
                Message = message
            };
            
            // Thread-safe \"inserção\" no banco
            lock (_lockObject)
            {
                _database.Add(entry);
                Console.WriteLine($\"[DB] Inserido log #{entry.Id}: [{level}] {message}\");
            }
        }
        
        public void Debug(string message) => Log(LogLevel.Debug, message);
        public void Info(string message) => Log(LogLevel.Info, message);
        public void Warning(string message) => Log(LogLevel.Warning, message);
        public void Error(string message) => Log(LogLevel.Error, message);
        
        public IReadOnlyList<LogEntry> GetAllLogs()
        {
            lock (_lockObject)
            {
                return _database.ToList();
            }
        }
    }
    
    public class LogEntry
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public LogLevel Level { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
```

### 👑 **Passo 8: Implementar Singleton LogManager**

```csharp
// src/Managers/LogManager.cs
using Exercicio01.Interfaces;
using Exercicio01.Factories;

namespace Exercicio01.Managers
{
    public sealed class LogManager
    {
        private static LogManager? _instance;
        private static readonly object _lock = new object();
        
        private ILogger? _currentLogger;
        
        // Construtor privado para Singleton
        private LogManager() { }
        
        public static LogManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new LogManager();
                    }
                }
                return _instance;
            }
        }
        
        public void SetLogger(LoggerFactory factory)
        {
            _currentLogger = factory.CreateLogger();
        }
        
        public void Debug(string message) => _currentLogger?.Debug(message);
        public void Info(string message) => _currentLogger?.Info(message);
        public void Warning(string message) => _currentLogger?.Warning(message);
        public void Error(string message) => _currentLogger?.Error(message);
        
        // Método auxiliar para configuração rápida
        public void SetConsoleLogger()
            => SetLogger(new ConsoleLoggerFactory());
            
        public void SetFileLogger(string filePath)
            => SetLogger(new FileLoggerFactory(filePath));
            
        public void SetDatabaseLogger(string connectionString)
            => SetLogger(new DatabaseLoggerFactory(connectionString));
    }
}
```

### 🎮 **Passo 9: Programa Principal**

```csharp
// src/Program.cs
using Exercicio01.Managers;

namespace Exercicio01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(\"=== Sistema de Logging - Exercício 1 ===\");
            
            var logManager = LogManager.Instance;
            
            // Teste 1: Console Logger
            Console.WriteLine(\"\\n1. Testando Console Logger:\");
            logManager.SetConsoleLogger();
            TestLogging(logManager);
            
            // Teste 2: File Logger
            Console.WriteLine(\"\\n2. Testando File Logger:\");
            logManager.SetFileLogger(\"logs/app.log\");
            TestLogging(logManager);
            
            // Teste 3: Database Logger
            Console.WriteLine(\"\\n3. Testando Database Logger:\");
            logManager.SetDatabaseLogger(\"Server=localhost;Database=Logs\");
            TestLogging(logManager);
            
            // Teste 4: Thread Safety
            Console.WriteLine(\"\\n4. Testando Thread Safety:\");
            TestThreadSafety();
            
            Console.WriteLine(\"\\nPressione qualquer tecla para sair...\");
            Console.ReadKey();
        }
        
        static void TestLogging(LogManager logManager)
        {
            logManager.Debug(\"Mensagem de debug\");
            logManager.Info(\"Sistema iniciado com sucesso\");
            logManager.Warning(\"Atenção: Memória baixa\");
            logManager.Error(\"Erro crítico detectado\");
        }
        
        static void TestThreadSafety()
        {
            var logManager = LogManager.Instance;
            logManager.SetConsoleLogger();
            
            var tasks = new List<Task>();
            
            for (int i = 0; i < 5; i++)
            {
                int taskId = i;
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        logManager.Info($\"Task {taskId} - Message {j}\");
                        Thread.Sleep(10); // Simula algum processamento
                    }
                }));
            }
            
            Task.WaitAll(tasks.ToArray());
            logManager.Info(\"Teste de thread safety concluído\");
        }
    }
}
```

### 📦 **Passo 10: Arquivo de Projeto**

```xml
<!-- Exercicio01.csproj -->
<Project Sdk=\"Microsoft.NET.Sdk\">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

---

## 🧪 **Testes (Opcional)**

```csharp
// tests/LoggingTests.cs
using Xunit;
using Exercicio01.Managers;
using Exercicio01.Factories;

namespace Exercicio01.Tests
{
    public class LoggingTests
    {
        [Fact]
        public void LogManager_Should_Be_Singleton()
        {
            // Arrange & Act
            var instance1 = LogManager.Instance;
            var instance2 = LogManager.Instance;
            
            // Assert
            Assert.Same(instance1, instance2);
        }
        
        [Fact]
        public void Factory_Should_Create_Different_Loggers()
        {
            // Arrange
            var consoleFactory = new ConsoleLoggerFactory();
            var fileFactory = new FileLoggerFactory(\"test.log\");
            
            // Act
            var consoleLogger = consoleFactory.CreateLogger();
            var fileLogger = fileFactory.CreateLogger();
            
            // Assert
            Assert.NotSame(consoleLogger, fileLogger);
            Assert.IsType<ConsoleLogger>(consoleLogger);
            Assert.IsType<FileLogger>(fileLogger);
        }
    }
}
```

---

## ✅ **Critérios de Validação**

### 🎯 **Funcionalidade**
- [ ] Sistema logga em console com cores diferentes por nível
- [ ] Sistema logga em arquivo (cria diretório se necessário)  
- [ ] Sistema logga em \"banco de dados\" simulado
- [ ] Troca entre tipos de logger dinamicamente
- [ ] Thread-safe para uso concurrent

### 🏗️ **Design Patterns**
- [ ] **Singleton**: LogManager tem instância única
- [ ] **Factory Method**: LoggerFactory cria loggers apropriados
- [ ] **Thread-Safety**: Implementado corretamente

### 📝 **Código**
- [ ] Código limpo e bem estruturado
- [ ] Tratamento de erros apropriado
- [ ] Logs formatados corretamente

---

## 🎯 **Desafios Extras** (Opcional)

1. **📊 Filtro por Nível**: Implementar filtro de nível mínimo
2. **🔄 Rotação de Arquivos**: Implementar rotação de logs por tamanho
3. **📧 Email Logger**: Criar EmailLogger usando padrões similares
4. **⚙️ Configuração**: Carregar configuração de arquivo JSON
5. **📈 Métricas**: Adicionar contadores de logs por nível

---

## 💡 **Dicas**

### ✅ **Do**
- Use `lock` para thread-safety
- Implemente `IDisposable` em FileLogger
- Valide parâmetros de entrada
- Use cores no console para melhor UX

### ❌ **Don't**  
- Não use Singleton para tudo
- Não deixe arquivos abertos
- Não ignore exceptions
- Não acople Factory ao LogManager

---

## 🎉 **Resultado Esperado**

Ao executar, você deve ver:
```
=== Sistema de Logging - Exercício 1 ===

1. Testando Console Logger:
[2024-01-15 14:30:15] [DEBUG] Mensagem de debug
[2024-01-15 14:30:15] [INFO] Sistema iniciado com sucesso
[2024-01-15 14:30:15] [WARNING] Atenção: Memória baixa
[2024-01-15 14:30:15] [ERROR] Erro crítico detectado

2. Testando File Logger:
[Logs salvos em logs/app.log]

3. Testando Database Logger:
[DB] Inserido log #abc-123: [DEBUG] Mensagem de debug
[DB] Inserido log #def-456: [INFO] Sistema iniciado com sucesso
...
```

**🏆 Parabéns! Você implementou com sucesso os padrões Singleton e Factory Method!**