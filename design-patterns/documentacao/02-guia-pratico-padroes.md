# 🛠️ Guia Prático de Design Patterns

## 🏗️ Padrões Criacionais

### 🎯 1. Singleton

**Problema**: Preciso garantir que uma classe tenha apenas uma instância.

**Solução**: Controlar a criação da instância na própria classe.

```csharp
public sealed class Logger
{
    private static Logger? _instance;
    private static readonly object _lock = new object();

    private Logger() { }

    public static Logger Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new Logger();
                }
            }
            return _instance;
        }
    }

    public void Log(string message)
    {
        Console.WriteLine($"[{DateTime.Now}] {message}");
    }
}

// Uso
Logger.Instance.Log("Sistema iniciado");
```

**✅ Quando usar**: Logging, Cache, Pool de Conexões  
**❌ Cuidados**: Dificulta testes, pode criar acoplamento

---

### 🏭 2. Factory Method

**Problema**: Criar objetos sem especificar suas classes concretas.

**Solução**: Delegar a criação para subclasses.

```csharp
// Interface do produto
public interface IPayment
{
    void ProcessPayment(decimal amount);
}

// Produtos concretos
public class CreditCardPayment : IPayment
{
    public void ProcessPayment(decimal amount)
        => Console.WriteLine($"Processando R$ {amount} no cartão de crédito");
}

public class PixPayment : IPayment
{
    public void ProcessPayment(decimal amount)
        => Console.WriteLine($"Processando R$ {amount} via PIX");
}

// Factory abstrata
public abstract class PaymentFactory
{
    public abstract IPayment CreatePayment();
    
    public void ProcessTransaction(decimal amount)
    {
        var payment = CreatePayment();
        payment.ProcessPayment(amount);
    }
}

// Factories concretas
public class CreditCardFactory : PaymentFactory
{
    public override IPayment CreatePayment() => new CreditCardPayment();
}

public class PixFactory : PaymentFactory
{
    public override IPayment CreatePayment() => new PixPayment();
}

// Uso
PaymentFactory factory = new PixFactory();
factory.ProcessTransaction(100.50m);
```

**✅ Quando usar**: Sistema de plugins, múltiplas implementações  
**❌ Cuidados**: Pode criar muitas classes pequenas

---

### 🏗️ 3. Builder

**Problema**: Construir objetos complexos passo a passo.

**Solução**: Separar a construção da representação.

```csharp
public class Pizza
{
    public string Massa { get; set; } = "";
    public string Molho { get; set; } = "";
    public List<string> Ingredientes { get; set; } = new();
    public string Tamanho { get; set; } = "";

    public override string ToString()
        => $"Pizza {Tamanho}: {Massa}, {Molho}, [{string.Join(", ", Ingredientes)}]";
}

public class PizzaBuilder
{
    private Pizza _pizza = new();

    public PizzaBuilder ComMassa(string massa)
    {
        _pizza.Massa = massa;
        return this;
    }

    public PizzaBuilder ComMolho(string molho)
    {
        _pizza.Molho = molho;
        return this;
    }

    public PizzaBuilder ComIngrediente(string ingrediente)
    {
        _pizza.Ingredientes.Add(ingrediente);
        return this;
    }

    public PizzaBuilder Tamanho(string tamanho)
    {
        _pizza.Tamanho = tamanho;
        return this;
    }

    public Pizza Build() => _pizza;
}

// Uso
var pizza = new PizzaBuilder()
    .Tamanho("Grande")
    .ComMassa("Integral")
    .ComMolho("Tomate")
    .ComIngrediente("Mussarela")
    .ComIngrediente("Calabresa")
    .Build();

Console.WriteLine(pizza);
```

**✅ Quando usar**: Objetos com muitos parâmetros opcionais  
**❌ Cuidados**: Pode ser complexo para objetos simples

---

## 🏛️ Padrões Estruturais

### 🔌 4. Adapter

**Problema**: Integrar classes com interfaces incompatíveis.

**Solução**: Criar um adaptador que converte uma interface em outra.

```csharp
// Interface moderna que queremos usar
public interface IModernLogger
{
    void LogInfo(string message);
    void LogError(string message);
}

// Classe legada que não podemos modificar
public class LegacyLogger
{
    public void WriteLog(string level, string message)
    {
        Console.WriteLine($"[LEGACY] {level}: {message}");
    }
}

// Adapter
public class LegacyLoggerAdapter : IModernLogger
{
    private readonly LegacyLogger _legacyLogger;

    public LegacyLoggerAdapter(LegacyLogger legacyLogger)
    {
        _legacyLogger = legacyLogger;
    }

    public void LogInfo(string message)
        => _legacyLogger.WriteLog("INFO", message);

    public void LogError(string message)
        => _legacyLogger.WriteLog("ERROR", message);
}

// Uso
IModernLogger logger = new LegacyLoggerAdapter(new LegacyLogger());
logger.LogInfo("Sistema funcionando");
logger.LogError("Erro detectado");
```

**✅ Quando usar**: Integração com APIs legadas, bibliotecas externas  
**❌ Cuidados**: Adiciona uma camada de abstração

