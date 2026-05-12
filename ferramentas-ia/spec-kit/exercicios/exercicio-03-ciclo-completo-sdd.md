# 🔴 Exercício 3: Ciclo Completo SDD

**Dificuldade:** Avançado | **Tempo estimado:** 90-120 min

---

## 🎯 Objetivo

Executar o ciclo completo do Spec-Driven Development — da especificação à implementação — usando spec-kit com GitHub Copilot, e comparar o resultado com o desenvolvimento ad-hoc para internalizar o valor da abordagem.

---

## 📋 Cenário

O `MonitoriaBase` registra exercícios concluídos, mas não tem como **notificar alunos** sobre eventos do sistema (ex: novo feedback recebido, novo exercício disponível). Você vai adicionar um sistema de notificação com **fallback automático de canal** usando SDD completo.

> 📂 **Projeto de referência:** `ferramentas-ia/spec-kit/codigo-exemplo/projeto-base/`  
> Os artefatos `.specify/` de exemplo estão em `codigo-exemplo/exemplo-specify/` — use-os como referência de qualidade, mas construa os seus próprios.

**Feature a implementar:**
> O sistema deve conseguir enviar notificações aos alunos. Cada aluno tem canais preferidos em ordem de prioridade (e-mail, SMS, push). Se o canal primário falhar, o sistema tenta automaticamente o próximo, até obter sucesso ou esgotar as opções.

---

## ✅ Pré-requisitos

- [ ] Exercícios 1 e 2 concluídos
- [ ] spec-kit instalado e Copilot integrado
- [ ] `constitution.md` criada no `projeto-base/` (Exercício 1)
- [ ] `dotnet test` passando no `projeto-base/`

---

## 🗺️ Visão Geral do Ciclo

```
Fase 1: SPECIFY     → spec.md (o quê construir)
Fase 2: CLARIFY     → spec.md refinada (sem ambiguidades)
Fase 3: PLAN        → plan.md (como construir)
Fase 4: TASKS       → tasks.md (etapas ordenadas)
Fase 5: IMPLEMENT   → código alinhado com spec+plan
Fase 6: ANALYZE     → verificar consistência
```

---

## 📐 Fase 1: Specify

### 1.1 Criar pasta da feature

```bash
mkdir .specify/specs/notificacao-fallback
```

### 1.2 Gerar a spec

No Copilot Chat:

```
/speckit.specify

FEATURE: Sistema de notificação de alunos com fallback automático de canal
CONTEXTO: O MonitoriaBase (Student, Exercise, ExerciseAttempt, MonitoriaService)
  ainda não tem mecanismo de notificação. Alunos precisam ser notificados sobre
  eventos como novo feedback recebido ou novo exercício disponível.
SISTEMA EXISTENTE:
  - Student tem Id, Name, Email (apenas e-mail, sem canal de notificação configurado ainda)
  - MonitoriaService gerencia alunos, exercícios e tentativas
PROBLEMA: Sem notificações, alunos precisam verificar ativamente se há atualizações.
SOLICITADO: Sistema de notificação onde cada aluno configura canais preferidos
  (e-mail, SMS, push) em ordem de prioridade. Se o canal primário falhar, o sistema
  tenta automaticamente o próximo até obter sucesso ou esgotar as opções.
RESTRIÇÕES:
  - Não reenviar pelo mesmo canal que já falhou nesta notificação
  - Retornar qual canal foi usado efetivamente
  - Registrar todas as tentativas (canal, horário, resultado)
  - Sem dependência de serviços externos reais (usar fakes nos testes)
```

Salve em `.specify/specs/notificacao-fallback/spec.md`.

---

## 🔍 Fase 2: Clarify

### 2.1 Execute o clarify

```
/speckit.clarify

Sobre a spec de fallback de notificação:
1. Existe limite de tentativas por notificação?
2. Se todos os canais falharem, a notificação é descartada ou fica em fila para retry?
3. O intervalo entre tentativas é imediato ou tem um tempo de espera?
4. O log de tentativas é acessível para o usuário final ou só para o sistema?
5. Uma tentativa de fallback conta no limite de notificações diárias do usuário?
```

### 2.2 Revise e valide a spec

Cheque a spec com o **Checklist da Spec** do documento de boas práticas antes de avançar.

---

## 🏗️ Fase 3: Plan

### 3.1 Gerar o plano técnico

```
/speckit.plan specs/notificacao-fallback/spec.md
```

Salve em `.specify/specs/notificacao-fallback/plan.md`.

### 3.2 Revisar o plan

