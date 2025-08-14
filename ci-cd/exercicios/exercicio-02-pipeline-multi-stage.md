# 🏗️ Exercício 2: Pipeline Multi-Stage

## 🎯 Objetivo
Expandir o pipeline básico para incluir múltiplos estágios com simulação de deployment para diferentes ambientes.

## 📋 Cenário
A equipe agora precisa de um pipeline mais robusto com múltiplos estágios. O pipeline deve ter estágios separados para Build, Testes de Qualidade, "Deploy" para Development e "Deploy" para Production (simulados).

## ✅ Pré-requisitos
- 🏆 Exercício 1 completado
- 📄 Conhecimento básico de YAML
- 📦 Código do exercício 1 no repositório

## 💡 Sobre este Exercício
**🎆 Foco no Aprendizado:** Este exercício simula deployments sem precisar de recursos Azure reais, permitindo que você aprenda os conceitos de pipelines multi-stage sem custos ou complexidades de infraestrutura.

## 📦 Parte 1: Preparação do Código

### 📄 1.1 Usar Código Existente

**🚀 Rápido:** Use o mesmo código do exercício 1 - você já tem tudo pronto!

1. ✅ **Certifique-se** de que o código do exercício 1 está no repositório
2. 📄 **Verifique** se o `azure-pipelines.yml` do exercício 1 funciona
3. 🎯 **Agora vamos evoluir** para multi-stage!

### 🧠 1.2 Conceitos que Vamos Aprender

- 🏗️ **Stages**: Organização lógica do pipeline
- 🛠️ **Jobs**: Unidades de execução paralela
- 📎 **Dependencies**: Controle de ordem de execução
- ⚙️ **Conditions**: Execução condicional
- 🌍 **Environments**: Controle de deployment (simulado)

## ⚙️ Parte 2: Pipeline Multi-Stage

### 📄 2.1 Criar Novo azure-pipelines-multi-stage.yml

💡 **Dica:** Crie um novo arquivo para não sobrescrever o exercício 1!

Crie o arquivo `azure-pipelines-multi-stage.yml`:

