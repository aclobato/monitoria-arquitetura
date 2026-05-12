using SingletonExample;

Console.WriteLine("=== Exemplo Prático: Singleton Pattern ===");
Console.WriteLine();

// Demonstração básica do Singleton
Console.WriteLine("1. Testando instância única:");
var logger1 = Logger.Instance;
var logger2 = Logger.Instance;

Console.WriteLine($"logger1 == logger2: {ReferenceEquals(logger1, logger2)}");
Console.WriteLine();

// Teste funcional
Console.WriteLine("2. Testando funcionalidade:");
logger1.Log("Sistema iniciado");
logger2.Log("Primeira operação executada");
logger1.Log("Sistema funcionando normalmente");
Console.WriteLine();

// Teste de thread-safety
Console.WriteLine("3. Testando thread-safety:");
var tasks = new List<Task>();

for (int i = 0; i < 5; i++)
{
    int taskId = i;
    tasks.Add(Task.Run(() =>
    {
        var logger = Logger.Instance;
        logger.Log($"Mensagem da Task {taskId}");
        
        // Simular algum processamento
        Thread.Sleep(100);
        
        logger.Log($"Task {taskId} finalizada");
    }));
}

Task.WaitAll(tasks.ToArray());
Console.WriteLine();

// Demonstração de configuração
Console.WriteLine("4. Configurando logger:");
logger1.SetLevel(LogLevel.Warning);
logger1.Log("Esta mensagem aparecerá", LogLevel.Error);
logger1.Log("Esta mensagem NÃO aparecerá", LogLevel.Info);
Console.WriteLine();

// Estatísticas
Console.WriteLine("5. Estatísticas do logger:");
var stats = logger1.GetStatistics();
Console.WriteLine($"Total de logs: {stats.TotalLogs}");
Console.WriteLine($"Logs por nível:");
foreach (var kvp in stats.LogsByLevel)
{
    Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
}

Console.WriteLine();
Console.WriteLine("Pressione qualquer tecla para sair...");
Console.ReadKey();