---

### 🎨 5. Decorator

**Problema**: Adicionar funcionalidades a objetos dinamicamente.

**Solução**: Envolver objetos em camadas de decoradores.

```csharp
public interface ICoffee
{
    string GetDescription();
    decimal GetCost();
}

// Componente base
public class SimpleCoffee : ICoffee
{
    public string GetDescription() => "Café simples";
    public decimal GetCost() => 5.00m;
}

// Decorator base
public abstract class CoffeeDecorator : ICoffee
{
    protected ICoffee _coffee;

    public CoffeeDecorator(ICoffee coffee)
    {
        _coffee = coffee;
    }

    public virtual string GetDescription() => _coffee.GetDescription();
    public virtual decimal GetCost() => _coffee.GetCost();
}

// Decorators concretos
public class MilkDecorator : CoffeeDecorator
{
    public MilkDecorator(ICoffee coffee) : base(coffee) { }

    public override string GetDescription() => _coffee.GetDescription() + ", com leite";
    public override decimal GetCost() => _coffee.GetCost() + 1.50m;
}

public class SugarDecorator : CoffeeDecorator
{
    public SugarDecorator(ICoffee coffee) : base(coffee) { }

    public override string GetDescription() => _coffee.GetDescription() + ", com açúcar";
    public override decimal GetCost() => _coffee.GetCost() + 0.50m;
}

// Uso
ICoffee coffee = new SimpleCoffee();
coffee = new MilkDecorator(coffee);
coffee = new SugarDecorator(coffee);

Console.WriteLine($"{coffee.GetDescription()} - R$ {coffee.GetCost()}");
// Output: Café simples, com leite, com açúcar - R$ 7,00
```

**✅ Quando usar**: Sistema de permissões, filtros, middleware  
**❌ Cuidados**: Pode criar muitos objetos pequenos

---

### 🏢 6. Facade

**Problema**: Interface complexa para subsistema complicado.

**Solução**: Criar uma interface simplificada.

```csharp
// Subsistemas complexos
public class DatabaseService
{
    public void Connect() => Console.WriteLine("Conectando ao banco...");
    public void ExecuteQuery(string query) => Console.WriteLine($"Executando: {query}");
    public void Disconnect() => Console.WriteLine("Desconectando do banco...");
}

public class LoggingService
{
    public void StartLogging() => Console.WriteLine("Iniciando logs...");
    public void Log(string message) => Console.WriteLine($"Log: {message}");
}

public class CacheService
{
    public void InitializeCache() => Console.WriteLine("Inicializando cache...");
    public void ClearCache() => Console.WriteLine("Limpando cache...");
}

// Facade
public class ApplicationFacade
{
    private readonly DatabaseService _database;
    private readonly LoggingService _logging;
    private readonly CacheService _cache;

    public ApplicationFacade()
    {
        _database = new DatabaseService();
        _logging = new LoggingService();
        _cache = new CacheService();
    }

    public void StartApplication()
    {
        _logging.StartLogging();
        _cache.InitializeCache();
        _database.Connect();
        _logging.Log("Aplicação iniciada");
    }

    public void ProcessUser(string userName)
    {
        _database.ExecuteQuery($"SELECT * FROM Users WHERE Name = '{userName}'");
        _logging.Log($"Usuário {userName} processado");
    }

    public void StopApplication()
    {
        _logging.Log("Parando aplicação");
        _cache.ClearCache();
        _database.Disconnect();
    }
}

// Uso
var app = new ApplicationFacade();
app.StartApplication();
app.ProcessUser("João");
app.StopApplication();
```

**✅ Quando usar**: APIs complexas, SDKs, integração com subsistemas  
**❌ Cuidados**: Pode esconder detalhes importantes

---

## 🎭 Padrões Comportamentais

### 👁️ 7. Observer

**Problema**: Notificar múltiplos objetos sobre mudanças de estado.

**Solução**: Lista de observadores que são notificados automaticamente.

```csharp
public interface IObserver
{
    void Update(string message);
}

public interface ISubject
{
    void Subscribe(IObserver observer);
    void Unsubscribe(IObserver observer);
    void Notify(string message);
}

// Subject concreto
public class NewsAgency : ISubject
{
    private List<IObserver> _observers = new();
    private string _news = "";

    public string News
    {
        get => _news;
        set
        {
            _news = value;
            Notify($"Notícia: {_news}");
        }
    }

    public void Subscribe(IObserver observer) => _observers.Add(observer);
    public void Unsubscribe(IObserver observer) => _observers.Remove(observer);
    public void Notify(string message) => _observers.ForEach(o => o.Update(message));
}

// Observers concretos
public class NewsChannel : IObserver
{
    private string _name;

    public NewsChannel(string name) => _name = name;

    public void Update(string message)
        => Console.WriteLine($"[{_name}] Recebeu: {message}");
}

// Uso
var agency = new NewsAgency();
var cnn = new NewsChannel("CNN");
var bbc = new NewsChannel("BBC");

agency.Subscribe(cnn);
agency.Subscribe(bbc);

agency.News = "Design Patterns são úteis!";
// Output:
// [CNN] Recebeu: Notícia: Design Patterns são úteis!
// [BBC] Recebeu: Notícia: Design Patterns são úteis!
```

