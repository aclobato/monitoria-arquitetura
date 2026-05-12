# 🛠️ Guia Prático: spec-kit com GitHub Copilot

## 📦 Instalação

O spec-kit usa o `uv` (gerenciador de pacotes Python moderno e rápido).

### 1. Instalar o uv (se ainda não tiver)

```bash
# Windows (PowerShell)
powershell -ExecutionPolicy ByPass -c "irm https://astral.sh/uv/install.ps1 | iex"

# macOS / Linux
curl -LsSf https://astral.sh/uv/install.sh | sh
```

### 2. Instalar o specify-cli

```bash
# Versão estável mais recente
uv tool install specify-cli --from git+https://github.com/github/spec-kit.git

# Verificar instalação
specify --version
```

### 3. Confirmar integração com Copilot

Após instalar, os comandos `/speckit.*` ficam disponíveis automaticamente no **GitHub Copilot Chat** dentro do VS Code.

---

## 🚀 Inicializando um Projeto

```bash
# Dentro da pasta raiz do projeto
specify init NOME-DO-PROJETO
```

Este comando cria a estrutura base:

```
.specify/
├── memory/
│   └── constitution.md      # Governança do projeto (a preencher)
├── templates/               # Templates core do spec-kit
├── extensions/              # Extensões opcionais
└── presets/                 # Customizações da equipe
```

> ✅ **Dica**: Commite a pasta `.specify/` no repositório. Os artefatos gerados são documentação viva do projeto.

---

## 📋 Estrutura de Artefatos

Cada feature especificada via SDD gera sua própria pasta em `.specify/specs/`:

```
.specify/specs/[nome-da-feature]/
├── spec.md      # O quê construir (requisitos, casos de uso, critérios de aceite)
├── plan.md      # Como construir (arquitetura, componentes, trade-offs)
└── tasks.md     # Lista ordenada de tarefas com dependências
```

---

## ⚡ Comandos Principais

### `/speckit.constitution`

**Quando usar:** No início do projeto, para estabelecer princípios de governança.

```
/speckit.constitution

Projeto: Sistema de Monitoramento de Arquitetura
Stack: .NET 8, xUnit, Azure DevOps
Padrões: Clean Architecture, SOLID, DDD
Convenções: PascalCase para classes, camelCase para variáveis
```

O Copilot gera um `constitution.md` com as diretrizes do projeto. **Revise e ajuste** antes de continuar.

---

### `/speckit.specify`

**Quando usar:** Para transformar uma ideia ou requisito em uma spec estruturada.

```
/speckit.specify

Preciso de um sistema de notificações que permita:
- Enviar notificações por e-mail, SMS e push
- Configurar preferências por usuário
- Agendar notificações para horários específicos
- Ter histórico de notificações enviadas
```

O Copilot gera um `spec.md` com:
- Descrição funcional detalhada
- Casos de uso com atores
- Critérios de aceite mensuráveis
- O que está **fora** do escopo

---

### `/speckit.clarify`

**Quando usar:** Quando a spec tem ambiguidades ou decisões em aberto.

```
/speckit.clarify

Na spec de notificações, não ficou claro:
- Qual o limite de notificações por usuário por dia?
- O que acontece se o canal (e-mail/SMS) estiver indisponível?
- As preferências de usuário têm valor padrão ou precisam ser configuradas?
```

O Copilot faz perguntas estruturadas e incorpora as respostas na spec. **Execute quantas vezes precisar** até a spec estar sem ambiguidades.

---

### `/speckit.plan`

**Quando usar:** Após a spec estar validada, para definir a arquitetura técnica.

```
/speckit.plan specs/sistema-notificacoes/spec.md
```

O Copilot gera um `plan.md` com:
- Arquitetura de componentes
- Escolhas técnicas com justificativas
- Fluxos de dados
- Estratégia de testes
- Riscos e mitigações

---

### `/speckit.tasks`

**Quando usar:** Após o plan estar validado, para gerar as tarefas de implementação.

```
/speckit.tasks specs/sistema-notificacoes/plan.md
```

O Copilot gera um `tasks.md` com:
- Tarefas ordenadas por dependência
- Estimativa de complexidade para cada tarefa
- Critérios claros de "feito"
- Tarefas de teste incluídas

---

### `/speckit.implement`

**Quando usar:** Para implementar uma tarefa específica com contexto completo da spec.

```
/speckit.implement specs/sistema-notificacoes/tasks.md#tarefa-3

Implementar a interface INotificationChannel com os métodos Send e IsAvailable
```

O Copilot usa spec + plan como contexto para gerar código alinhado com as decisões já tomadas.

---

### `/speckit.analyze`

**Quando usar:** Para verificar consistência entre os artefatos gerados.

```
/speckit.analyze specs/sistema-notificacoes/
```

O Copilot verifica se spec, plan e tasks estão alinhados e aponta divergências. Execute após mudanças para garantir que os artefatos continuam coerentes.

---

## 🔄 Fluxo Completo: Exemplo Passo a Passo

```
1. specify init MEU-PROJETO
       ↓
2. /speckit.constitution  →  Revisa constitution.md
       ↓
3. /speckit.specify  →  Revisa spec.md
       ↓
4. /speckit.clarify  →  Repete até spec estar clara
       ↓
5. /speckit.plan  →  Revisa plan.md
       ↓
6. /speckit.tasks  →  Revisa tasks.md
       ↓
7. /speckit.implement (tarefa 1)  →  Revisa, commita
8. /speckit.implement (tarefa 2)  →  Revisa, commita
   ...
       ↓
9. /speckit.analyze  →  Verifica consistência final
```

---

## 🎛️ Customizando com Presets

Presets permitem adaptar o spec-kit ao contexto da equipe sem modificar os templates core:

```bash
# Criar preset para sua equipe
specify preset create minha-empresa
```

Edite `.specify/presets/minha-empresa/` para definir:
- Terminologia customizada (ex: "história" em vez de "spec")
- Formato preferido de tasks (ex: integração com Jira)
- Regras arquiteturais específicas da empresa

---

## 📂 Resultado Esperado

Ao final do processo, você terá:
- ✅ Especificação completa e revisada (`spec.md`)
- ✅ Arquitetura técnica documentada (`plan.md`)
- ✅ Lista de tarefas rastreável (`tasks.md`)
- ✅ Código implementado de forma coerente com as decisões documentadas
- ✅ Documentação viva que pode ser evoluída junto com o projeto
