# Constitution: Monitoria de Arquitetura de Software

## Propósito do Projeto

Repositório educacional para ensinar práticas de arquitetura de software a desenvolvedores em formação, por meio de documentação teórica, exercícios práticos progressivos e exemplos de código funcional.

## Stack e Tecnologias

- **Linguagem:** C# (.NET 8)
- **Testes:** xUnit
- **CI/CD:** Azure DevOps Pipelines
- **Controle de versão:** Git / GitHub
- **Ferramentas de IA:** GitHub Copilot, spec-kit

## Convenções de Código

- **Nomes:** PascalCase para classes, interfaces e métodos; camelCase para variáveis e parâmetros
- **Interfaces:** prefixadas com `I` (ex: `INotificationChannel`)
- **Testes:** método nomeado como `Metodo_Cenario_ResultadoEsperado` (ex: `Send_WhenChannelUnavailable_ReturnsFalse`)
- **Idioma do código:** Inglês (nomes de classes, métodos, variáveis)
- **Idioma da documentação:** Português brasileiro

## Convenções de Documentação

- Documentação sempre em português brasileiro
- Emojis usados para organização visual (🎯 objetivos, ✅ critérios, 📋 cenários)
- Exercícios organizados em ordem crescente de dificuldade: Iniciante → Intermediário → Avançado
- Cada exercício deve ter: tempo estimado, pré-requisitos, critérios de sucesso

## Padrões Arquiteturais

- Exemplos de código devem ser autocontidos e executáveis sem dependências externas
- Preferência por injeção de dependência via construtor
- Interfaces antes de implementações (programar para abstrações)
- Testes unitários obrigatórios para cada exemplo de código
- Sem acesso a recursos externos reais (banco de dados real, APIs externas) nos exercícios — usar fakes/mocks

## Organização de Diretórios

```
topico/
├── documentacao/   # Teoria e conceitos (01-, 02-, 03-...)
├── exercicios/     # Exercícios práticos com guias passo a passo
└── codigo-exemplo/ # Código funcional de referência com testes
```

## Regras de Qualidade

- Todo código de exemplo deve ter ao menos um teste que demonstre o comportamento principal
- Exercícios não devem ter dependência de recursos pagos ou com limite de uso
- Documentação deve ser suficiente para alguém completar o exercício sem suporte externo
