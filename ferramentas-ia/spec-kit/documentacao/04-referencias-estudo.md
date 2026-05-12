# 📚 Referências de Estudo — spec-kit e SDD

## 🔧 Recursos Oficiais do spec-kit

| Recurso | Descrição |
|---|---|
| [Repositório GitHub](https://github.com/github/spec-kit) | Código-fonte, issues e documentação oficial |
| [README do projeto](https://github.com/github/spec-kit#readme) | Visão geral, instalação e primeiros passos |
| [Templates core](https://github.com/github/spec-kit/tree/main/.specify/templates) | Templates padrão para spec, plan e tasks |
| [Exemplos de extensões](https://github.com/github/spec-kit/tree/main/.specify/extensions) | Extensões disponíveis (Jira, segurança, etc.) |

## 📖 Sobre Spec-Driven Development

### Conceitos fundamentais

- **Spec-Driven Development** — Metodologia que prioriza especificação antes de implementação, usando IA como parceiro de refinamento
- **Intent-driven development** — Termo relacionado que enfatiza capturar a intenção, não apenas o código
- **Living documentation** — Documentação que evolui junto com o código, não em paralelo

### Comparativos com outras metodologias

| Metodologia | Foco principal | Relação com SDD |
|---|---|---|
| **TDD** (Test-Driven) | Testes como guia de implementação | Complementar — critérios de aceite da spec viram testes |
| **BDD** (Behavior-Driven) | Comportamento descrito em linguagem natural | Complementar — spec vira cenários Gherkin |
| **DDD** (Domain-Driven) | Modelo de domínio como centro do design | Complementar — spec usa linguagem ubíqua, plan mapeia para entidades |
| **Vibe Coding** | Prompts ad-hoc, código emergente | Oposto — SDD é a alternativa estruturada |

## 🤖 GitHub Copilot — Recursos Avançados

| Recurso | Descrição |
|---|---|
| [Copilot Chat — Slash Commands](https://docs.github.com/en/copilot/using-github-copilot/asking-github-copilot-questions-in-your-ide) | Documentação oficial dos comandos do Copilot |
| [Copilot Extensions](https://docs.github.com/en/copilot/building-copilot-extensions) | Como construir extensões personalizadas |
| [Agentes de Copilot](https://docs.github.com/en/copilot/using-github-copilot/using-github-copilot-in-your-ide) | Uso de agentes no VS Code e JetBrains |

## 🛠️ Ferramentas Relacionadas

| Ferramenta | Descrição |
|---|---|
| [uv](https://github.com/astral-sh/uv) | Gerenciador de pacotes Python (necessário para instalar o specify-cli) |
| [Claude Code](https://claude.ai/code) | Agente de IA compatível com spec-kit |
| [Cursor](https://www.cursor.com/) | Editor com IA integrada, compatível com spec-kit |

## 📝 Conceitos para Aprofundar

### Especificação de software

Antes de usar SDD com IA, vale entender os fundamentos de especificação:

- **Requisitos funcionais vs não-funcionais** — o que o sistema faz vs como ele se comporta
- **Critérios de aceite** — condições verificáveis que definem quando uma feature está pronta
- **Casos de uso** — interações entre atores e o sistema para atingir um objetivo
- **Escopo e limitações** — o que o sistema não vai fazer (tão importante quanto o que vai)

### Engenharia de prompt para especificação

Para obter specs de qualidade com o Copilot, vale estudar:

- **Contexto é tudo** — quanto mais contexto de negócio, melhor a spec gerada
- **Iterar, não perfeccionar no primeiro try** — use clarify para refinar
- **Separar "o quê" do "como"** — resista à tentação de especificar implementação na spec

## 🎓 Trilha de Aprendizado Sugerida

```
1. 📐 Fundamentos SDD          → 01-fundamentos-spec-driven-development.md
        ↓
2. 🛠️ Guia prático             → 02-guia-pratico-spec-kit.md
        ↓
3. 💡 Boas práticas            → 03-boas-praticas-e-dicas.md
        ↓
4. 🏋️ Prática guiada          → exercicios/
   ├── Exercício 1: Instalação e setup
   ├── Exercício 2: Especificando uma feature
   └── Exercício 3: Ciclo completo SDD
        ↓
5. 📂 Referência de artefatos  → codigo-exemplo/
```
