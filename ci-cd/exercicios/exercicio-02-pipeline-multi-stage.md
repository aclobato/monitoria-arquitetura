# Exercício 2: Pipeline Multi-Stage

## Objetivo
Expandir o pipeline básico para incluir múltiplos estágios com deployment automático para ambientes de Development e Production.

## Cenário
A equipe agora precisa de deployment automatizado. O pipeline deve deployar automaticamente para Development após build/test bem-sucedidos, e para Production apenas quando aprovado manualmente.

## Pré-requisitos
- Exercício 1 completado
- Azure Subscription (conta gratuita serve)
- Resource Group criado no Azure
- Dois App Services criados (dev e prod)

## Parte 1: Preparação da Infraestrutura Azure

### 1.1 Criar Resource Group e App Services

```bash
# Login no Azure
az login

# Criar Resource Group
az group create --name rg-pipeline-demo --location eastus

# Criar App Service Plan
az appservice plan create --name asp-pipeline-demo --resource-group rg-pipeline-demo --sku F1

# Criar App Services
az webapp create --name webapp-pipeline-demo-dev --resource-group rg-pipeline-demo --plan asp-pipeline-demo
az webapp create --name webapp-pipeline-demo-prod --resource-group rg-pipeline-demo --plan asp-pipeline-demo
```

### 1.2 Configurar Service Connection no Azure DevOps

1. **Project Settings** > **Service connections**
2. **New service connection** > **Azure Resource Manager**
3. **Service principal (automatic)**
4. Selecione sua subscription e resource group
5. Nome: `Azure-Pipeline-Demo`

## Parte 2: Pipeline Multi-Stage

### 2.1 Atualizar azure-pipelines.yml

Substitua o conteúdo do arquivo `azure-pipelines.yml`:

