# 📐 Fundamentos do Spec-Driven Development (SDD)

## 🤔 O que é Spec-Driven Development?

**Spec-Driven Development (SDD)** é uma metodologia de desenvolvimento de software onde **especificações detalhadas** são o artefato principal que guia toda a implementação — não apenas um pré-requisito burocrático, mas um **blueprint executável** que a IA usa para desenvolver com precisão.

> 💡 **Analogia**: Pense na diferença entre construir uma casa com e sem planta arquitetônica. Sem planta, cada pedreiro decide na hora. Com planta detalhada, todos trabalham coordenados para o mesmo resultado.

## ⚡ Por que SDD surgiu?

### O problema do "vibe coding"

Com a popularização de ferramentas como GitHub Copilot e ChatGPT, surgiu um padrão chamado **"vibe coding"**: o desenvolvedor vai pedindo trechos de código, corrigindo erros conforme aparecem e acumulando uma base de código que ninguém entende completamente — nem a IA, nem o humano.

**Sintomas do vibe coding:**
- Código que "funciona" mas ninguém sabe exatamente por quê
- Prompts cada vez mais longos tentando dar contexto acumulado
- Refatorações constantes porque a direção mudou no meio do caminho
- Dificuldade em integrar partes desenvolvidas em sessões diferentes

### A proposta do SDD

O SDD "inverte o script": em vez de usar IA para escrever código diretamente, você primeiro usa IA para **especificar com precisão o que precisa ser feito** — e só então usa IA para implementar.

```
❌ Vibe Coding:   Ideia → Prompt → Código → Ajustes → Mais ajustes → ...
✅ SDD:           Ideia → Spec → Clarify → Plan → Tasks → Implement ✓
```

## 📊 SDD vs Desenvolvimento Tradicional

| Aspecto | Desenvolvimento Tradicional | Spec-Driven Development |
|---|---|---|
| **Ponto de partida** | Requisitos vagos ou história de usuário | Spec detalhada e validada |
| **Direcionamento da IA** | Prompts pontuais sem contexto | Spec como contexto persistente |
| **Resultado esperado** | Emergente (descobre-se fazendo) | Previsível (definido antes) |
| **Revisão** | Revisão de código depois | Revisão de spec antes |
| **Documentação** | Escrever depois (se escrever) | Gerada durante o processo |
| **Mudanças de escopo** | Custosas e frequentes | Identificadas na fase de spec |

## 🔧 O papel do spec-kit

O **spec-kit** é um toolkit open-source que implementa o SDD fornecendo:

- **Comandos estruturados** para guiar cada fase do processo via Copilot
- **Templates padronizados** para specs, planos e tarefas
- **Sistema de extensões** para customizar ao contexto da equipe
- **Compatibilidade** com 30+ agentes de IA (Copilot, Claude Code, Cursor, Gemini, etc.)

## 🗺️ As 6 Fases do Fluxo SDD

```
┌─────────────────────────────────────────────────────────┐
│                    FLUXO SDD                            │
│                                                         │
│  1. CONSTITUTION  →  Princípios de governança           │
│         ↓                                               │
│  2. SPECIFICATION →  O que construir (requisitos)       │
│         ↓                                               │
│  3. PLANNING      →  Como construir (arquitetura)       │
│         ↓                                               │
│  4. TASK BREAKDOWN → Etapas ordenadas com dependências  │
│         ↓                                               │
│  5. IMPLEMENTATION → Execução sistemática               │
│         ↓                                               │
│  6. VALIDATION    →  Teste e refinamento                │
└─────────────────────────────────────────────────────────┘
```

### Fase 1: Constitution
Define os **princípios de governança** do projeto: tecnologias adotadas, padrões de código, convenções de nomes, regras arquiteturais. É o "contrato" que guia todas as decisões.

> 📝 Arquivo gerado: `.specify/memory/constitution.md`

### Fase 2: Specification
Define **o quê** precisa ser construído — não como. Cobre requisitos funcionais, casos de uso, critérios de aceite e o que está fora do escopo.

> 📝 Arquivo gerado: `.specify/specs/[feature]/spec.md`

### Fase 3: Planning
Define **como** construir: arquitetura técnica, escolhas de tecnologia, componentes envolvidos, integrações e trade-offs.

> 📝 Arquivo gerado: `.specify/specs/[feature]/plan.md`

### Fase 4: Task Breakdown
Gera uma **lista ordenada de tarefas** com dependências claras, estimativas e critérios de conclusão para cada item.

> 📝 Arquivo gerado: `.specify/specs/[feature]/tasks.md`

### Fase 5: Implementation
Executa as tarefas sistematicamente, com a IA usando spec + plan + tasks como contexto completo para gerar código preciso.

### Fase 6: Validation
Testa o resultado contra os critérios definidos na spec e refina o que for necessário.

## 🤝 Integração com GitHub Copilot

O spec-kit se integra ao Copilot por meio de **slash commands** que a ferramenta registra no ambiente:

| Comando | Fase | O que faz |
|---|---|---|
| `/speckit.constitution` | 1 | Define princípios de governança do projeto |
| `/speckit.specify` | 2 | Gera spec detalhada a partir de uma descrição |
| `/speckit.clarify` | 2 | Resolve ambiguidades na spec |
| `/speckit.plan` | 3 | Cria arquitetura técnica baseada na spec |
| `/speckit.tasks` | 4 | Gera lista de tarefas ordenadas |
| `/speckit.implement` | 5 | Implementa uma tarefa específica |
| `/speckit.analyze` | Qualquer | Verifica consistência entre artefatos |

## 🎯 Quando usar SDD?

**SDD é mais valioso quando:**
- A feature tem complexidade média a alta
- Mais de uma pessoa vai trabalhar no mesmo contexto
- A spec precisa ser revisada antes de implementar
- Você quer documentação gerada automaticamente
- O contexto da IA precisa ser consistente entre sessões

**Veja também:** [Boas Práticas e Dicas](./03-boas-praticas-e-dicas.md) para saber quando **não** usar SDD.
