# Spec: Fallback Automático de Canal de Notificação

**Feature:** notificacao-fallback  
**Versão:** 1.0  
**Status:** Aprovada

---

## Contexto e Problema

O Sistema de Notificações atual suporta três canais: e-mail, SMS e push. Cada usuário possui uma lista de canais preferidos em ordem de prioridade (ex: e-mail primeiro, SMS como alternativa).

**Problema atual:** quando o canal primário falha (servidor de e-mail fora do ar, número de telefone inválido), a notificação é descartada. O usuário não recebe a notificação e nem é informado da falha.

---

## Objetivo

Implementar lógica de **fallback automático**: se um canal falhar, o sistema tenta automaticamente o próximo canal na lista de preferências do usuário, até obter sucesso ou esgotar as opções.

---

## Atores

| Ator | Papel |
|---|---|
| **Usuário** | Configura seus canais preferidos e recebe notificações |
| **Sistema de Notificações** | Orquestra o envio e executa a lógica de fallback |
| **Canal de Notificação** | E-mail, SMS ou push — pode estar disponível ou não |

---

## Casos de Uso

### UC-01: Enviar notificação com fallback bem-sucedido

**Pré-condição:** Usuário tem dois canais configurados (ex: e-mail, SMS). Canal primário (e-mail) está indisponível.

**Fluxo:**
1. Sistema tenta enviar via e-mail
2. Canal de e-mail reporta falha
3. Sistema registra a tentativa falhada
4. Sistema tenta o próximo canal (SMS)
5. Canal de SMS confirma sucesso
6. Sistema registra tentativa bem-sucedida
7. Sistema retorna resultado com canal efetivo usado: SMS

**Resultado:** Usuário recebe a notificação via SMS. Resultado inclui qual canal foi usado.

---

### UC-02: Enviar notificação com todos os canais falhando

**Pré-condição:** Usuário tem dois canais configurados. Ambos estão indisponíveis.

**Fluxo:**
1. Sistema tenta cada canal em ordem
2. Todos os canais reportam falha
3. Sistema registra todas as tentativas falhadas
4. Sistema retorna resultado de falha total com lista de tentativas

**Resultado:** Notificação não enviada. Resultado contém histórico de tentativas para auditoria.

---

### UC-03: Enviar notificação quando canal primário está disponível

**Pré-condição:** Canal primário está disponível.

**Fluxo:**
1. Sistema tenta o canal primário
2. Canal confirma sucesso
3. Sistema não tenta outros canais

**Resultado:** Notificação enviada via canal primário. Sem tentativas adicionais.

---

### UC-04: Consultar histórico de tentativas

**Pré-condição:** Uma notificação foi processada (com ou sem fallback).

**Ator:** Sistema ou serviço de auditoria

**Resultado:** Lista de tentativas com canal, horário e resultado (sucesso/falha) para cada tentativa.

---

## Critérios de Aceite

| ID | Critério |
|---|---|
| CA-01 | O sistema tenta os canais na ordem exata de preferência configurada pelo usuário |
| CA-02 | Um canal que já falhou nesta notificação nunca é tentado novamente |
| CA-03 | O resultado do envio indica qual canal foi usado efetivamente (ou que todos falharam) |
| CA-04 | Cada tentativa (bem-sucedida ou não) é registrada com canal, horário UTC e resultado |
| CA-05 | O número máximo de tentativas é igual ao número de canais configurados para o usuário |
| CA-06 | A lógica de fallback não adiciona mais de 100ms de latência por tentativa adicional |
| CA-07 | A ausência de canais configurados resulta em falha imediata com mensagem descritiva |

---

## Fora do Escopo

- Notificação ao usuário sobre qual canal foi usado (isso é responsabilidade da camada de apresentação)
- Retry com intervalo de tempo (tentativas são imediatas, sem espera entre canais)
- Configuração dinâmica de canais em tempo de execução (canais são configurados no perfil do usuário)
- Persistência do histórico de tentativas em banco de dados (apenas em memória nesta versão)

---

## Questões Resolvidas (via clarify)

**P: Existe limite de tentativas?**  
R: O limite é o número de canais configurados para o usuário. Não existe limite separado.

**P: Se todos os canais falharem, a notificação fica em fila para retry?**  
R: Não. Nesta versão, a notificação é descartada. O chamador recebe o resultado de falha e decide o que fazer.

**P: O intervalo entre tentativas é imediato ou tem espera?**  
R: Imediato. Sem delay entre tentativas de canais diferentes.

**P: O log é acessível para o usuário final?**  
R: Não. O histórico de tentativas é retornado no resultado do envio e é responsabilidade do chamador expô-lo ou não.
