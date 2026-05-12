# Plan: Fallback Automático de Canal de Notificação

**Feature:** notificacao-fallback  
**Baseado na spec:** spec.md v1.0

---

## Arquitetura de Componentes

```
NotificationService (orquestrador)
    │
    ├── INotificationChannel (abstração)
    │   ├── EmailChannel
    │   ├── SmsChannel
    │   └── PushChannel
    │
    ├── FallbackOrchestrator (lógica de fallback)
    │   └── Itera pelos canais em ordem de preferência
    │
    └── NotificationAttempt (value object)
        └── Canal + Horário + Resultado
```

---

## Decisões Técnicas

### 1. Padrão: Chain of Responsibility

**Decisão:** Implementar a lógica de fallback usando **Chain of Responsibility**, onde cada canal é um handler que tenta o envio e passa para o próximo em caso de falha.

**Justificativa:** 
- Cada canal é independente e não precisa conhecer os outros
- Novos canais podem ser adicionados sem alterar os existentes
- A ordem de tentativa é configurável sem mudar a lógica dos canais

**Alternativa considerada:** Lista linear com loop no `NotificationService`  
**Por que não:** Acoplaria o serviço à lógica de iteração, misturando responsabilidades.

---

### 2. Retorno: NotificationResult (value object)

**Decisão:** O método de envio retorna um `NotificationResult` com:
- `bool Success`
- `string? EffectiveChannel` (canal que funcionou, se algum)
- `IReadOnlyList<NotificationAttempt> Attempts`

**Justificativa:** Permite ao chamador inspecionar o que aconteceu sem precisar de exceções. Alinhado com CA-03 e CA-04.

**Alternativa considerada:** Lançar exceção em caso de falha total  
**Por que não:** Fallback total é um caso de negócio esperado, não uma condição excepcional.

---

### 3. Registro de Tentativas: Em memória (lista)

**Decisão:** As tentativas são acumuladas em uma lista durante o processamento e retornadas no `NotificationResult`.

**Justificativa:** A spec define que persistência em banco está fora do escopo. O chamador recebe o histórico e decide o que fazer com ele.

---

### 4. Canais: Verificação de disponibilidade antes de enviar

**Decisão:** `INotificationChannel` expõe `IsAvailable()` além de `Send()`. O orquestrador verifica disponibilidade antes de tentar o envio.

**Justificativa:** Permite que canais reportem indisponibilidade sem tentar um envio real (ex: configuração faltando, quota excedida).

---

## Diagrama de Fluxo

```
NotificationService.Send(message, userChannels)
    │
    ├─ Para cada canal em userChannels (em ordem):
    │   ├─ channel.IsAvailable()? → Não → registra tentativa INDISPONIVEL, próximo canal
    │   └─ channel.Send(message)  → Falha → registra tentativa FALHA, próximo canal
    │                              → Sucesso → registra tentativa SUCESSO, retorna resultado
    │
    └─ Todos falharam → retorna NotificationResult(Success=false, todas as tentativas)
```

---

## Interfaces

```csharp
public interface INotificationChannel
{
    string ChannelName { get; }
    bool IsAvailable();
    Task<bool> SendAsync(NotificationMessage message);
}

public record NotificationMessage(string Recipient, string Subject, string Body);

public record NotificationAttempt(string Channel, DateTime AttemptedAtUtc, bool Success, string? FailureReason);

public record NotificationResult(bool Success, string? EffectiveChannel, IReadOnlyList<NotificationAttempt> Attempts);
```

---

## Estratégia de Testes

| Tipo | O que testar |
|---|---|
| **Unitários** | Cada canal isolado (IsAvailable, Send), FallbackOrchestrator com canais fake |
| **Cenários de fallback** | 1° falha, 2° sucesso / todos falhando / 1° sucesso (sem fallback) |
| **Edge cases** | Lista de canais vazia, canal com IsAvailable=false, canal que lança exceção |

---

## Riscos e Mitigações

| Risco | Probabilidade | Mitigação |
|---|---|---|
| Canal lança exceção ao invés de retornar false | Alta | FallbackOrchestrator captura exceções e trata como falha |
| Canais adicionados no futuro com comportamentos diferentes | Média | Interface bem definida garante contrato |
| Latência acumulada em múltiplas tentativas | Baixa | CA-06 define limite de 100ms por tentativa; canais fake nos testes validam isso |
