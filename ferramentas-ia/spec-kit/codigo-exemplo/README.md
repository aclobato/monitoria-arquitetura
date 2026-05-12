# 💻 Código de Exemplo — Artefatos spec-kit

## 📋 O que tem aqui

Este diretório contém artefatos `.specify/` de exemplo para um projeto real do repositório de monitoria — o **Sistema de Notificações com Fallback** (tema do Exercício 3).

Esses artefatos mostram como `spec.md`, `plan.md` e `tasks.md` bem elaborados se parecem na prática.

## 📂 Estrutura

```
codigo-exemplo/
└── exemplo-specify/
    └── .specify/
        ├── memory/
        │   └── constitution.md          ← Governança do projeto
        └── specs/
            └── notificacao-fallback/
                ├── spec.md              ← Requisitos e critérios de aceite
                ├── plan.md              ← Arquitetura técnica
                └── tasks.md             ← Tarefas ordenadas por dependência
```

## 🎯 Como usar estes artefatos

### Como referência de qualidade

Leia cada arquivo e compare com os que você gerou nos exercícios:
- A spec cobre casos de erro? Os critérios são mensuráveis?
- O plan justifica as decisões técnicas?
- As tasks estão ordenadas por dependência e têm critério claro de "feito"?

### Como ponto de partida

Se quiser usar esses artefatos diretamente no seu projeto:

```bash
# Copiar a estrutura para o seu projeto
cp -r exemplo-specify/.specify/ SEU-PROJETO/.specify/

# Inicializar o spec-kit (se ainda não fez)
specify init SEU-PROJETO
```

### Com o Copilot

Abra qualquer arquivo `.specify/specs/notificacao-fallback/` e execute:

```
/speckit.implement specs/notificacao-fallback/tasks.md

Implementar: Tarefa 1 - Criar interface INotificationChannel
```

O Copilot usará spec + plan como contexto para gerar código alinhado com as decisões documentadas.

## 📖 Como interpretar cada artefato

| Arquivo | Pergunta que responde | Quem deve revisar |
|---|---|---|
| `constitution.md` | Quais são as regras do jogo? | Time inteiro, 1x por projeto |
| `spec.md` | O que o sistema deve fazer? | PO, Tech Lead, QA |
| `plan.md` | Como vamos construir? | Tech Lead, desenvolvedores sênior |
| `tasks.md` | Quais passos, em que ordem? | Desenvolvedor responsável |
