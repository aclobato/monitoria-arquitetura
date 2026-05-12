# 👑 Singleton Pattern - Logger Example

## 🎯 Objetivo

Demonstrar implementação thread-safe do padrão **Singleton** através de um sistema de logging global que mantém estatísticas de uso.

## 📚 Conceitos Demonstrados

- ✅ **Instância única**: Apenas um logger em toda aplicação
- 🔒 **Thread-safety**: Double-checked locking pattern
- 🐌 **Lazy initialization**: Criação apenas quando necessário
- 📊 **Estado global**: Configuração e estatísticas compartilhadas

---

## 🏗️ Arquitetura

```
Logger (Singleton)
├── Instance (propriedade estática)
├── Log(message, level) 
├── SetLevel(minimumLevel)
├── GetStatistics()
└── ClearStatistics()

LogLevel (enum)
├── Debug
├── Info  
├── Warning
├── Error
└── Critical

LogStatistics (class)
├── TotalLogs
└── LogsByLevel
```

---

## 🚀 Como Executar

### ▶️ **Executar Exemplo**
```bash
# No diretório do exemplo
dotnet run
```

**Saída esperada**:
```
=== Exemplo Prático: Singleton Pattern ===

1. Testando instância única:
logger1 == logger2: True

2. Testando funcionalidade:  
[2024-01-15 14:30:15.123] [INFO] Sistema iniciado
[2024-01-15 14:30:15.125] [INFO] Primeira operação executada
[2024-01-15 14:30:15.127] [INFO] Sistema funcionando normalmente

3. Testando thread-safety:
[Múltiplas mensagens de diferentes threads...]

4. Configurando logger:
[LOGGER] Nível mínimo alterado para: Warning
[2024-01-15 14:30:15.200] [ERROR] Esta mensagem aparecerá

5. Estatísticas do logger:
Total de logs: 15
Logs por nível:
  Info: 8
  Warning: 3  
  Error: 4
```

### 🧪 **Executar Testes**
```bash
dotnet test
```

---

## 🔍 **Pontos de Interesse no Código**

### 🔒 **Thread-Safe Implementation** 
```csharp
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
```

**Por que funciona**:
- ✅ **Double-checked locking**: Evita lock desnecessário
- 🔒 **Thread-safety**: Garante uma única instância
- ⚡ **Performance**: Lock apenas na primeira criação

### 🏗️ **Construtor Privado**
```csharp
// Construtor privado impede criação externa
private Logger()
{
    // Inicialização...
}
```

**Propósito**: Impedir `new Logger()` - força uso do `Instance`

### 📊 **Estado Global Thread-Safe**
```csharp
public void Log(string message, LogLevel level = LogLevel.Info)
{
    lock (_lock)
    {
        // Operações thread-safe
        _logCounts[level]++;
        _totalLogs++;
    }
}
```

**Benefícios**: Estatísticas consistentes em ambiente multi-thread

---

## ✅ **Vantagens do Padrão**

1. **🎯 Controle de Acesso**: Ponto único de acesso global
2. **💾 Economia de Recursos**: Uma única instância em memória  
3. **🔄 Estado Consistente**: Configuração e dados compartilhados
4. **🔒 Thread-Safety**: Implementação segura para concorrência

---

## ❌ **Desvantagens e Cuidados**

1. **🧪 Dificulta Testes**: Estado global pode interferir em testes
2. **🔗 Acoplamento**: Classe fica acoplada ao Singleton
3. **👥 Problemas de Herança**: Difícil de especializar
4. **🌍 Escopo Global**: Pode violar princípios de encapsulamento

---

## 🎯 **Quando Usar**

### ✅ **Bons Casos de Uso**
- 📝 **Logging**: Um logger global na aplicação
- ⚙️ **Configuração**: Settings globais da aplicação
- 🏊 **Pool de Recursos**: Pool de conexões de banco
- 💾 **Cache**: Cache único em memória

### ❌ **Evitar Quando**
- 🧪 **Testabilidade** é prioritária
- 👥 **Herança** é necessária
- 🔄 **Múltiplas instâncias** podem ser úteis
- 🎯 **Injeção de Dependência** está disponível

---

## 🔧 **Variações do Padrão**

### 1️⃣ **Lazy Initialization** (Implementação atual)
```csharp
// Criação apenas quando necessário
if (_instance == null) 
    _instance = new Logger();
```

### 2️⃣ **Eager Initialization**
```csharp
// Criação na inicialização da classe
private static readonly Logger _instance = new Logger();
```

### 3️⃣ **Using Lazy<T>**
```csharp
private static readonly Lazy<Logger> _lazy = 
    new Lazy<Logger>(() => new Logger());

public static Logger Instance => _lazy.Value;
```

---

## 🧪 **Testes Incluídos**

1. **🔄 Instância Única**: Verifica se sempre retorna a mesma instância
2. **🔒 Thread-Safety**: Testa acesso concurrent ao singleton
3. **📊 Filtragem de Nível**: Valida configuração de nível mínimo
4. **📈 Contagem de Logs**: Verifica estatísticas corretas
5. **⚡ Logging Concurrent**: Testa múltiplas threads logando
6. **🧹 Reset de Estatísticas**: Valida limpeza de estado

---

## 💡 **Exercícios Sugeridos**

### 🎯 **Básico**
1. Adicionar método `GetLevel()` para consultar nível atual
2. Implementar logging em arquivo além do console
3. Adicionar timestamp personalizado

### 🏗️ **Intermediário**  
4. Implementar rotação de logs por tamanho
5. Adicionar formatação customizável de mensagens
6. Criar configuração via arquivo JSON

### 🚀 **Avançado**
7. Implementar Singleton thread-safe sem locks (usando Lazy<T>)
8. Adicionar suporte a múltiplos appenders (console, arquivo, etc.)
9. Criar sistema de logging estruturado (JSON)

---

## 📚 **Comparação com Alternativas**

### 🆚 **Singleton vs Dependency Injection**

| Aspecto | Singleton | Dependency Injection |
|---------|-----------|---------------------|
| **Testabilidade** | ❌ Difícil | ✅ Fácil |
| **Acoplamento** | ❌ Alto | ✅ Baixo |
| **Flexibilidade** | ❌ Limitada | ✅ Alta |
| **Simplicidade** | ✅ Simples | ❌ Mais complexo |

### 🎯 **Quando Escolher Cada Um**

**Singleton**:
- Aplicações simples
- Prototipagem rápida  
- Recursos verdadeiramente únicos

**Dependency Injection**:
- Aplicações corporativas
- Alta testabilidade necessária
- Arquiteturas complexas

---

## 🎉 **Conclusão**

O padrão Singleton é útil para casos específicos onde uma única instância é genuinamente necessária. Esta implementação demonstra:

- ✅ **Thread-safety** com performance otimizada
- 📊 **Estado global** consistente
- 🔄 **Interface simples** e intuitiva
- 🧪 **Testabilidade** (com limitações)

**💡 Lição**: Use Singleton quando realmente precisar de uma única instância global, mas considere alternativas como Dependency Injection para aplicações mais complexas.

---

**🎯 Próximo passo**: Experimente modificar o código e implementar suas próprias variações!