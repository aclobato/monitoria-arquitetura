# 🏗️ Código de Exemplo - Exercícios CI/CD

Esta pasta contém **código pronto** para os exercícios, permitindo que você **foque 100% na criação das pipelines**.

## 📁 Estrutura

```
codigo-exemplo/
├── exercicio-01/               # Pipeline básico
│   └── src/
│       ├── WebApi/             # Web API simples
│       └── WebApi.Tests/       # Testes unitários
├── exercicio-03-templates/     # Templates reutilizáveis
│   ├── templates/              # Templates YAML
│   │   ├── steps/             # Templates de steps
│   │   └── jobs/              # Templates de jobs
│   ├── projeto-a/             # Projeto exemplo A (Products API)
│   └── projeto-b/             # Projeto exemplo B (Orders API)
```

## 🎯 Como Usar

### Para qualquer exercício:
1. **📋 Copie o código** relevante para seu repositório Azure DevOps
2. **⚙️ Foque apenas na pipeline** - o código já funciona!
3. **🚀 Teste e implemente** sem perder tempo com desenvolvimento

## ✅ Benefícios

- ⏱️ **Economia de tempo** - sem codificação desnecessária
- 🎯 **Foco no CI/CD** - aprenda pipelines, não programação
- ✅ **Código testado** - projetos funcionais e com testes
- 🔄 **Reutilizável** - use para múltiplos exercícios

## 📚 Uso nos Exercícios

- **Exercício 1**: Use `exercicio-01/` - Web API básica
- **Exercício 2**: Reutilize `exercicio-01/` com deployment
- **Exercício 3**: Use `exercicio-03-templates/` - múltiplos projetos

## 🚀 Testando Localmente (Opcional)

```bash
cd exercicio-01/src/WebApi
dotnet run

# Em outro terminal
cd exercicio-01/src/WebApi.Tests  
dotnet test
```