```yaml
# Pipeline multi-stage para .NET Web API
trigger:
  branches:
    include:
    - main
  paths:
    include:
    - src/*

variables:
  # Build variables
  buildConfiguration: 'Release'
  vmImageName: 'ubuntu-latest'
  
  # Azure variables
  azureServiceConnection: 'Azure-Pipeline-Demo'
  webAppNameDev: 'webapp-pipeline-demo-dev'
  webAppNameProd: 'webapp-pipeline-demo-prod'

stages:
# ===== BUILD STAGE =====
- stage: Build
  displayName: 'Build and Test'
  jobs:
  - job: BuildJob
    displayName: 'Build Job'
    pool:
      vmImage: $(vmImageName)
    steps:
    # Install .NET SDK
    - task: UseDotNet@2
      displayName: 'Install .NET 6 SDK'
      inputs:
        version: '6.x'
        performMultiLevelLookup: true

    # Restore packages
    - task: DotNetCoreCLI@2
      displayName: 'Restore NuGet packages'
      inputs:
        command: 'restore'
        projects: 'src/**/*.csproj'

    # Build
    - task: DotNetCoreCLI@2
      displayName: 'Build application'
      inputs:
        command: 'build'
        projects: 'src/**/*.csproj'
        arguments: '--configuration $(buildConfiguration) --no-restore'

    # Run tests
    - task: DotNetCoreCLI@2
      displayName: 'Run unit tests'
      inputs:
        command: 'test'
        projects: 'src/**/*Tests.csproj'
        arguments: '--configuration $(buildConfiguration) --no-build --logger trx --collect "Code coverage"'

    # Publish test results
    - task: PublishTestResults@2
      displayName: 'Publish test results'
      inputs:
        testResultsFormat: 'VSTest'
        testResultsFiles: '**/*.trx'
        mergeTestResults: true
        failTaskOnFailedTests: true
      condition: succeededOrFailed()

    # Publish application
    - task: DotNetCoreCLI@2
      displayName: 'Publish application'
      inputs:
        command: 'publish'
        publishWebProjects: false
        projects: 'src/WebApi/WebApi.csproj'
        arguments: '--configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory)'
        zipAfterPublish: true

    # Upload artifacts for deployment stages
    - task: PublishPipelineArtifact@1
      displayName: 'Publish pipeline artifact'
      inputs:
        targetPath: '$(Build.ArtifactStagingDirectory)'
        artifactName: 'WebApi'
        publishLocation: 'pipeline'

# ===== DEVELOPMENT DEPLOYMENT STAGE =====
- stage: DeployDev
  displayName: 'Deploy to Development'
  dependsOn: Build
  condition: succeeded()
  variables:
    environmentName: 'Development'
  jobs:
  - deployment: DeployToDev
    displayName: 'Deploy to Development Environment'
    environment: 'development'
    pool:
      vmImage: $(vmImageName)
    strategy:
      runOnce:
        deploy:
          steps:
          # Download artifacts
          - task: DownloadPipelineArtifact@2
            displayName: 'Download build artifacts'
            inputs:
              buildType: 'current'
              artifactName: 'WebApi'
              targetPath: '$(Pipeline.Workspace)/WebApi'

          # Deploy to Azure App Service
          - task: AzureWebApp@1
            displayName: 'Deploy to Azure App Service'
            inputs:
              azureSubscription: '$(azureServiceConnection)'
              appType: 'webApp'
              appName: '$(webAppNameDev)'
              package: '$(Pipeline.Workspace)/WebApi/**/*.zip'
              deploymentMethod: 'auto'

          # Smoke test
          - task: PowerShell@2
            displayName: 'Smoke Test - Health Check'
            inputs:
              targetType: 'inline'
              script: |
                $url = "https://$(webAppNameDev).azurewebsites.net/WeatherForecast"
                Write-Host "Testing endpoint: $url"
                
                try {
                  $response = Invoke-RestMethod -Uri $url -Method Get -TimeoutSec 30
                  Write-Host "✅ Health check passed"
                  Write-Host "Response: $($response | ConvertTo-Json -Depth 2)"
                } catch {
                  Write-Error "❌ Health check failed: $($_.Exception.Message)"
                  exit 1
                }

# ===== PRODUCTION DEPLOYMENT STAGE =====
- stage: DeployProd
  displayName: 'Deploy to Production'
  dependsOn: DeployDev
  condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))
  variables:
    environmentName: 'Production'
  jobs:
  - deployment: DeployToProd
    displayName: 'Deploy to Production Environment'
    environment: 'production'
    pool:
      vmImage: $(vmImageName)
    strategy:
      runOnce:
        deploy:
          steps:
          # Download artifacts
          - task: DownloadPipelineArtifact@2
            displayName: 'Download build artifacts'
            inputs:
              buildType: 'current'
              artifactName: 'WebApi'
              targetPath: '$(Pipeline.Workspace)/WebApi'

          # Deploy to Azure App Service
          - task: AzureWebApp@1
            displayName: 'Deploy to Azure App Service'
            inputs:
              azureSubscription: '$(azureServiceConnection)'
              appType: 'webApp'
              appName: '$(webAppNameProd)'
              package: '$(Pipeline.Workspace)/WebApi/**/*.zip'
              deploymentMethod: 'auto'

          # Production smoke test
          - task: PowerShell@2
            displayName: 'Production Smoke Test'
            inputs:
              targetType: 'inline'
              script: |
                $url = "https://$(webAppNameProd).azurewebsites.net/WeatherForecast"
                Write-Host "Testing production endpoint: $url"
                
                try {
                  $response = Invoke-RestMethod -Uri $url -Method Get -TimeoutSec 30
                  Write-Host "✅ Production health check passed"
                  
                  # Additional production validations
                  if ($response.Count -eq 5) {
                    Write-Host "✅ Response contains expected number of items"
                  } else {
                    throw "Expected 5 items, got $($response.Count)"
                  }
                } catch {
                  Write-Error "❌ Production health check failed: $($_.Exception.Message)"
                  exit 1
                }

          # Post-deployment notification
          - task: PowerShell@2
            displayName: 'Post-Deployment Notification'
            inputs:
              targetType: 'inline'
              script: |
                Write-Host "🚀 Deployment to Production completed successfully!"
                Write-Host "Application URL: https://$(webAppNameProd).azurewebsites.net"
                Write-Host "Build Number: $(Build.BuildNumber)"
                Write-Host "Deployed by: $(Build.RequestedFor)"
```

## Parte 3: Configurar Environments

### 3.1 Criar Environment Development

1. **Pipelines** > **Environments**
2. **New environment**
3. Nome: `development`
4. Description: `Development environment for automatic deployments`
5. **Create**

### 3.2 Criar Environment Production com Approval

1. **New environment**
2. Nome: `production`
3. Description: `Production environment with manual approval`
4. **Create**
5. Clique no environment `production`
6. **Approvals and checks** > **Approvals**
7. **Add** > Selecione aprovadores
8. Configurar:
   - **Approvers**: Seu usuário
   - **Timeout**: 30 days
   - **Instructions**: "Please review deployment to production"

## Parte 4: Configurações Avançadas

### 4.1 Adicionar Variable Group

