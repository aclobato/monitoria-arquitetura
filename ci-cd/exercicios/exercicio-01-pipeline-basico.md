# 🏗️ Exercício 1: Pipeline Básico

## 🎯 Objetivo
Criar seu primeiro pipeline CI/CD no Azure DevOps para uma aplicação .NET Web API simples.

## 📋 Cenário
Você é desenvolvedor de uma equipe que precisa automatizar o processo de build e teste de uma Web API. O objetivo é criar um pipeline que execute build e testes automaticamente a cada commit na branch main.

## ✅ Pré-requisitos
- 🔵 Conta no Azure DevOps
- 📁 Projeto criado no Azure DevOps
- 🌿 Repositório Git configurado

## 📦 Código Pronto
**🎉 Boa notícia!** O código da aplicação já está pronto na pasta `codigo-exemplo/exercicio-01/`. 

Você pode **focar 100% na criação da pipeline** sem perder tempo escrevendo código!

## 📦 Parte 1: Preparação do Código

### 📂 1.1 Copiar Código de Exemplo

1. **📥 Copie** todo o conteúdo da pasta `codigo-exemplo/exercicio-01/` 
2. **📤 Cole** na raiz do seu repositório Azure DevOps
3. **✅ Verifique** a estrutura:

```
seu-repositorio/
└── src/
    ├── WebApi/              # Web API com controller
    │   ├── WebApi.csproj
    │   ├── Program.cs
    │   └── Controllers/
    │       └── WeatherForecastController.cs
    └── WebApi.Tests/        # Testes unitários  
        ├── WebApi.Tests.csproj
        └── WeatherForecastControllerTests.cs
```

### 🧪 1.2 Testar Localmente (Opcional)

```bash
# Testar se o código funciona
cd src/WebApi
dotnet run

# Em outro terminal - executar testes
cd src/WebApi.Tests  
dotnet test
```

**🎯 Agora você pode focar totalmente na criação da pipeline!**

## ⚙️ Parte 2: Criação do Pipeline

### 📄 2.1 Criar arquivo azure-pipelines.yml

Na raiz do repositório, crie o arquivo `azure-pipelines.yml`:

```yaml
# Pipeline básico para .NET Web API
trigger:
  branches:
    include:
    - main
  paths:
    include:
    - src/*

variables:
  buildConfiguration: 'Release'
  vmImageName: 'ubuntu-latest'

pool:
  vmImage: $(vmImageName)

steps:
# Instalar .NET SDK
- task: UseDotNet@2
  displayName: 'Install .NET 6 SDK'
  inputs:
    version: '6.x'
    performMultiLevelLookup: true

# Restaurar dependências
- task: DotNetCoreCLI@2
  displayName: 'Restore NuGet packages'
  inputs:
    command: 'restore'
    projects: 'src/**/*.csproj'
    feedsToUse: 'select'

# Build da aplicação
- task: DotNetCoreCLI@2
  displayName: 'Build application'
  inputs:
    command: 'build'
    projects: 'src/**/*.csproj'
    arguments: '--configuration $(buildConfiguration) --no-restore'

# Executar testes
- task: DotNetCoreCLI@2
  displayName: 'Run unit tests'
  inputs:
    command: 'test'
    projects: 'src/**/*Tests.csproj'
    arguments: '--configuration $(buildConfiguration) --no-build --logger trx --collect "Code coverage"'

# Publicar resultados dos testes
- task: PublishTestResults@2
  displayName: 'Publish test results'
  inputs:
    testResultsFormat: 'VSTest'
    testResultsFiles: '**/*.trx'
    mergeTestResults: true
    failTaskOnFailedTests: true
  condition: succeededOrFailed()

# Publicar aplicação
- task: DotNetCoreCLI@2
  displayName: 'Publish application'
  inputs:
    command: 'publish'
    publishWebProjects: false
    projects: 'src/WebApi/WebApi.csproj'
    arguments: '--configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory)'
    zipAfterPublish: true

# Publicar artefatos
- task: PublishBuildArtifacts@1
  displayName: 'Publish build artifacts'
  inputs:
    PathtoPublish: '$(Build.ArtifactStagingDirectory)'
    ArtifactName: 'WebApi-$(Build.BuildNumber)'
    publishLocation: 'Container'
```

## 🔵 Parte 3: Configuração no Azure DevOps

### 📤 3.1 Commit e Push do Código
```bash
git add .
git commit -m "Adicionar aplicação e pipeline inicial"
git push origin main
```

### ⚙️ 3.2 Configurar Pipeline no Azure DevOps

1. Acesse seu projeto no Azure DevOps
2. Navegue para **Pipelines** > **Create Pipeline**
3. Selecione **Azure Repos Git**
4. Escolha seu repositório
5. Selecione **Existing Azure Pipelines YAML file**
6. Selecione `/azure-pipelines.yml`
7. Clique em **Run**

## ✅ Parte 4: Validação

### 🏆 4.1 Critérios de Sucesso

✅ **Pipeline executa sem erros**
- Build completa com sucesso
- Testes são executados e passam
- Artefatos são publicados

✅ **Resultados dos testes são visíveis**
- Tab "Tests" mostra resultados
- Cobertura de código é reportada

✅ **Artefatos são gerados**
- Tab "Artifacts" contém o build publicado
- Arquivo ZIP da aplicação está disponível

### 🔍 4.2 Verificações Adicionais

1. **Trigger automático**: Faça um commit e verifique se o pipeline executa automaticamente
2. **Logs detalhados**: Examine os logs de cada step
3. **Duração**: Pipeline deve completar em menos de 5 minutos

## 🚀 Parte 5: Melhorias Opcionais

### 🏅 5.1 Adicionar Badge ao README
```markdown
[![Build Status](https://dev.azure.com/[organization]/[project]/_apis/build/status/[pipeline-name]?branchName=main)](https://dev.azure.com/[organization]/[project]/_build/latest?definitionId=[pipeline-id]&branchName=main)
```

### 🔔 5.2 Configurar Notifications
1. Project Settings > Notifications
2. New subscription > Build completion
3. Configurar email ou Teams

### 🛡️ 5.3 Branch Policies
1. Repos > Branches
2. Selecionar branch main > Branch policies
3. Habilitar "Require a minimum number of reviewers"
4. Adicionar "Check for linked work items"

## 🐛 Troubleshooting

### ⚠️ Problemas Comuns

**Erro: "No test result files matching *.trx were found"**
- Solução: Verificar se o projeto de teste está sendo executado corretamente

**Erro: ".NET SDK not found"**
- Solução: Verificar a versão do .NET SDK na task UseDotNet@2

**Pipeline não executa automaticamente**
- Solução: Verificar trigger configuration e branch names

### 🔧 Debug Tips

1. Adicionar variável `system.debug: true` para logs detalhados
2. Usar `displayName` em todos os steps para melhor rastreabilidade
3. Verificar permissões do service account

## 🎉 Resultado Esperado

Ao final deste exercício, você terá:
- ✅ Pipeline funcionando no Azure DevOps
- ✅ Build automatizado a cada commit
- ✅ Testes executados automaticamente
- ✅ Artefatos publicados
- ✅ Relatórios de teste visíveis na interface

⏱️ **Tempo estimado**: 30-45 minutos

## ➡️ Próximo Passo
Prossiga para o **Exercício 2: Pipeline Multi-Stage** para aprender sobre deployment automatizado.