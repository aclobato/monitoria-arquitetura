namespace SingletonExample;

/// <summary>
/// Implementação thread-safe do padrão Singleton para logging
/// Demonstra as principais características do padrão:
/// - Instância única
/// - Thread-safety  
/// - Lazy initialization
/// - Estado global
/// </summary>
public sealed class Logger
{
    private static Logger? _instance;
    private static readonly object _lock = new object();
    
    private LogLevel _minimumLevel = LogLevel.Info;
    private readonly Dictionary<LogLevel, int> _logCounts = new();
    private int _totalLogs = 0;

    // Construtor privado impede criação externa
    private Logger()
    {
        // Inicializar contadores
        foreach (LogLevel level in Enum.GetValues<LogLevel>())
        {
            _logCounts[level] = 0;
        }
        
        Console.WriteLine("[LOGGER] Instância do Logger criada");
    }

    /// <summary>
    /// Propriedade que garante uma única instância (thread-safe)
    /// Usa Double-Checked Locking pattern para otimização
    /// </summary>
    public static Logger Instance
    {
        get
        {
            // Primeira verificação (performance)
            if (_instance == null)
            {
                // Lock apenas se necessário
                lock (_lock)
                {
                    // Segunda verificação (thread-safety)
                    if (_instance == null)
                        _instance = new Logger();
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Método principal de logging
    /// </summary>
    public void Log(string message, LogLevel level = LogLevel.Info)
    {
        // Filtrar por nível mínimo
        if (level < _minimumLevel)
            return;

        lock (_lock)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var levelStr = level.ToString().ToUpper();
            
            // Colorir baseado no nível
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = GetColorForLevel(level);
            
            Console.WriteLine($"[{timestamp}] [{levelStr}] {message}");
            
            Console.ForegroundColor = originalColor;
            
            // Atualizar estatísticas
            _logCounts[level]++;
            _totalLogs++;
        }
    }

    /// <summary>
    /// Configurar nível mínimo de logging
    /// </summary>
    public void SetLevel(LogLevel level)
    {
        lock (_lock)
        {
            _minimumLevel = level;
            Console.WriteLine($"[LOGGER] Nível mínimo alterado para: {level}");
        }
    }

    /// <summary>
    /// Obter estatísticas de uso
    /// </summary>
    public LogStatistics GetStatistics()
    {
        lock (_lock)
        {
            return new LogStatistics
            {
                TotalLogs = _totalLogs,
                LogsByLevel = new Dictionary<LogLevel, int>(_logCounts)
            };
        }
    }

    /// <summary>
    /// Limpar histórico de logs (para testes)
    /// </summary>
    public void ClearStatistics()
    {
        lock (_lock)
        {
            _totalLogs = 0;
            foreach (var key in _logCounts.Keys.ToList())
            {
                _logCounts[key] = 0;
            }
        }
    }

    private static ConsoleColor GetColorForLevel(LogLevel level)
    {
        return level switch
        {
            LogLevel.Debug => ConsoleColor.Gray,
            LogLevel.Info => ConsoleColor.White,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Critical => ConsoleColor.Magenta,
            _ => ConsoleColor.White
        };
    }
}

/// <summary>
/// Níveis de logging suportados
/// </summary>
public enum LogLevel
{
    Debug = 0,
    Info = 1,
    Warning = 2,
    Error = 3,
    Critical = 4
}

/// <summary>
/// Estatísticas de uso do logger
/// </summary>
public class LogStatistics
{
    public int TotalLogs { get; set; }
    public Dictionary<LogLevel, int> LogsByLevel { get; set; } = new();
}