1. **Pipelines** > **Library**
2. **Variable groups** > **+ Variable group**
3. Nome: `WebApi-Config`
4. Adicionar variáveis:
   ```
   ApiVersion: v1.0
   Environment.Dev: Development
   Environment.Prod: Production
   LogLevel: Information
   ```
5. **Save**

### 4.2 Atualizar Pipeline para usar Variable Group

Adicione no início do arquivo YAML:
```yaml
variables:
- group: 'WebApi-Config'
- name: buildConfiguration
  value: 'Release'
# ... resto das variáveis
```

## Parte 5: Deployment Slots (Opcional)

### 5.1 Configurar Deployment Slots

```yaml
# Adicionar antes do deploy para produção
- task: AzureAppServiceManage@0
  displayName: 'Create staging slot'
  inputs:
    azureSubscription: '$(azureServiceConnection)'
    Action: 'Create or update Deployment Slot'
    WebAppName: '$(webAppNameProd)'
    ResourceGroupName: 'rg-pipeline-demo'
    Slot: 'staging'

# Deploy para staging slot
- task: AzureWebApp@1
  displayName: 'Deploy to staging slot'
  inputs:
    azureSubscription: '$(azureServiceConnection)'
    appType: 'webApp'
    appName: '$(webAppNameProd)'
    slotName: 'staging'
    package: '$(Pipeline.Workspace)/WebApi/**/*.zip'

# Swap slots após validação
- task: AzureAppServiceManage@0
  displayName: 'Swap with production'
  inputs:
    azureSubscription: '$(azureServiceConnection)'
    Action: 'Swap Slots'
    WebAppName: '$(webAppNameProd)'
    ResourceGroupName: 'rg-pipeline-demo'
    SourceSlot: 'staging'
```

## Parte 6: Validação

### 6.1 Executar Pipeline

1. Commit e push das mudanças
2. Verificar execução do pipeline
3. Aprovar deployment para produção quando solicitado

### 6.2 Critérios de Sucesso

✅ **Build Stage completa com sucesso**
- Todos os testes passam
- Artefatos são publicados

✅ **Development deployment é automático**
- Deploy acontece automaticamente após build
- Smoke test passa
- Aplicação está acessível

✅ **Production deployment requer aprovação**
- Stage aguarda aprovação manual
- Após aprovação, deploy completa com sucesso
- Smoke tests de produção passam

✅ **Environments são criados corretamente**
- Environment development existe
- Environment production tem approval configurado

### 6.3 Testar Endpoints

```bash
# Development
curl https://webapp-pipeline-demo-dev.azurewebsites.net/WeatherForecast

# Production (após deployment)
curl https://webapp-pipeline-demo-prod.azurewebsites.net/WeatherForecast
```

## Parte 7: Monitoramento e Insights

### 7.1 Configurar Release Annotations

Adicione após o deployment:
```yaml
- task: PowerShell@2
  displayName: 'Create Release Annotation'
  inputs:
    targetType: 'inline'
    script: |
      # Criar anotação para Application Insights (se configurado)
      Write-Host "Release $(Build.BuildNumber) deployed to $(environmentName)"
      Write-Host "Deployment time: $(Get-Date)"
```

### 7.2 Dashboard de Pipeline

1. **Dashboards** > **New Dashboard**
2. Adicionar widgets:
   - Pipeline status
   - Test results
   - Deployment status

## Troubleshooting

### Problemas Comuns

**Service Connection não funciona**
- Verificar permissões no Azure
- Regenerar service principal se necessário

**App Service deployment falha**
- Verificar se App Service existe
- Verificar configurações de deployment

**Approval não aparece**
- Verificar configuração do environment
- Verificar permissões de aprovação

### Logs Importantes

```yaml
# Adicionar para debug
- task: PowerShell@2
  displayName: 'Debug Information'
  inputs:
    script: |
      Write-Host "Build ID: $(Build.BuildId)"
      Write-Host "Source Branch: $(Build.SourceBranch)"
      Write-Host "Agent: $(Agent.Name)"
      Write-Host "Pipeline Workspace: $(Pipeline.Workspace)"
```

## Resultado Esperado

Ao final deste exercício, você terá:
- ✅ Pipeline com 3 stages (Build, Deploy Dev, Deploy Prod)
- ✅ Deployment automático para Development
- ✅ Deployment manual com aprovação para Production
- ✅ Smoke tests funcionando
- ✅ Environments configurados corretamente
- ✅ Aplicações funcionando no Azure

**Tempo estimado**: 60-90 minutos

## Próximo Passo
Prossiga para o **Exercício 3: Templates e Reutilização** para aprender sobre organização e reutilização de código de pipeline.