Perguntas para guiar a revisão:
- O plan propõe uma estratégia de retry pattern ou chain of responsibility?
- Como o plan sugere registrar as tentativas de fallback?
- A estratégia de testes cobre falhas simuladas em cada canal?
- Há alguma decisão técnica que você mudaria? Por quê?

> 💡 **Dica:** Se o plan escolheu uma abordagem e você preferiria outra, informe o Copilot: *"Prefiro usar Chain of Responsibility ao invés de lista linear. Ajuste o plan."*

---

## 📋 Fase 4: Tasks

### 4.1 Gerar as tarefas

```
/speckit.tasks specs/notificacao-fallback/plan.md
```

Salve em `.specify/specs/notificacao-fallback/tasks.md`.

### 4.2 Revisar as tarefas

Verifique:
- [ ] As tarefas estão em ordem de dependência (nenhuma depende de algo não criado)
- [ ] Cada tarefa tem critério claro de "feito"
- [ ] As tarefas de teste estão incluídas, não são um afterthought
- [ ] Você consegue estimar quanto tempo cada tarefa levaria

**Marque as primeiras 3 tarefas** que vai implementar neste exercício.

---

## ⚙️ Fase 5: Implement

### 5.1 Implementar tarefa por tarefa

Lembre-se: a implementação vai dentro do `projeto-base/src/MonitoriaBase/`. Para cada uma das 3 tarefas selecionadas, use:

```
/speckit.implement specs/notificacao-fallback/tasks.md

Implementar: [COLE AQUI O TEXTO DA TAREFA ESPECÍFICA]
```

**Fluxo para cada tarefa:**
1. Execute `/speckit.implement` referenciando a tarefa
2. Revise o código gerado — ele está alinhado com a spec?
3. Execute os testes (`dotnet test`)
4. Commite: `git commit -m "feat: [descrição da tarefa]"`
5. Marque a tarefa como concluída no `tasks.md`

### 5.2 Contexto persistente

O Copilot usa spec + plan como contexto em cada chamada de implement, gerando código coerente com o `MonitoriaBase` existente (nomes de namespace, padrão de interface, etc.). Compare:

```
❌ Sem SDD:
"Crie um método que tenta enviar a notificação por e-mail, e se falhar tenta por SMS"

✅ Com SDD:
/speckit.implement tasks.md
Tarefa: Implementar INotificationChannel com integração ao MonitoriaBase
```

O segundo gera código que respeita todas as decisões já tomadas (limites de tentativas, formato de log, padrão de namespace do projeto).

---

## 🔎 Fase 6: Analyze

### 6.1 Verificar consistência

```
/speckit.analyze specs/notificacao-fallback/
```

O Copilot verifica se:
- O código implementado reflete os critérios da spec
- O plan foi seguido na implementação
- As tasks concluídas estão alinhadas com o que foi implementado

### 6.2 Ajustar divergências encontradas

Se o analyze apontar divergências, decida: ajuste o código **ou** atualize a spec com justificativa. Nunca ignore uma divergência silenciosamente.

---

## ⚖️ Comparação: SDD vs Ad-hoc

### Experimento opcional (mas valioso)

Peça para um colega implementar a mesma feature **sem spec-kit**, usando apenas prompts diretos ao Copilot. Depois comparem:

| Critério | SDD | Ad-hoc |
|---|---|---|
| Cobertura de casos de erro | | |
| Consistência com requisitos | | |
| Qualidade da documentação gerada | | |
| Tempo total gasto | | |
| Confiança no resultado | | |

---

## ✅ Critérios de Sucesso

- [ ] `spec.md`, `plan.md` e `tasks.md` criados e consistentes entre si
- [ ] `/speckit.clarify` executado e spec refinada
- [ ] Pelo menos 3 tarefas implementadas usando `/speckit.implement`
- [ ] `dotnet test` passa para as tarefas implementadas
- [ ] `/speckit.analyze` executado sem divergências críticas
- [ ] Você consegue explicar qualquer decisão de código referenciando spec ou plan

---

## 🏆 Desafio Final

Implemente também a feature do Exercício 2 (avaliação e feedback) usando o ciclo completo das 6 fases. Você já tem a spec — agora gere o plan, as tasks e implemente no `projeto-base`. Use os artefatos de exemplo em `codigo-exemplo/exemplo-specify/` como referência de qualidade.

---

## 🎓 Conclusão do Módulo

Parabéns! Você passou pelo ciclo completo do Spec-Driven Development. Os principais aprendizados:

- Especificação antes de implementação **reduz retrabalho**
- `/speckit.clarify` é onde o real valor emerge — decisões implícitas viram explícitas
- Spec + plan como contexto persistente torna a IA mais precisa e consistente
- Artefatos versionados no git são documentação que não envelhece sozinha
