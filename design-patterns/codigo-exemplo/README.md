# 💻 Códigos-Exemplo - Design Patterns

## 📚 Visão Geral

Este diretório contém implementações completas e funcionais dos principais Design Patterns. Os códigos são **prontos para executar** e servem como **referência prática** para seus estudos.

### 🎯 **Objetivo**
- ✅ Fornecer exemplos funcionais dos padrões
- 🔍 Demonstrar implementações corretas  
- 📋 Servir como base para os exercícios
- 🧪 Incluir testes unitários

---

## 📁 **Estrutura dos Exemplos**

### 🏗️ **Padrões Criacionais**
```
criacionais/
├── singleton/              # Instância única
│   ├── src/
│   ├── tests/
│   └── README.md
├── factory-method/         # Criação de objetos
│   ├── src/
│   ├── tests/
│   └── README.md
└── builder/               # Construção complexa
    ├── src/
    ├── tests/
    └── README.md
```

### 🏛️ **Padrões Estruturais**
```
estruturais/
├── adapter/               # Compatibilidade de interfaces
│   ├── src/
│   ├── tests/
│   └── README.md
├── decorator/             # Funcionalidades dinâmicas
│   ├── src/
│   ├── tests/
│   └── README.md
└── facade/               # Interface simplificada
    ├── src/
    ├── tests/
    └── README.md
```

### 🎭 **Padrões Comportamentais**
```
comportamentais/
├── observer/              # Notificação de mudanças
│   ├── src/
│   ├── tests/
│   └── README.md
├── strategy/              # Algoritmos intercambiáveis
│   ├── src/
│   ├── tests/
│   └── README.md
└── command/              # Encapsular operações
    ├── src/
    ├── tests/
    └── README.md
```

---

## 🚀 **Como Usar os Exemplos**

### 📋 **Pré-requisitos**
- ✅ .NET 6 ou superior
- ✅ Visual Studio ou VS Code
- ✅ Git (opcional)

### 🔧 **Executar um Exemplo**

```bash
# Navegue até um padrão específico
cd criacionais/singleton

# Compile e execute
dotnet run

# Execute os testes
dotnet test
```

### 📖 **Estudar um Padrão**

1. **📚 Leia o README.md** do padrão
2. **🔍 Analise o código** em `src/`
3. **🧪 Execute os testes** em `tests/`
4. **⚡ Execute o exemplo** com `dotnet run`
5. **🔧 Modifique** e experimente

---

## 📊 **Características dos Exemplos**

### ✨ **Qualidade**
- ✅ **Código limpo**: Seguindo boas práticas
- 🧪 **Testado**: Testes unitários incluídos
- 📝 **Documentado**: Comentários relevantes
- 🎯 **Focado**: Um padrão por vez

### 🎨 **Cenários Realistas**
- 💼 **Casos de uso reais**: Problemas do mundo real
- 🔧 **Implementações práticas**: Não apenas teoria
- 📈 **Progressiva**: Do simples ao complexo
- 🌟 **Extensível**: Fácil de modificar e expandir

---

## 🏆 **Padrões Incluídos**

### 🏗️ **Criacionais** (3 padrões)

#### 👑 **Singleton**
**📝 Cenário**: Sistema de logging global  
**🎯 Aprenda**: Thread-safety, lazy initialization  
**⏱️ Tempo**: 15 minutos  

#### 🏭 **Factory Method**  
**📝 Cenário**: Sistema de pagamentos  
**🎯 Aprenda**: Criação flexível de objetos  
**⏱️ Tempo**: 20 minutos  

#### 🏗️ **Builder**
**📝 Cenário**: Construtor de consultas SQL  
**🎯 Aprenda**: Construção passo a passo  
**⏱️ Tempo**: 25 minutos  

---

### 🏛️ **Estruturais** (3 padrões)

#### 🔌 **Adapter**
**📝 Cenário**: Integração com API legada  
**🎯 Aprenda**: Compatibilidade de interfaces  
**⏱️ Tempo**: 20 minutos  

#### 🎨 **Decorator**
**📝 Cenário**: Sistema de permissões  
**🎯 Aprenda**: Extensão dinâmica de funcionalidades  
**⏱️ Tempo**: 25 minutos  

#### 🏢 **Facade**
**📝 Cenário**: SDK para serviços externos  
**🎯 Aprenda**: Simplificação de interfaces complexas  
**⏱️ Tempo**: 20 minutos  

