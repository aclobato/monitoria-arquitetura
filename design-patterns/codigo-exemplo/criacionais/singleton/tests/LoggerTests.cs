using Xunit;
using SingletonExample;

namespace SingletonExample.Tests;

/// <summary>
/// Testes unitários para demonstrar comportamento do Singleton
/// </summary>
public class LoggerTests
{
    [Fact]
    public void Logger_Should_Return_Same_Instance()
    {
        // Arrange & Act
        var logger1 = Logger.Instance;
        var logger2 = Logger.Instance;
        
        // Assert
        Assert.Same(logger1, logger2);
    }

    [Fact]
    public void Logger_Should_Be_Thread_Safe()
    {
        // Arrange
        var instances = new List<Logger>();
        var tasks = new List<Task>();

        // Act - Criar múltiplas tasks que acessam o singleton
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                var logger = Logger.Instance;
                lock (instances)
                {
                    instances.Add(logger);
                }
            }));
        }

        Task.WaitAll(tasks.ToArray());

        // Assert - Todas as instâncias devem ser a mesma
        var firstInstance = instances.First();
        Assert.All(instances, instance => Assert.Same(firstInstance, instance));
    }

    [Fact]
    public void Logger_Should_Filter_By_Minimum_Level()
    {
        // Arrange
        var logger = Logger.Instance;
        logger.ClearStatistics(); // Limpar estado anterior
        logger.SetLevel(LogLevel.Warning);

        // Act
        logger.Log("Debug message", LogLevel.Debug);     // Não deve aparecer
        logger.Log("Info message", LogLevel.Info);       // Não deve aparecer  
        logger.Log("Warning message", LogLevel.Warning); // Deve aparecer
        logger.Log("Error message", LogLevel.Error);     // Deve aparecer

        // Assert
        var stats = logger.GetStatistics();
        Assert.Equal(2, stats.TotalLogs); // Apenas Warning e Error
        Assert.Equal(0, stats.LogsByLevel[LogLevel.Debug]);
        Assert.Equal(0, stats.LogsByLevel[LogLevel.Info]);
        Assert.Equal(1, stats.LogsByLevel[LogLevel.Warning]);
        Assert.Equal(1, stats.LogsByLevel[LogLevel.Error]);
    }

    [Fact]
    public void Logger_Should_Count_Logs_Correctly()
    {
        // Arrange
        var logger = Logger.Instance;
        logger.ClearStatistics();
        logger.SetLevel(LogLevel.Debug); // Permitir todos os níveis

        // Act
        logger.Log("Debug 1", LogLevel.Debug);
        logger.Log("Debug 2", LogLevel.Debug);
        logger.Log("Info 1", LogLevel.Info);
        logger.Log("Warning 1", LogLevel.Warning);
        logger.Log("Error 1", LogLevel.Error);

        // Assert
        var stats = logger.GetStatistics();
        Assert.Equal(5, stats.TotalLogs);
        Assert.Equal(2, stats.LogsByLevel[LogLevel.Debug]);
        Assert.Equal(1, stats.LogsByLevel[LogLevel.Info]);
        Assert.Equal(1, stats.LogsByLevel[LogLevel.Warning]);
        Assert.Equal(1, stats.LogsByLevel[LogLevel.Error]);
    }

    [Fact]
    public void Logger_Should_Handle_Concurrent_Logging()
    {
        // Arrange
        var logger = Logger.Instance;
        logger.ClearStatistics();
        logger.SetLevel(LogLevel.Info);
        var tasks = new List<Task>();

        // Act - Múltiplas threads logando simultaneamente
        for (int i = 0; i < 5; i++)
        {
            int taskId = i;
            tasks.Add(Task.Run(() =>
            {
                for (int j = 0; j < 10; j++)
                {
                    logger.Log($"Task {taskId} Message {j}", LogLevel.Info);
                    Thread.Sleep(1); // Pequena pausa para simular trabalho
                }
            }));
        }

        Task.WaitAll(tasks.ToArray());

        // Assert
        var stats = logger.GetStatistics();
        Assert.Equal(50, stats.TotalLogs); // 5 tasks * 10 mensagens cada
        Assert.Equal(50, stats.LogsByLevel[LogLevel.Info]);
    }

    [Fact]
    public void Logger_Clear_Should_Reset_Statistics()
    {
        // Arrange
        var logger = Logger.Instance;
        logger.SetLevel(LogLevel.Debug);
        logger.Log("Test message", LogLevel.Info);

        // Act
        logger.ClearStatistics();

        // Assert
        var stats = logger.GetStatistics();
        Assert.Equal(0, stats.TotalLogs);
        Assert.All(stats.LogsByLevel.Values, count => Assert.Equal(0, count));
    }
}

/// <summary>
/// Testes de integração para demonstrar uso real do Singleton
/// </summary>
public class LoggerIntegrationTests
{
    [Fact]
    public void Logger_Should_Work_Across_Multiple_Classes()
    {
        // Arrange
        var service1 = new UserService();
        var service2 = new OrderService(); 
        Logger.Instance.ClearStatistics();
        Logger.Instance.SetLevel(LogLevel.Info);

        // Act
        service1.CreateUser("João");
        service2.CreateOrder(123);
        service1.DeleteUser("Maria");

        // Assert
        var stats = Logger.Instance.GetStatistics();
        Assert.True(stats.TotalLogs >= 3); // Pelo menos 3 logs foram criados
    }
}

// Classes auxiliares para testes de integração
internal class UserService
{
    public void CreateUser(string name)
    {
        Logger.Instance.Log($"Criando usuário: {name}", LogLevel.Info);
        // Simular lógica de negócio
        Logger.Instance.Log($"Usuário {name} criado com sucesso", LogLevel.Info);
    }

    public void DeleteUser(string name)
    {
        Logger.Instance.Log($"Deletando usuário: {name}", LogLevel.Warning);
    }
}

internal class OrderService  
{
    public void CreateOrder(int orderId)
    {
        Logger.Instance.Log($"Criando pedido #{orderId}", LogLevel.Info);
    }
}