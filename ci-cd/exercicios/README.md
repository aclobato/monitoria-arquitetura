# 🏗️ Exercícios de CI/CD com Azure DevOps

Esta seção contém exercícios práticos para aplicar os conceitos de CI/CD usando Azure DevOps.

## 📝 Lista de Exercícios

### 🏗️ Exercício 1: Pipeline Básico
**🎯 Objetivo**: Criar um pipeline básico para uma aplicação .NET
- **📄 Arquivo**: `exercicio-01-pipeline-basico.md`
- **🌱 Nível**: Iniciante
- **⏱️ Duração**: 30-45 minutos

### 🏗️ Exercício 2: Pipeline Multi-Stage
**🎯 Objetivo**: Implementar pipeline com múltiplos estágios (Build, Test, Deploy)
- **📄 Arquivo**: `exercicio-02-pipeline-multi-stage.md`
- **📈 Nível**: Intermediário
- **⏱️ Duração**: 60-90 minutos

### 🏗️ Exercício 3: Templates e Reutilização
**🎯 Objetivo**: Criar templates reutilizáveis para pipelines
- **📄 Arquivo**: `exercicio-03-templates.md`
- **📈 Nível**: Intermediário
- **⏱️ Duração**: 45-60 minutos

### 🔐 Exercício 4: Integração com Azure Key Vault
**🎯 Objetivo**: Configurar segurança com Azure Key Vault
- **📄 Arquivo**: `exercicio-04-key-vault.md`
- **🏆 Nível**: Avançado
- **⏱️ Duração**: 60-75 minutos

### 🔵🟢 Exercício 5: Deploy com Blue-Green Strategy
**🎯 Objetivo**: Implementar estratégia Blue-Green deployment
- **📄 Arquivo**: `exercicio-05-blue-green.md`
- **🏆 Nível**: Avançado
- **⏱️ Duração**: 90-120 minutos

## ✅ Pré-requisitos

### 🔵 Conta Azure DevOps
1. Criar conta gratuita em [dev.azure.com](https://dev.azure.com)
2. Criar novo projeto
3. Configurar repositório Git

### ☁️ Azure Subscription (Para exercícios de deploy)
1. Conta gratuita do Azure
2. Criar Resource Group
3. Configurar App Service (para web apps)

### 🛠️ Ferramentas Locais
- Git
- Visual Studio Code
- Azure CLI (opcional)
- .NET SDK 6.0+

## 📚 Como Usar

1. **Escolha um exercício** baseado no seu nível de experiência
2. **Leia os pré-requisitos** específicos do exercício
3. **Siga o passo-a-passo** detalhado
4. **Valide o resultado** usando os critérios de sucesso
5. **Explore as extensões** sugeridas para aprofundar o conhecimento

## 💡 Dicas Gerais

### 🐛 Debugging
- Use `system.debug: true` para ver logs detalhados
- Verifique as permissões de service connections
- Valide a sintaxe YAML com extensões do VS Code

### ✨ Boas Práticas
- Versione sempre os arquivos de pipeline
- Use templates para evitar duplicação
- Configure notifications para falhas
- Documente mudanças significativas

### 📚 Recursos Úteis
- 📚 **[Referências para Estudo](../documentacao/03-referencias-estudo.md)** - Links completos para aprofundamento
- [Azure DevOps Documentation](https://docs.microsoft.com/azure/devops/) - Documentação oficial
- [YAML Schema Reference](https://docs.microsoft.com/azure/devops/pipelines/yaml-schema) - Referência YAML
- [Azure DevOps Labs](https://www.azuredevopslabs.com/) - Laboratórios práticos

## ➡️ Próximos Passos

Após completar os exercícios básicos, considere explorar:
- Infrastructure as Code com ARM Templates/Terraform
- Container Orchestration com AKS
- Monitoring e Application Insights
- Security scanning e compliance