```yaml
# Pipeline multi-stage para .NET Web API (SIMULADO - SEM RECURSOS AZURE)
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
  majorVersion: '1'
  minorVersion: '0'
  patchVersion: $[counter(format('{0}.{1}', variables['majorVersion'], variables['minorVersion']), 0)]
  version: '$(majorVersion).$(minorVersion).$(patchVersion)'

stages:
# ===== BUILD STAGE =====
- stage: Build
  displayName: '🏗️ Build and Test'
  jobs:
  - job: BuildJob
    displayName: 'Build Application'
    pool:
      vmImage: $(vmImageName)
    steps:
    - task: UseDotNet@2
      displayName: '📦 Install .NET 6 SDK'
      inputs:
        version: '6.x'
        performMultiLevelLookup: true

    - task: DotNetCoreCLI@2
      displayName: '🔄 Restore packages'
      inputs:
        command: 'restore'
        projects: 'src/**/*.csproj'

    - task: DotNetCoreCLI@2
      displayName: '🏗️ Build application'
      inputs:
        command: 'build'
        projects: 'src/**/*.csproj'
        arguments: '--configuration $(buildConfiguration) --no-restore'

    - task: DotNetCoreCLI@2
      displayName: '🧪 Run unit tests'
      inputs:
        command: 'test'
        projects: 'src/**/*Tests.csproj'
        arguments: '--configuration $(buildConfiguration) --no-build --logger trx --collect "Code coverage"'

    - task: PublishTestResults@2
      displayName: '📊 Publish test results'
      inputs:
        testResultsFormat: 'VSTest'
        testResultsFiles: '**/*.trx'
        mergeTestResults: true
        failTaskOnFailedTests: true
      condition: succeededOrFailed()

    - task: DotNetCoreCLI@2
      displayName: '📦 Publish application'
      inputs:
        command: 'publish'
        publishWebProjects: false
        projects: 'src/WebApi/WebApi.csproj'
        arguments: '--configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory) --no-build'
        zipAfterPublish: true

    - task: PublishBuildArtifacts@1
      displayName: '📤 Publish artifacts'
      inputs:
        PathtoPublish: '$(Build.ArtifactStagingDirectory)'
        ArtifactName: 'WebApi-v$(version)'
        publishLocation: 'Container'

# ===== CODE QUALITY STAGE =====
- stage: CodeQuality
  displayName: '🔍 Code Quality'
  dependsOn: Build
  condition: succeeded()
  jobs:
  - job: QualityGates
    displayName: 'Quality Gates'
    pool:
      vmImage: $(vmImageName)
    steps:
    - task: UseDotNet@2
      displayName: 'Install .NET 6 SDK'
      inputs:
        version: '6.x'
        performMultiLevelLookup: true

    - script: |
        echo "🔍 Running static code analysis..."
        echo "Checking code coverage..."
        echo "Validating security rules..."
        echo "✅ All quality gates passed!"
      displayName: '🔍 Simulate Code Analysis'

    - script: |
        echo "📢 Quality Report:"
        echo "- Code Coverage: 85%"
        echo "- Security Issues: 0"
        echo "- Code Smells: 2 minor"
        echo "- Maintainability: A"
      displayName: '📊 Quality Report'

# ===== DEPLOY TO DEV STAGE =====
- stage: DeployDev
  displayName: '🚀 Deploy to Development'
  dependsOn: 
  - Build
  - CodeQuality
  condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))
  jobs:
  - deployment: DeployToDevJob
    displayName: 'Deploy to Dev Environment'
    environment: 'development'
    pool:
      vmImage: $(vmImageName)
    strategy:
      runOnce:
        deploy:
          steps:
          - script: |
              echo "🚀 Starting deployment to DEVELOPMENT..."
              echo "Environment: Development"
              echo "Version: $(version)"
              echo "Artifact: WebApi-v$(version)"
            displayName: '📋 Deployment Info'

          - script: |
              echo "📦 Downloading artifacts..."
              sleep 2
              echo "🔄 Extracting application..."
              sleep 1
              echo "⚙️ Configuring application..."
              sleep 1
              echo "🚀 Starting application..."
              sleep 2
              echo "✅ Deployment to Development completed successfully!"
            displayName: '🚀 Simulate Deployment'

          - script: |
              echo "🧪 Running smoke tests..."
              echo "Testing health endpoint..."
              echo "Testing API endpoints..."
              echo "✅ All smoke tests passed!"
            displayName: '🧪 Smoke Tests'

          - script: |
              echo "📢 Deployment Summary:"
              echo "Environment: Development"
              echo "Status: SUCCESS"
              echo "URL: https://webapp-demo-dev.azurewebsites.net (simulated)"
              echo "Deployed at: $(date)"
            displayName: '📊 Deployment Summary'

# ===== DEPLOY TO PROD STAGE =====
- stage: DeployProd
  displayName: '🎆 Deploy to Production'
  dependsOn: DeployDev
  condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))
  jobs:
  - deployment: DeployToProdJob
    displayName: 'Deploy to Production Environment'
    environment: 'production'
    pool:
      vmImage: $(vmImageName)
    strategy:
      runOnce:
        deploy:
          steps:
          - script: |
              echo "🎆 Starting deployment to PRODUCTION..."
              echo "Environment: Production"
              echo "Version: $(version)"
              echo "This deployment requires manual approval!"
            displayName: '📋 Production Deployment Info'

          - script: |
              echo "📦 Downloading production-ready artifacts..."
              sleep 3
              echo "🔄 Extracting application..."
              sleep 2
              echo "⚙️ Applying production configuration..."
              sleep 2
              echo "🔐 Setting up security..."
              sleep 1
              echo "🚀 Starting production deployment..."
              sleep 3
              echo "✅ Deployment to Production completed successfully!"
            displayName: '🎆 Simulate Production Deployment'

          - script: |
              echo "🧪 Running comprehensive tests..."
              echo "Testing all API endpoints..."
              echo "Validating performance..."
              echo "Checking security..."
              echo "Verifying integrations..."
              echo "✅ All production tests passed!"
            displayName: '🧪 Production Tests'

          - script: |
              echo "🎆 PRODUCTION DEPLOYMENT SUMMARY:"
              echo "Environment: Production"
              echo "Status: SUCCESS"
              echo "URL: https://webapp-demo-prod.azurewebsites.net (simulated)"
              echo "Version: $(version)"
              echo "Deployed at: $(date)"
              echo "🎉 Application is now live!"
            displayName: '🎉 Production Summary'
```

### 🏗️ 2.2 Entendendo a Estrutura Multi-Stage

