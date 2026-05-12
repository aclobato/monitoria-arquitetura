# 🎯 Exercícios Práticos - Design Patterns

## 📋 Visão Geral dos Exercícios

Este diretório contém exercícios práticos progressivos para dominar Design Patterns. Cada exercício foca em padrões específicos e cenários reais de desenvolvimento.

### 🎯 **Objetivos**
- ✅ Aplicar padrões em cenários práticos
- 🔧 Refatorar código usando padrões apropriados  
- 🧪 Implementar testes para validar soluções
- 📝 Documentar decisões de design

### ⏱️ **Tempo Estimado**
- **Exercício 1**: 45-60 minutos (Iniciante)
- **Exercício 2**: 60-90 minutos (Intermediário)  
- **Exercício 3**: 90-120 minutos (Avançado)

---

## 📚 **Lista de Exercícios**

### 🚀 **Exercício 1 - Sistema de Logging** (Padrões Criacionais)
**📁 Arquivo**: `exercicio-01-sistema-logging.md`  
**🎯 Padrões**: Singleton, Factory Method  
**⏱️ Duração**: 45-60 minutos  
**📝 Descrição**: Implementar sistema de logging flexível  

**✨ O que você vai aprender**:
- Como garantir instância única (Singleton)
- Criar objetos sem especificar classes concretas (Factory)
- Thread-safety em aplicações C#

---

### 🏗️ **Exercício 2 - E-commerce com Decorator** (Padrões Estruturais)  
**📁 Arquivo**: `exercicio-02-ecommerce-decorator.md`  
**🎯 Padrões**: Decorator, Adapter, Facade  
**⏱️ Duração**: 60-90 minutos  
**📝 Descrição**: Sistema de produtos com funcionalidades dinâmicas  

**✨ O que você vai aprender**:
- Adicionar funcionalidades dinamicamente (Decorator)
- Integrar sistemas legados (Adapter)  
- Simplificar interfaces complexas (Facade)

---

### 🎮 **Exercício 3 - Sistema de Notificações** (Padrões Comportamentais)
**📁 Arquivo**: `exercicio-03-notificacoes.md`  
**🎯 Padrões**: Observer, Strategy, Command  
**⏱️ Duração**: 90-120 minutos  
**📝 Descrição**: Sistema completo de notificações em tempo real  

**✨ O que você vai aprender**:
- Notificar múltiplos observadores (Observer)
- Trocar algoritmos dinamicamente (Strategy)
- Encapsular operações como objetos (Command)

---

## 🛠️ **Como Executar os Exercícios**

### 📋 **Pré-requisitos**
- ✅ .NET 6 ou superior instalado
- ✅ Visual Studio ou VS Code  
- ✅ Git para controle de versão
- ✅ Conhecimento básico de C#

### 🚀 **Passo a Passo**

#### 1️⃣ **Preparação**
```bash
# Clone ou navegue até o diretório
cd design-patterns/exercicios

# Escolha um exercício
cd exercicio-01  # ou 02, 03
```

#### 2️⃣ **Leitura**
- 📖 Leia o arquivo `.md` do exercício escolhido
- 🎯 Entenda os requisitos e objetivos  
- 📋 Revise os padrões que serão aplicados

#### 3️⃣ **Implementação**
- 💻 Implemente a solução seguindo as instruções
- 🧪 Execute os testes fornecidos
- ✅ Valide se todos os cenários funcionam

#### 4️⃣ **Validação**
```bash
# Execute os testes
dotnet test

# Execute a aplicação  
dotnet run
```

#### 5️⃣ **Reflexão**
- 📝 Compare sua solução com a sugerida
- 🤔 Analise trade-offs das decisões tomadas
- 🔄 Refatore se necessário

---

## 📊 **Critérios de Avaliação**

### ✅ **Funcionalidade** (40%)
- [ ] Todos os requisitos implementados
- [ ] Testes passando
- [ ] Aplicação executando sem erros

### 🏗️ **Design Patterns** (35%)
- [ ] Padrões aplicados corretamente
- [ ] Problemas reais resolvidos pelos padrões
- [ ] Implementação segue boas práticas

### 📝 **Código** (25%)
- [ ] Código limpo e legível
- [ ] Nomes descritivos
- [ ] Estrutura organizada
- [ ] Comentários relevantes (quando necessário)

---

## 🎯 **Dicas de Sucesso**

### 💡 **Antes de Começar**
- 📚 **Revise a teoria**: Releia a documentação dos padrões
- 🎯 **Entenda o problema**: Antes de pensar na solução
- 📋 **Planeje**: Esboce a arquitetura no papel

### ⚡ **Durante a Implementação**
- 🔄 **Iterativo**: Implemente um padrão por vez
- 🧪 **Teste cedo**: Execute testes frequentemente  
- 🤔 **Questione**: Por que este padrão aqui?

### 🏁 **Ao Finalizar**
- 📊 **Compare**: Sua solução vs. exemplo oficial
- 🔍 **Analise**: Pontos fortes e fracos
- 📝 **Documente**: Aprendizados e insights

---

## 🆘 **Precisa de Ajuda?**

### 📚 **Recursos de Apoio**
1. **Documentação**: Consulte `documentacao/02-guia-pratico-padroes.md`
2. **Referências**: Veja `documentacao/03-referencias-estudo.md`  
3. **Código-exemplo**: Explore `codigo-exemplo/` para inspiração

### 🐛 **Problemas Comuns**

#### **"Não sei qual padrão usar"**
- ❓ **Problema**: Confusão sobre aplicabilidade
- ✅ **Solução**: Foque no problema primeiro, depois no padrão

#### **"Minha implementação está complexa"**
- ❓ **Problema**: Over-engineering  
- ✅ **Solução**: Comece simples, adicione complexidade gradualmente

#### **"Os testes não passam"**
- ❓ **Problema**: Implementação incorreta
- ✅ **Solução**: Verifique se seguiu exatamente os requisitos

#### **"Não consigo compilar"**
- ❓ **Problema**: Dependências ou configuração
- ✅ **Solução**: Verifique se tem .NET correto instalado

---

## 🎉 **Após Completar**

### 🌟 **Próximos Passos**
- 🔄 **Refatore**: Aplique os padrões em projetos pessoais
- 👥 **Compartilhe**: Discuta soluções com colegas
- 📖 **Aprofunde**: Estude padrões arquiteturais avançados
- 🏗️ **Pratique**: Crie novos exercícios e desafios

### 🏆 **Certificado de Conclusão**
Ao completar todos os exercícios:
- ✅ Você dominará 9 padrões fundamentais
- 🎯 Saberá quando aplicar cada padrão  
- 💻 Terá código de qualidade profissional no portfólio
- 🧠 Pensará em design de software de forma estruturada

---

**💜 Bons estudos e mãos à obra!**

*Lembre-se: o objetivo não é usar todos os padrões, mas aplicar o padrão certo no momento certo!* 🎯