# Tasks: Fallback Automático de Canal de Notificação

**Feature:** notificacao-fallback  
**Baseado no plan:** plan.md v1.0  
**Total de tarefas:** 8

---

## Legenda

- ⬜ Pendente
- 🔄 Em progresso
- ✅ Concluída

---

## Tarefas

### Tarefa 1 — Criar modelos de domínio
**Status:** ⬜ Pendente  
**Dependências:** Nenhuma  
**Estimativa:** Pequena (< 30 min)

Criar os value objects e records definidos no plan:
- `NotificationMessage(Recipient, Subject, Body)`
- `NotificationAttempt(Channel, AttemptedAtUtc, Success, FailureReason?)`
- `NotificationResult(Success, EffectiveChannel?, Attempts)`

**Critério de conclusão:** Os tipos compilam sem erros e têm testes básicos de igualdade estrutural (records).

---

### Tarefa 2 — Criar interface `INotificationChannel`
**Status:** ⬜ Pendente  
**Dependências:** Tarefa 1  
**Estimativa:** Pequena (< 30 min)

Criar a interface `INotificationChannel` com:
- `string ChannelName { get; }`
- `bool IsAvailable()`
- `Task<bool> SendAsync(NotificationMessage message)`

**Critério de conclusão:** Interface criada. Nenhuma implementação ainda — apenas o contrato.

---

### Tarefa 3 — Implementar canal fake para testes
**Status:** ⬜ Pendente  
**Dependências:** Tarefa 2  
**Estimativa:** Pequena (< 30 min)

Criar `FakeNotificationChannel` para uso em testes:
- Recebe via construtor: `channelName`, `isAvailable`, `sendSucceeds`
- Registra chamadas recebidas (para verificar em testes)
- Não faz IO real

**Critério de conclusão:** `FakeNotificationChannel` pode simular canal disponível/indisponível e envio bem-sucedido/falhado.

---

### Tarefa 4 — Implementar `FallbackOrchestrator`
**Status:** ⬜ Pendente  
**Dependências:** Tarefas 1, 2, 3  
**Estimativa:** Média (30-60 min)

Implementar a lógica central de fallback:
- Itera pelos canais na ordem fornecida
- Para cada canal: verifica `IsAvailable()` → tenta `SendAsync()` → registra `NotificationAttempt`
- Captura exceções dos canais e trata como falha (não propaga)
- Retorna `NotificationResult` com todas as tentativas e canal efetivo (se algum funcionou)

**Critério de conclusão:** Testes cobrindo os 3 cenários do UC-01, UC-02 e UC-03 passam.

---

### Tarefa 5 — Testes de edge cases do `FallbackOrchestrator`
**Status:** ⬜ Pendente  
**Dependências:** Tarefa 4  
**Estimativa:** Pequena (< 30 min)

Cobrir os edge cases:
- Lista de canais vazia → `NotificationResult(Success=false, Attempts=[])`
- Canal lança exceção em `SendAsync` → tratado como falha, próximo canal tentado
- Canal com `IsAvailable=false` → não chama `SendAsync`, registra como INDISPONIVEL

**Critério de conclusão:** Todos os edge cases têm teste e passam.

---

### Tarefa 6 — Implementar `EmailChannel`
**Status:** ⬜ Pendente  
**Dependências:** Tarefa 2  
**Estimativa:** Pequena (< 30 min)

Implementar `EmailChannel` que implementa `INotificationChannel`:
- `IsAvailable()`: verifica se configuração de SMTP está presente
- `SendAsync()`: simula envio (não precisa de SMTP real nesta versão — usar `Task.Delay` para simular latência)
- `ChannelName`: "email"

**Critério de conclusão:** `EmailChannel` implementa a interface. Teste de unidade confirma comportamento quando configuração ausente (`IsAvailable=false`) e presente (`IsAvailable=true`).

---

### Tarefa 7 — Implementar `SmsChannel` e `PushChannel`
**Status:** ⬜ Pendente  
**Dependências:** Tarefa 2  
**Estimativa:** Pequena (< 30 min)

Implementar seguindo o mesmo padrão do `EmailChannel`:
- `SmsChannel` com `ChannelName = "sms"`
- `PushChannel` com `ChannelName = "push"`

**Critério de conclusão:** Os dois canais implementam a interface e têm testes básicos.

---

### Tarefa 8 — Integrar no `NotificationService`
**Status:** ⬜ Pendente  
**Dependências:** Tarefas 4, 6, 7  
**Estimativa:** Pequena (< 30 min)

Atualizar o `NotificationService` para:
- Receber lista de `INotificationChannel` via construtor (injeção de dependência)
- Delegar ao `FallbackOrchestrator` a lógica de fallback
- Expor método `SendAsync(NotificationMessage, IEnumerable<string> preferredChannels)` que:
  - Ordena os canais conforme as preferências do usuário
  - Chama o orquestrador

**Critério de conclusão:** Teste de integração usando `FakeNotificationChannel` confirma que `NotificationService` respeita a ordem de preferência do usuário.

---

## Ordem de execução recomendada

```
1 → 2 → 3 → 4 → 5 → 6 → 7 → 8
            ↑
       Valide aqui antes de continuar
       (testes do FallbackOrchestrator devem passar)
```

---

## Verificação final

Após concluir todas as tarefas:

```bash
dotnet test

# Resultado esperado: todos os testes passando, cobertura dos cenários UC-01, UC-02 e UC-03
```

Execute também:
```
/speckit.analyze specs/notificacao-fallback/
```

Para confirmar que a implementação está alinhada com spec e plan.
