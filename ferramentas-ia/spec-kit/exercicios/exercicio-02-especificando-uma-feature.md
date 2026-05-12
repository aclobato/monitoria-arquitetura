# 🟡 Exercício 2: Especificando uma Feature com spec-kit

**Dificuldade:** Intermediário | **Tempo estimado:** 60-90 min

---

## 🎯 Objetivo

Usar `/speckit.specify` e `/speckit.clarify` para transformar um requisito vago em uma spec detalhada e sem ambiguidades, entendendo o valor do refinamento iterativo.

---

## 📋 Cenário

O time de arquitetura precisa adicionar ao repositório de monitoria um **sistema de avaliação de exercícios**: monitores precisam conseguir registrar o progresso dos desenvolvedores nos exercícios e dar feedback estruturado.

Você receberá o requisito de forma vaga (como acontece na prática) e seu trabalho é transformá-lo em uma spec precisa usando o spec-kit.

---

## ✅ Pré-requisitos

- [ ] Exercício 1 concluído (spec-kit instalado e Copilot integrado)
- [ ] `.specify/memory/constitution.md` criada
- [ ] VS Code aberto no repositório de monitoria

---

## 🏋️ Parte 1: Especificando a Partir do Requisito Vago

### 1.1 O requisito inicial

Este é o requisito como você receberia na prática — vago e incompleto:

> *"Quero conseguir registrar que um aluno fez um exercício e dar um feedback pra ele."*

### 1.2 Criar a pasta da feature

```bash
mkdir .specify/specs/avaliacao-exercicios
```

### 1.3 Gerar a spec com Copilot

No Copilot Chat, execute:

```
/speckit.specify

FEATURE: Sistema de avaliação de exercícios da monitoria
CONTEXTO: Repositório educacional onde monitores acompanham desenvolvedores que praticam exercícios de arquitetura
REQUISITO INICIAL: Quero conseguir registrar que um aluno fez um exercício e dar um feedback pra ele
USUÁRIOS: Monitores (quem avalia) e Desenvolvedores (quem recebe feedback)
```

Salve o resultado em `.specify/specs/avaliacao-exercicios/spec.md`.

---

## 🔍 Parte 2: Identificando Ambiguidades

### 2.1 Leia a spec gerada

Leia o `spec.md` com atenção e anote:
- Que decisões a IA tomou sem você pedir?
- Que perguntas você teria se fosse implementar agora?
- Algum critério de aceite parece vago ou impossível de verificar?

### 2.2 Lista de ambiguidades a clarificar

Use as questões abaixo como ponto de partida (adicione as suas):

```
/speckit.clarify

Tenho as seguintes dúvidas sobre a spec de avaliação:

1. Como um monitor registra que um aluno "fez" um exercício?
   Só marcar como concluído ou precisa anexar evidência?

2. Qual o formato do feedback? Texto livre, notas (1-5), categorias fixas?

3. Um aluno pode refazer um exercício já avaliado? O que acontece com o feedback anterior?

4. Monitores podem editar feedbacks já enviados?

5. Alunos são notificados quando recebem feedback? Por qual canal?

6. Existe prazo para dar feedback após o exercício ser marcado como concluído?
```

### 2.3 Responder as perguntas levantadas pelo Copilot

O Copilot pode levantar perguntas adicionais. Responda com base no contexto da monitoria. Use seu julgamento — não existe resposta certa única aqui.

Atualize o `spec.md` com as decisões tomadas.

---

## 📊 Parte 3: Comparar Antes e Depois

### 3.1 Diff da spec

Se você salvou a spec original antes do clarify, abra os dois no VS Code com `Ctrl+D` para comparar.

**Questões para refletir:**
- Quantas decisões foram tomadas implicitamente na spec inicial?
- Alguma decisão tomada pelo Copilot você discordaria?
- O que teria acontecido se você tivesse implementado a spec inicial sem clarificar?

### 3.2 Teste de leitura

Peça para alguém que não participou do exercício ler sua spec final e responder:
- Quais são os 3 atores envolvidos?
- O que um monitor precisa fazer para registrar uma avaliação?
- O que acontece se um aluno refizer o exercício?

Se a pessoa conseguir responder sem ambiguidade, a spec está boa. Se não, volte ao `/speckit.clarify`.

---

## 💡 Parte 4: Reflexão sobre o Processo

Anote suas respostas (pode ser no próprio repositório ou no seu bloco de notas):

1. **Qual foi a ambiguidade mais importante** que o clarify revelou?
2. **Quanto tempo levou** o processo de especificação? Valeu a pena?
3. **O que teria ido para o código** se você não tivesse clarificado?
4. **Como esse processo muda** a conversa com o cliente/stakeholder?

---

## ✅ Critérios de Sucesso

- [ ] `spec.md` criada em `.specify/specs/avaliacao-exercicios/`
- [ ] `/speckit.clarify` executado ao menos uma vez com perguntas reais
- [ ] Spec atualizada com as decisões do clarify
- [ ] Todos os critérios de aceite são mensuráveis (sem "deve ser rápido", "deve ser fácil")
- [ ] O escopo está explicitamente definido (o que está fora)
- [ ] Você consegue explicar qualquer decisão na spec sem precisar "adivinhar"

---

## 🔍 Desafio Adicional

Tente fazer o mesmo exercício com um requisito de **outra área** que você conhece:
- Um sistema de agendamento de reuniões
- Uma feature de busca no repositório
- Um dashboard de progresso dos alunos

Compare: a spec ficou diferente de qualidade dependendo do domínio? Por quê?

---

## ➡️ Próximo Exercício

Com a spec validada, você está pronto para o ciclo completo:
**[Exercício 3: Ciclo Completo SDD →](./exercicio-03-ciclo-completo-sdd.md)**
