# 💡 Boas Práticas e Dicas para Maximizar o spec-kit

## ✍️ Como Escrever Boas Specs

### O nível certo de detalhe

Uma boa spec está no equilíbrio entre **muito vaga** (IA toma decisões que deveriam ser suas) e **muito detalhada** (você está especificando a implementação, não o comportamento).

| ❌ Vaga demais | ✅ No ponto certo | ❌ Detalhada demais |
|---|---|---|
| "Criar sistema de notificações" | "Sistema que envia notificações por e-mail e SMS, com retry automático em falha e histórico dos últimos 30 dias" | "Criar classe NotificationService com método SendAsync que chama HttpClient..." |

**Regra prática:** A spec deve descrever **o que o sistema faz** do ponto de vista do usuário. O **como** fica para o `plan.md`.

### O que sempre incluir na spec

- ✅ **Contexto do problema** — por que essa feature existe
- ✅ **Atores envolvidos** — quem usa e o que espera
- ✅ **Critérios de aceite** mensuráveis (evite "deve ser rápido", prefira "< 2 segundos")
- ✅ **Casos de erro** — o que acontece quando algo falha
- ✅ **Fora do escopo** — o que explicitamente não será feito (evita scope creep)

### O que evitar na spec

- ❌ Nomes de classes, métodos ou variáveis
- ❌ Detalhes de implementação (banco de dados, framework, algoritmo)
- ❌ Requisitos vagos sem critério de aceite ("ser fácil de usar", "ter boa performance")
- ❌ Assumir que o leitor conhece contexto não escrito

---

## ⚠️ Armadilhas Comuns

### 1. Pular o `/speckit.clarify`

**O problema:** A spec gerada pelo `/speckit.specify` quase sempre tem ambiguidades. Se você ir direto para o plan sem clarificar, essas decisões serão tomadas arbitrariamente pela IA.

**Como evitar:** Execute `/speckit.clarify` ao menos uma vez e responda todas as perguntas levantadas antes de avançar.

---

### 2. Revisar spec superficialmente

**O problema:** O Copilot gera uma spec que parece boa, você bate o olho, aprova, e só descobre o problema na implementação.

**Como evitar:** Leia a spec em voz alta (ou peça para um colega ler). Se algum trecho soar ambíguo, é ambíguo para a IA também.

---

### 3. Aprovar plan sem entender as decisões

**O problema:** O `plan.md` lista escolhas técnicas. Se você aceitar sem entender, vai ter dificuldade de fazer ajustes depois.

**Como evitar:** Para cada decisão técnica no plan, pergunte ao Copilot: *"Por que essa abordagem e não [alternativa]?"*. Entender os trade-offs é parte do processo.

---

### 4. Implementar fora do fluxo SDD

**O problema:** Você usou SDD para especificar, mas na implementação volta a fazer prompts ad-hoc sem referenciar spec e plan.

**Como evitar:** Sempre use `/speckit.implement` referenciando a tarefa específica do `tasks.md`. Isso garante que o contexto acumulado seja aproveitado.

---

### 5. Não versionar os artefatos

**O problema:** Os artefatos `.specify/` ficam locais e se perdem. Outro membro da equipe começa do zero.

**Como evitar:** Adicione `.specify/` ao git. Evolua os artefatos junto com o código. Trate spec e plan como documentação de primeira classe.

---

## 👥 Integrando SDD com o Time

### Spec como artefato de revisão

Antes de qualquer PR, inclua links para `spec.md` e `plan.md` na descrição. Isso transforma a revisão de código em uma **verificação de aderência à spec**, não uma discussão sobre estilo.

```markdown
## PR: Sistema de Notificações

📐 Spec: `.specify/specs/sistema-notificacoes/spec.md`
📋 Plan: `.specify/specs/sistema-notificacoes/plan.md`
✅ Tarefas concluídas: #1, #2, #3 (de 8)
```

### Revisando specs antes do código

Inclua **revisão de spec** como etapa do fluxo de trabalho da equipe:

```
Requisito → Spec (gerada) → Revisão da Spec → Plan → Tasks → Código → PR
                                ↑
                           Mais barato corrigir aqui
```

### Spec como documentação viva

Quando um requisito mudar, **atualize a spec primeiro** e depois o código. Use `/speckit.analyze` para identificar onde o código precisa mudar. Isso mantém spec e código sempre sincronizados.

---

## 🎯 Maximizando o GitHub Copilot com spec-kit

### Prompts que funcionam melhor