**📊 Fluxo do Pipeline:**
1. **🏗️ Build Stage** - Compila, testa e gera artefatos
2. **🔍 Code Quality Stage** - Simula análise de qualidade
3. **🚀 Deploy Dev Stage** - "Deploya" para desenvolvimento
4. **🎆 Deploy Prod Stage** - "Deploya" para produção (com aprovação)

**📎 Dependencies e Conditions:**
- Stages só executam se o anterior teve sucesso
- Deploy só acontece na branch `main`
- Produção requer aprovação manual

**📦 Environments:**
- `development` - Deploy automático
- `production` - Requer aprovação manual

## 🔵 Parte 3: Configuração no Azure DevOps

### 📄 3.1 Criar Novo Pipeline

1. **Pipelines** > **New Pipeline**
2. **Azure Repos Git** > Selecione seu repositório
3. **Existing Azure Pipelines YAML file**
4. **Selecione** `/azure-pipelines-multi-stage.yml`
5. **Run**

### 🌍 3.2 Configurar Environments

**Para Development:**
1. **Environments** > **New environment**
2. Nome: `development`
3. **Create** (sem aprovações)

**Para Production:**
1. **Environments** > **New environment**
2. Nome: `production`
3. **Create** > **Approvals and checks**
4. **Approvals** > Adicionar você mesmo como aprovador

## ✅ Parte 4: Validação

### 🏆 4.1 Critérios de Sucesso

✅ **Pipeline executa todos os stages**
- Build completa com sucesso
- Quality stage simula análises
- Deploy Dev executa automaticamente
- Deploy Prod aguarda aprovação

✅ **Fluxo visual claro**
- Interface mostra progresso entre stages
- Dependencies funcionam corretamente
- Environments são criados

✅ **Aprovação manual funciona**
- Pipeline pausa antes de produção
- Notificação de aprovação é enviada
- Pode aprovar/rejeitar deployment

### 🔍 4.2 Verificações Adicionais

1. **📊 Visualização**: Pipeline mostra stages visualmente
2. **⏱️ Timing**: Cada stage executa na ordem correta
3. **🔔 Notificações**: Recebe email para aprovação de produção
4. **📈 Artefatos**: Artefatos são publicados com versionamento

## 🚀 Parte 5: Melhorias Opcionais

### 🔔 5.1 Configurar Notificações

1. **Project Settings** > **Notifications**
2. **New subscription** > **Run stage waiting for approval**
3. **Delivery options** > Email/Teams

### 📊 5.2 Adicionar Métricas

Adicione ao final do pipeline:

```yaml
# No final de cada deployment job
- script: |
    echo "📊 Pipeline Metrics:"
    echo "Total Duration: $SYSTEM_ELAPSEDTIME"
    echo "Build Time: $(buildTime)"
    echo "Deploy Time: $(deployTime)"
  displayName: '📊 Metrics'
```

### 🔐 5.3 Simular Rollback

Adicione stage de rollback:

```yaml
- stage: Rollback
  displayName: '⏪ Rollback (if needed)'
  dependsOn: DeployProd
  condition: failed()
  jobs:
  - job: RollbackJob
    steps:
    - script: |
        echo "⏪ ROLLBACK INITIATED"
        echo "Rolling back to previous version..."
        echo "✅ Rollback completed!"
      displayName: '⏪ Simulate Rollback'
```

## 🐛 Troubleshooting

### ⚠️ Problemas Comuns

**❌ Environment não encontrado:**
- Solução: Criar environments no Azure DevOps primeiro

**❌ Pipeline não executa stage seguinte:**
- Solução: Verificar conditions e dependencies

**❌ Aprovação não funciona:**
- Solução: Configurar approvers no environment

### 🔧 Debug Tips

1. Use `system.debug: true` para logs detalhados
2. Verifique conditions com `echo` statements
3. Teste cada stage independentemente

## 🎉 Resultado Esperado

Ao final deste exercício, você terá:
- ✅ Pipeline multi-stage funcionando
- ✅ Environments configurados (dev/prod)
- ✅ Aprovação manual para produção
- ✅ Fluxo visual de deployment
- ✅ Simulação realista sem custos Azure

⏱️ **Tempo estimado**: 45-60 minutos

## ➡️ Próximo Passo
Prossiga para o **Exercício 3: Templates** para aprender reutilização de código YAML!