**✅ Quando usar**: Eventos, Model-View patterns, pub/sub  
**❌ Cuidados**: Pode criar vazamentos de memória

---

### 🎯 8. Strategy

**Problema**: Escolher algoritmos dinamicamente.

**Solução**: Encapsular algoritmos em classes separadas.

```csharp
public interface IShippingStrategy
{
    decimal CalculateCost(decimal weight, decimal distance);
    string GetDescription();
}

// Estratégias concretas
public class StandardShipping : IShippingStrategy
{
    public decimal CalculateCost(decimal weight, decimal distance)
        => weight * 2 + distance * 0.1m;

    public string GetDescription() => "Entrega Padrão (5-7 dias)";
}

public class ExpressShipping : IShippingStrategy
{
    public decimal CalculateCost(decimal weight, decimal distance)
        => weight * 5 + distance * 0.2m;

    public string GetDescription() => "Entrega Expressa (1-2 dias)";
}

public class EconomicShipping : IShippingStrategy
{
    public decimal CalculateCost(decimal weight, decimal distance)
        => weight * 1 + distance * 0.05m;

    public string GetDescription() => "Entrega Econômica (10-15 dias)";
}

// Contexto
public class ShippingCalculator
{
    private IShippingStrategy _strategy;

    public ShippingCalculator(IShippingStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IShippingStrategy strategy)
    {
        _strategy = strategy;
    }

    public void CalculateShipping(decimal weight, decimal distance)
    {
        var cost = _strategy.CalculateCost(weight, distance);
        Console.WriteLine($"{_strategy.GetDescription()}: R$ {cost:F2}");
    }
}

// Uso
var calculator = new ShippingCalculator(new StandardShipping());
calculator.CalculateShipping(5, 100);

calculator.SetStrategy(new ExpressShipping());
calculator.CalculateShipping(5, 100);
```

**✅ Quando usar**: Múltiplos algoritmos, regras de negócio variáveis  
**❌ Cuidados**: Pode aumentar número de classes

---

### 📝 9. Command

**Problema**: Encapsular requisições como objetos.

**Solução**: Transformar chamadas de método em objetos.

```csharp
// Interface do comando
public interface ICommand
{
    void Execute();
    void Undo();
}

// Receiver
public class Light
{
    private bool _isOn = false;

    public void TurnOn()
    {
        _isOn = true;
        Console.WriteLine("Luz ligada");
    }

    public void TurnOff()
    {
        _isOn = false;
        Console.WriteLine("Luz desligada");
    }
}

// Comandos concretos
public class LightOnCommand : ICommand
{
    private Light _light;

    public LightOnCommand(Light light) => _light = light;

    public void Execute() => _light.TurnOn();
    public void Undo() => _light.TurnOff();
}

public class LightOffCommand : ICommand
{
    private Light _light;

    public LightOffCommand(Light light) => _light = light;

    public void Execute() => _light.TurnOff();
    public void Undo() => _light.TurnOn();
}

// Invoker
public class RemoteControl
{
    private ICommand? _lastCommand;

    public void PressButton(ICommand command)
    {
        command.Execute();
        _lastCommand = command;
    }

    public void PressUndo()
    {
        _lastCommand?.Undo();
    }
}

// Uso
var light = new Light();
var remote = new RemoteControl();

var lightOn = new LightOnCommand(light);
var lightOff = new LightOffCommand(light);

remote.PressButton(lightOn);   // Luz ligada
remote.PressButton(lightOff);  // Luz desligada
remote.PressUndo();            // Luz ligada (undo)
```

**✅ Quando usar**: Undo/Redo, filas de operações, logging  
**❌ Cuidados**: Pode criar muitas classes pequenas

---

## 🎯 Resumo dos Padrões

| Categoria | Padrão | Use quando | Evite quando |
|-----------|---------|------------|--------------|
| **Criacional** | Singleton | Precisar de instância única | Facilitar testes for prioridade |
| **Criacional** | Factory | Criar objetos sem saber tipo concreto | Tipos são conhecidos e fixos |
| **Criacional** | Builder | Construir objetos complexos | Objetos simples com poucos parâmetros |
| **Estrutural** | Adapter | Integrar interfaces incompatíveis | Pode modificar as classes originais |
| **Estrutural** | Decorator | Adicionar funcionalidades dinamicamente | Funcionalidades são estáticas |
| **Estrutural** | Facade | Simplificar interface complexa | Interface simples já existe |
| **Comportamental** | Observer | Notificar mudanças para múltiplos objetos | Poucos objetos interessados |
| **Comportamental** | Strategy | Alternar algoritmos dinamicamente | Algoritmo é fixo |
| **Comportamental** | Command | Encapsular operações como objetos | Operações são simples e diretas |

---

**🎯 Próximo passo**: Pratique esses padrões nos exercícios e implemente os códigos-exemplo!