---

### 🎭 **Comportamentais** (3 padrões)

#### 👁️ **Observer**
**📝 Cenário**: Sistema de notificações  
**🎯 Aprenda**: Comunicação entre objetos  
**⏱️ Tempo**: 25 minutos  

#### 🎯 **Strategy**
**📝 Cenário**: Cálculo de frete  
**🎯 Aprenda**: Algoritmos intercambiáveis  
**⏱️ Tempo**: 20 minutos  

#### 📝 **Command**
**📝 Cenário**: Editor com undo/redo  
**🎯 Aprenda**: Encapsulamento de operações  
**⏱️ Tempo**: 30 minutos  

---

## 📚 **Ordem de Estudo Recomendada**

### 🎯 **Iniciante** (1-2 semanas)
1. 👑 **Singleton** - Mais simples de entender
2. 🏭 **Factory Method** - Conceito fundamental  
3. 🔌 **Adapter** - Problema comum em integrações
4. 🎯 **Strategy** - Muito útil no dia a dia

### 🏗️ **Intermediário** (2-3 semanas)
5. 🎨 **Decorator** - Padrão poderoso mas complexo
6. 🏢 **Facade** - Simplifica arquiteturas
7. 👁️ **Observer** - Base para programação reativa

### 🚀 **Avançado** (1-2 semanas)
8. 🏗️ **Builder** - Para construções complexas
9. 📝 **Command** - Padrão sofisticado

---

## 🧪 **Executar Todos os Testes**

```bash
# Na raiz do diretório codigo-exemplo
find . -name \"*.csproj\" -exec dirname {} \\; | xargs -I {} sh -c 'cd \"{}\" && echo \"Testing {}\" && dotnet test'
```

---

## 🔧 **Modificar e Experimentar**

### 💡 **Sugestões de Modificação**

#### **Singleton**
- Implementar diferentes tipos de inicialização
- Adicionar configuração via arquivo
- Testar performance em cenários multi-thread

#### **Factory Method**
- Adicionar novos tipos de pagamento
- Implementar cache de instances  
- Criar factory abstrata mais genérica

#### **Builder**
- Adicionar validação de parâmetros obrigatórios
- Implementar builder fluent mais complexo
- Criar diferentes tipos de builders

#### **Adapter**
- Criar adaptadores bidirecionais
- Implementar cache no adapter
- Adaptar múltiplas interfaces

#### **Decorator**
- Implementar decorator chain
- Adicionar configuração dinâmica
- Criar decorators compostos

#### **Facade**
- Implementar facade assíncrono
- Adicionar tratamento de erros robusto
- Criar facade configurável

#### **Observer**
- Implementar filtros de eventos
- Adicionar prioridades nos observers
- Criar observers assíncronos

#### **Strategy**
- Implementar strategy factory
- Adicionar cache de strategies
- Criar strategy composto

#### **Command**
- Implementar command queue
- Adicionar logging de comandos
- Criar macro commands

---

## 🆘 **Troubleshooting**

### ❓ **Problemas Comuns**

#### **\"dotnet command not found\"**
- Instale o .NET SDK 6 ou superior
- Verifique se está no PATH

#### **\"Build failed\"**
- Verifique se está no diretório correto
- Execute `dotnet restore` primeiro

#### **\"Tests não executam\"**
- Certifique-se que há arquivo `*Tests.csproj`
- Execute `dotnet test --logger console`

#### **\"Código não compila\"**
- Verifique dependências em `*.csproj`
- Execute `dotnet build` para ver erros

---

## 💎 **Próximos Passos**

### 🎯 **Após Dominar os Exemplos**
1. **🔄 Refatore**: Aplique os padrões em projetos pessoais
2. **🎮 Combine**: Use múltiplos padrões juntos
3. **📚 Aprofunde**: Estude padrões arquiteturais (MVC, MVVM, etc.)
4. **👥 Compartilhe**: Crie seus próprios exemplos
5. **🏗️ Pratique**: Implemente em projetos reais

### 🌟 **Recursos Adicionais**
- 📖 Leia `documentacao/02-guia-pratico-padroes.md`
- 🎯 Faça os exercícios em `exercicios/`
- 📚 Consulte `documentacao/03-referencias-estudo.md`

---

**💜 Explore, experimente e aprenda fazendo!**

*Lembre-se: o melhor código é aquele que resolve um problema real de forma simples e elegante* ✨