**Forneça contexto de negócio no `/speckit.specify`:**
```
/speckit.specify

CONTEXTO: Sistema de RH de empresa com 500 funcionários
PROBLEMA: Gestores perdem tempo enviando e-mails manuais de feedback
SOLICITADO: Sistema de feedback automático com gatilhos configuráveis
RESTRIÇÕES: Integrar com o sistema de RH legado (API REST disponível)
```

**Seja específico no `/speckit.clarify`:**
```
/speckit.clarify

Tenho dúvidas sobre:
1. O que acontece se o destinatário não existir no sistema de RH?
2. Feedback enviado fora do horário comercial: aguarda ou envia imediatamente?
3. Existe limite de feedbacks por período?
```

### Quando o resultado não satisfaz

Se o Copilot gerar algo que não atende, **não reescreva manualmente** — reformule o prompt:

```
❌ (Editar o arquivo diretamente e perder o contexto)

✅ /speckit.clarify
   O critério de aceite gerado para "tempo de entrega" não está mensurável.
   Preciso de um número concreto baseado no nosso SLA de 99.9% de uptime.
```

### Usando `/speckit.analyze` proativamente

Execute o analyze após qualquer mudança na spec ou no plan para capturar divergências antes que cheguem ao código:

```bash
# Após atualizar a spec
/speckit.analyze specs/minha-feature/
```

---

## 🔗 Combinando SDD com Outras Metodologias

### SDD + TDD

O SDD e TDD se complementam perfeitamente. Use a seguinte ordem:

1. Spec define **o que testar** (critérios de aceite viram testes)
2. Plan define **como estruturar** os testes (unitário, integração, e2e)
3. Tasks inclui tarefas de teste explícitas
4. Implemente o teste antes do código (TDD dentro de cada task)

```
spec.md (critérios de aceite) → testes de aceite (E2E)
plan.md (componentes)         → testes de integração
tasks.md (unidades de trabalho) → testes unitários
```

### SDD + BDD

Use spec.md como fonte para cenários Gherkin:

```
Spec: "Usuário sem permissão de admin não pode acessar relatórios"

↓ Vira BDD:

Dado que sou um usuário com perfil "colaborador"
Quando acesso a rota /relatorios
Então recebo status 403 com mensagem "Acesso não autorizado"
```

### SDD + DDD

Na fase de spec, use linguagem ubíqua do domínio. No plan, mapeie os termos da spec para entidades, value objects e agregados. Isso garante que o código reflita o modelo de domínio.

---

## ✅ Checklist: Spec e Plan Prontos para Implementar?

### Checklist da Spec
- [ ] Todos os casos de uso estão descritos com ator e objetivo
- [ ] Cada critério de aceite é mensurável (tem número, formato ou comportamento preciso)
- [ ] Casos de erro e exceções estão cobertos
- [ ] O escopo está explicitamente delimitado (o que está fora)
- [ ] Nenhum jargão técnico de implementação aparece
- [ ] `/speckit.clarify` foi executado e todas as ambiguidades resolvidas

### Checklist do Plan
- [ ] Todas as decisões técnicas têm justificativa
- [ ] A estratégia de testes está definida
- [ ] Integrações com sistemas externos estão identificadas
- [ ] Riscos e mitigações foram listados
- [ ] O plan não contradiz nenhum item da spec

### Checklist das Tasks
- [ ] As tarefas estão ordenadas por dependência (nenhuma task depende de algo não criado ainda)
- [ ] Cada task tem critério claro de "feito"
- [ ] Tarefas de teste estão incluídas, não são um adendo
- [ ] A estimativa de complexidade parece razoável

---

## 🚫 Quando NÃO Usar SDD

O SDD é valioso, mas tem custo. Use com parcimônia:

| Situação | Recomendação |
|---|---|
| **Hotfix urgente** em produção | ❌ Vá direto ao código. Documente depois. |
| **Protótipo descartável** ou spike técnico | ❌ O aprendizado é o objetivo, não o artefato. |
| **Mudança trivial** (renomear, ajustar texto, cor) | ❌ Overhead desnecessário. |
| **Feature com requisito claro** e baixa complexidade | ⚠️ Opcional. Uma spec simples pode ajudar. |
| **Feature média/alta complexidade** | ✅ SDD agrega muito valor. |
| **Feature que envolve múltiplos times** | ✅ A spec vira contrato entre times. |
| **Refatoração arquitetural** | ✅ Plan define a direção, tasks garantem progresso rastreável. |
