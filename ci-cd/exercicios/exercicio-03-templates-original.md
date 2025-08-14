# 🏗️ Exercício 3: Templates e Reutilização

## 🎯 Objetivo
Criar templates reutilizáveis para pipelines, promovendo consistência e manutenibilidade em múltiplos projetos.

## 📋 Cenário
Sua empresa tem múltiplas APIs .NET e precisa padronizar os pipelines. Você deve criar templates que podem ser reutilizados por diferentes equipes, mantendo flexibilidade para customizações específicas.

## ✅ Pré-requisitos
- 🏆 Exercícios 1 e 2 completados
- 📄 Conhecimento básico de YAML
- 📁 Múltiplos repositórios ou projetos para testar reutilização

## 📁 Parte 1: Estrutura de Templates

### 🏗️ 1.1 Criar Repositório de Templates

Crie um novo repositório chamado `azure-pipeline-templates` ou pasta `templates/` no repositório atual:

```
templates/
├── README.md
├── steps/
│   ├── dotnet-build.yml
│   ├── dotnet-test.yml
│   ├── dotnet-publish.yml
│   └── azure-deploy.yml
├── jobs/
│   ├── build-job.yml
│   ├── test-job.yml
│   └── deploy-job.yml
└── stages/
    ├── build-stage.yml
    ├── deploy-stage.yml
    └── full-pipeline.yml
```

### 📄 1.2 README para Templates

```markdown
# Azure Pipeline Templates

Templates reutilizáveis para pipelines Azure DevOps.

## Estrutura

- **steps/**: Templates de steps individuais
- **jobs/**: Templates de jobs completos  
- **stages/**: Templates de stages completos

## Como Usar

### Step Template
```yaml
steps:
- template: templates/steps/dotnet-build.yml
  parameters:
    buildConfiguration: 'Release'
```

### Job Template
```yaml
jobs:
- template: templates/jobs/build-job.yml
  parameters:
    vmImage: 'ubuntu-latest'
    buildConfiguration: 'Release'
```

### Stage Template
```yaml
stages:
- template: templates/stages/build-stage.yml
  parameters:
    vmImage: 'ubuntu-latest'
    buildConfiguration: 'Release'
```
```

## Parte 2: Step Templates

### 2.1 Template: dotnet-build.yml

```yaml
# templates/steps/dotnet-build.yml
parameters:
- name: buildConfiguration
  type: string
  default: 'Release'
- name: projects
  type: string
  default: '**/*.csproj'
- name: dotnetVersion
  type: string
  default: '6.x'
- name: restoreProjects
  type: string
  default: '**/*.csproj'

steps:
- task: UseDotNet@2
  displayName: 'Install .NET SDK ${{ parameters.dotnetVersion }}'
  inputs:
    version: ${{ parameters.dotnetVersion }}
    performMultiLevelLookup: true

- task: DotNetCoreCLI@2
  displayName: 'Restore NuGet packages'
  inputs:
    command: 'restore'
    projects: ${{ parameters.restoreProjects }}
    feedsToUse: 'select'

- task: DotNetCoreCLI@2
  displayName: 'Build ${{ parameters.buildConfiguration }}'
  inputs:
    command: 'build'
    projects: ${{ parameters.projects }}
    arguments: '--configuration ${{ parameters.buildConfiguration }} --no-restore'
```

### 2.2 Template: dotnet-test.yml

```yaml
# templates/steps/dotnet-test.yml
parameters:
- name: buildConfiguration
  type: string
  default: 'Release'
- name: testProjects
  type: string
  default: '**/*Tests.csproj'
- name: collectCoverage
  type: boolean
  default: true
- name: coverageThreshold
  type: number
  default: 80

steps:
- task: DotNetCoreCLI@2
  displayName: 'Run Unit Tests'
  inputs:
    command: 'test'
    projects: ${{ parameters.testProjects }}
    arguments: >-
      --configuration ${{ parameters.buildConfiguration }}
      --no-build
      --logger trx
      ${{ if parameters.collectCoverage }}:
        --collect "Code coverage"
        /p:CollectCoverage=true
        /p:CoverletOutputFormat=cobertura
        /p:CoverletOutput=$(Agent.TempDirectory)/coverage/

- task: PublishTestResults@2
  displayName: 'Publish Test Results'
  inputs:
    testResultsFormat: 'VSTest'
    testResultsFiles: '**/*.trx'
    mergeTestResults: true
    failTaskOnFailedTests: true
  condition: succeededOrFailed()

- ${{ if parameters.collectCoverage }}:
  - task: PublishCodeCoverageResults@1
    displayName: 'Publish Code Coverage'
    inputs:
      codeCoverageTool: 'cobertura'
      summaryFileLocation: '$(Agent.TempDirectory)/coverage/coverage.cobertura.xml'
      failIfCoverageEmpty: true
    condition: succeededOrFailed()

  - task: PowerShell@2
    displayName: 'Check Coverage Threshold'
    inputs:
      targetType: 'inline'
      script: |
        # Verificar se cobertura atende ao threshold
        $threshold = ${{ parameters.coverageThreshold }}
        Write-Host "Coverage threshold: $threshold%"
        
        # Aqui você adicionaria lógica para verificar cobertura
        # Por simplicidade, vamos apenas mostrar a mensagem
        Write-Host "✅ Coverage check completed (threshold: $threshold%)"
```

### 2.3 Template: azure-deploy.yml

```yaml
# templates/steps/azure-deploy.yml
parameters:
- name: azureServiceConnection
  type: string
- name: appName
  type: string
- name: environment
  type: string
  default: 'development'
- name: slotName
  type: string
  default: ''
- name: artifactName
  type: string
  default: 'drop'
- name: performSmokeTest
  type: boolean
  default: true
- name: smokeTestEndpoint
  type: string
  default: ''

steps:
- task: DownloadPipelineArtifact@2
  displayName: 'Download Build Artifacts'
  inputs:
    buildType: 'current'
    artifactName: ${{ parameters.artifactName }}
    targetPath: '$(Pipeline.Workspace)/${{ parameters.artifactName }}'

- task: AzureWebApp@1
  displayName: 'Deploy to Azure App Service'
  inputs:
    azureSubscription: ${{ parameters.azureServiceConnection }}
    appType: 'webApp'
    appName: ${{ parameters.appName }}
    ${{ if ne(parameters.slotName, '') }}:
      slotName: ${{ parameters.slotName }}
    package: '$(Pipeline.Workspace)/${{ parameters.artifactName }}/**/*.zip'
    deploymentMethod: 'auto'

- ${{ if parameters.performSmokeTest }}:
  - task: PowerShell@2
    displayName: 'Smoke Test - ${{ parameters.environment }}'
    inputs:
      targetType: 'inline'
      script: |
        $appName = "${{ parameters.appName }}"
        $slotName = "${{ parameters.slotName }}"
        $endpoint = "${{ parameters.smokeTestEndpoint }}"
        
        if ($slotName -ne "") {
            $url = "https://$appName-$slotName.azurewebsites.net"
        } else {
            $url = "https://$appName.azurewebsites.net"
        }
        
        if ($endpoint -ne "") {
            $url = "$url/$endpoint"
        }
        
        Write-Host "Testing endpoint: $url"
        
        $maxRetries = 5
        $delay = 10
        
        for ($i = 1; $i -le $maxRetries; $i++) {
            try {
                $response = Invoke-RestMethod -Uri $url -Method Get -TimeoutSec 30
                Write-Host "✅ Smoke test passed on attempt $i"
                Write-Host "Response: $($response | ConvertTo-Json -Depth 2)"
                break
            } catch {
                Write-Warning "❌ Attempt $i failed: $($_.Exception.Message)"
                if ($i -eq $maxRetries) {
                    Write-Error "Smoke test failed after $maxRetries attempts"
                    exit 1
                }
                Start-Sleep -Seconds $delay
            }
        }
```

## Parte 3: Job Templates

### 3.1 Template: build-job.yml

```yaml
# templates/jobs/build-job.yml
parameters:
- name: vmImage
  type: string
  default: 'ubuntu-latest'
- name: buildConfiguration
  type: string
  default: 'Release'
- name: dotnetVersion
  type: string
  default: '6.x'
- name: projects
  type: string
  default: '**/*.csproj'
- name: testProjects
  type: string
  default: '**/*Tests.csproj'
- name: publishProjects
  type: string
  default: ''
- name: artifactName
  type: string
  default: 'drop'

jobs:
- job: Build
  displayName: 'Build and Test'
  pool:
    vmImage: ${{ parameters.vmImage }}
  
  steps:
  # Build steps
  - template: ../steps/dotnet-build.yml
    parameters:
      buildConfiguration: ${{ parameters.buildConfiguration }}
      projects: ${{ parameters.projects }}
      dotnetVersion: ${{ parameters.dotnetVersion }}
  
  # Test steps
  - template: ../steps/dotnet-test.yml
    parameters:
      buildConfiguration: ${{ parameters.buildConfiguration }}
      testProjects: ${{ parameters.testProjects }}
      collectCoverage: true
      coverageThreshold: 80
  
  # Publish steps (if specified)
  - ${{ if ne(parameters.publishProjects, '') }}:
    - task: DotNetCoreCLI@2
      displayName: 'Publish Application'
      inputs:
        command: 'publish'
        publishWebProjects: false
        projects: ${{ parameters.publishProjects }}
        arguments: '--configuration ${{ parameters.buildConfiguration }} --output $(Build.ArtifactStagingDirectory)'
        zipAfterPublish: true
    
    - task: PublishPipelineArtifact@1
      displayName: 'Publish Pipeline Artifact'
      inputs:
        targetPath: '$(Build.ArtifactStagingDirectory)'
        artifactName: ${{ parameters.artifactName }}
        publishLocation: 'pipeline'
```

### 3.2 Template: deploy-job.yml

```yaml
# templates/jobs/deploy-job.yml
parameters:
- name: vmImage
  type: string
  default: 'ubuntu-latest'
- name: environment
  type: string
- name: azureServiceConnection
  type: string
- name: appName
  type: string
- name: artifactName
  type: string
  default: 'drop'
- name: slotName
  type: string
  default: ''
- name: smokeTestEndpoint
  type: string
  default: ''
- name: deploymentStrategy
  type: string
  default: 'runOnce'
  values:
  - runOnce
  - rolling
  - canary

jobs:
- deployment: Deploy
  displayName: 'Deploy to ${{ parameters.environment }}'
  environment: ${{ parameters.environment }}
  pool:
    vmImage: ${{ parameters.vmImage }}
  
  strategy:
    ${{ parameters.deploymentStrategy }}:
      deploy:
        steps:
        - template: ../steps/azure-deploy.yml
          parameters:
            azureServiceConnection: ${{ parameters.azureServiceConnection }}
            appName: ${{ parameters.appName }}
            environment: ${{ parameters.environment }}
            slotName: ${{ parameters.slotName }}
            artifactName: ${{ parameters.artifactName }}
            performSmokeTest: true
            smokeTestEndpoint: ${{ parameters.smokeTestEndpoint }}
```

## Parte 4: Stage Templates

### 4.1 Template: build-stage.yml

```yaml
# templates/stages/build-stage.yml
parameters:
- name: vmImage
  type: string
  default: 'ubuntu-latest'
- name: buildConfiguration
  type: string
  default: 'Release'
- name: stageName
  type: string
  default: 'Build'
- name: displayName
  type: string
  default: 'Build and Test'
- name: dotnetVersion
  type: string
  default: '6.x'
- name: projects
  type: string
  default: '**/*.csproj'
- name: testProjects
  type: string
  default: '**/*Tests.csproj'
- name: publishProjects
  type: string
  default: ''
- name: artifactName
  type: string
  default: 'drop'
- name: dependsOn
  type: object
  default: []
- name: condition
  type: string
  default: 'succeeded()'

stages:
- stage: ${{ parameters.stageName }}
  displayName: ${{ parameters.displayName }}
  ${{ if ne(length(parameters.dependsOn), 0) }}:
    dependsOn: ${{ parameters.dependsOn }}
  condition: ${{ parameters.condition }}
  
  jobs:
  - template: ../jobs/build-job.yml
    parameters:
      vmImage: ${{ parameters.vmImage }}
      buildConfiguration: ${{ parameters.buildConfiguration }}
      dotnetVersion: ${{ parameters.dotnetVersion }}
      projects: ${{ parameters.projects }}
      testProjects: ${{ parameters.testProjects }}
      publishProjects: ${{ parameters.publishProjects }}
      artifactName: ${{ parameters.artifactName }}
```

### 4.2 Template: deploy-stage.yml

```yaml
# templates/stages/deploy-stage.yml
parameters:
- name: environment
  type: string
- name: stageName
  type: string
- name: displayName
  type: string
- name: dependsOn
  type: object
  default: []
- name: condition
  type: string
  default: 'succeeded()'
- name: vmImage
  type: string
  default: 'ubuntu-latest'
- name: azureServiceConnection
  type: string
- name: appName
  type: string
- name: artifactName
  type: string
  default: 'drop'
- name: slotName
  type: string
  default: ''
- name: smokeTestEndpoint
  type: string
  default: ''
- name: deploymentStrategy
  type: string
  default: 'runOnce'
  values:
  - runOnce
  - rolling
  - canary

stages:
- stage: ${{ parameters.stageName }}
  displayName: ${{ parameters.displayName }}
  ${{ if ne(length(parameters.dependsOn), 0) }}:
    dependsOn: ${{ parameters.dependsOn }}
  condition: ${{ parameters.condition }}
  
  jobs:
  - template: ../jobs/deploy-job.yml
    parameters:
      vmImage: ${{ parameters.vmImage }}
      environment: ${{ parameters.environment }}
      azureServiceConnection: ${{ parameters.azureServiceConnection }}
      appName: ${{ parameters.appName }}
      artifactName: ${{ parameters.artifactName }}
      slotName: ${{ parameters.slotName }}
      smokeTestEndpoint: ${{ parameters.smokeTestEndpoint }}
      deploymentStrategy: ${{ parameters.deploymentStrategy }}
```

## Parte 5: Pipeline Completo com Templates

### 5.1 Novo azure-pipelines.yml usando templates

```yaml
# azure-pipelines.yml
trigger:
  branches:
    include:
    - main
  paths:
    include:
    - src/*

variables:
- name: buildConfiguration
  value: 'Release'
- name: azureServiceConnection
  value: 'Azure-Pipeline-Demo'
- name: webAppNameDev
  value: 'webapp-pipeline-demo-dev'
- name: webAppNameProd
  value: 'webapp-pipeline-demo-prod'

stages:
# Build Stage
- template: templates/stages/build-stage.yml
  parameters:
    stageName: 'Build'
    displayName: 'Build and Test Application'
    vmImage: 'ubuntu-latest'
    buildConfiguration: $(buildConfiguration)
    dotnetVersion: '6.x'
    projects: 'src/**/*.csproj'
    testProjects: 'src/**/*Tests.csproj'
    publishProjects: 'src/WebApi/WebApi.csproj'
    artifactName: 'WebApi'

# Deploy to Development
- template: templates/stages/deploy-stage.yml
  parameters:
    stageName: 'DeployDev'
    displayName: 'Deploy to Development'
    environment: 'development'
    dependsOn: ['Build']
    condition: succeeded()
    azureServiceConnection: $(azureServiceConnection)
    appName: $(webAppNameDev)
    artifactName: 'WebApi'
    smokeTestEndpoint: 'WeatherForecast'

# Deploy to Production
- template: templates/stages/deploy-stage.yml
  parameters:
    stageName: 'DeployProd'
    displayName: 'Deploy to Production'
    environment: 'production'
    dependsOn: ['DeployDev']
    condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))
    azureServiceConnection: $(azureServiceConnection)
    appName: $(webAppNameProd)
    artifactName: 'WebApi'
    smokeTestEndpoint: 'WeatherForecast'
```

### 5.2 Pipeline Simplificado para Outros Projetos

```yaml
# Exemplo para outro projeto - minimal-api-pipeline.yml
trigger:
- main

variables:
- name: buildConfiguration
  value: 'Release'

stages:
- template: templates/stages/build-stage.yml
  parameters:
    publishProjects: 'src/MinimalApi/MinimalApi.csproj'
    artifactName: 'MinimalApi'

- template: templates/stages/deploy-stage.yml
  parameters:
    stageName: 'DeployToTest'
    displayName: 'Deploy to Test Environment'
    environment: 'test'
    dependsOn: ['Build']
    azureServiceConnection: 'Azure-Test-Connection'
    appName: 'minimal-api-test'
    artifactName: 'MinimalApi'
```

## Parte 6: Extends Template

### 6.1 Template Master: full-pipeline.yml

```yaml
# templates/stages/full-pipeline.yml
parameters:
- name: buildConfiguration
  type: string
  default: 'Release'
- name: dotnetVersion
  type: string
  default: '6.x'
- name: vmImage
  type: string
  default: 'ubuntu-latest'
- name: projects
  type: string
  default: '**/*.csproj'
- name: testProjects
  type: string
  default: '**/*Tests.csproj'
- name: publishProjects
  type: string
- name: deployments
  type: object
  default: []

trigger:
  branches:
    include:
    - main

stages:
# Build Stage
- template: build-stage.yml
  parameters:
    buildConfiguration: ${{ parameters.buildConfiguration }}
    dotnetVersion: ${{ parameters.dotnetVersion }}
    vmImage: ${{ parameters.vmImage }}
    projects: ${{ parameters.projects }}
    testProjects: ${{ parameters.testProjects }}
    publishProjects: ${{ parameters.publishProjects }}

# Dynamic Deployment Stages
- ${{ each deployment in parameters.deployments }}:
  - template: deploy-stage.yml
    parameters:
      stageName: ${{ deployment.stageName }}
      displayName: ${{ deployment.displayName }}
      environment: ${{ deployment.environment }}
      dependsOn: ${{ deployment.dependsOn }}
      condition: ${{ deployment.condition }}
      azureServiceConnection: ${{ deployment.azureServiceConnection }}
      appName: ${{ deployment.appName }}
      smokeTestEndpoint: ${{ deployment.smokeTestEndpoint }}
```

### 6.2 Usando Extends Template

```yaml
# azure-pipelines-extends.yml
extends:
  template: templates/stages/full-pipeline.yml
  parameters:
    buildConfiguration: 'Release'
    dotnetVersion: '6.x'
    publishProjects: 'src/WebApi/WebApi.csproj'
    deployments:
    - stageName: 'DeployDev'
      displayName: 'Deploy to Development'
      environment: 'development'
      dependsOn: ['Build']
      condition: 'succeeded()'
      azureServiceConnection: 'Azure-Pipeline-Demo'
      appName: 'webapp-pipeline-demo-dev'
      smokeTestEndpoint: 'WeatherForecast'
    - stageName: 'DeployProd'
      displayName: 'Deploy to Production'
      environment: 'production'
      dependsOn: ['DeployDev']
      condition: "and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))"
      azureServiceConnection: 'Azure-Pipeline-Demo'
      appName: 'webapp-pipeline-demo-prod'
      smokeTestEndpoint: 'WeatherForecast'
```

## Parte 7: Validação e Testes

### 7.1 Validar Templates

```yaml
# validate-templates.yml - Pipeline para testar templates
trigger: none

pool:
  vmImage: 'ubuntu-latest'

steps:
- task: PowerShell@2
  displayName: 'Validate YAML Templates'
  inputs:
    targetType: 'inline'
    script: |
      # Encontrar todos os arquivos YAML de template
      $templates = Get-ChildItem -Path "templates" -Filter "*.yml" -Recurse
      
      foreach ($template in $templates) {
        Write-Host "Validating template: $($template.FullName)"
        
        # Aqui você pode adicionar validações específicas
        # Por exemplo, verificar se parâmetros obrigatórios existem
        $content = Get-Content $template.FullName -Raw
        
        if ($content -match "parameters:") {
          Write-Host "✅ Template has parameters section"
        } else {
          Write-Warning "⚠️  Template may be missing parameters section"
        }
        
        if ($content -match "steps:|jobs:|stages:") {
          Write-Host "✅ Template has valid Azure DevOps structure"
        } else {
          Write-Error "❌ Template missing valid structure"
        }
      }
      
      Write-Host "Template validation completed"
```

### 7.2 Testes de Integração

Crie diferentes pipelines para testar os templates:

```yaml
# test-templates-pipeline.yml
trigger: none

stages:
- template: templates/stages/build-stage.yml
  parameters:
    stageName: 'TestBuild'
    displayName: 'Test Build with Templates'
    projects: 'src/**/*.csproj'
    testProjects: 'src/**/*Tests.csproj'
    publishProjects: 'src/WebApi/WebApi.csproj'
```

## Parte 8: Versionamento e Distribuição

### 8.1 Versionamento de Templates

```yaml
# templates/version.yml
parameters:
- name: templateVersion
  type: string
  default: '1.0.0'

steps:
- task: PowerShell@2
  displayName: 'Template Version Info'
  inputs:
    script: |
      Write-Host "Using template version: ${{ parameters.templateVersion }}"
      Write-Host "Template last updated: $(Get-Date)"
```

### 8.2 Documentação de Mudanças

```markdown
# CHANGELOG.md

## [1.2.0] - 2024-01-15
### Added
- Support for deployment slots in azure-deploy.yml
- Coverage threshold parameter in dotnet-test.yml
- Extends template for full pipeline

### Changed
- Improved error handling in smoke tests
- Updated default .NET version to 6.x

### Fixed
- Fixed artifact download path in deploy templates

## [1.1.0] - 2023-12-01
### Added
- Job templates for build and deploy
- Stage templates with conditional logic
- Support for multiple deployment strategies
```

## Parte 9: Validação Final

### 9.1 Critérios de Sucesso

✅ **Templates funcionam corretamente**
- Step templates executam sem erro
- Job templates produzem artifacts esperados
- Stage templates deployam com sucesso

✅ **Reutilização é efetiva**
- Mesmo template usado em múltiplos pipelines
- Parametrização funciona corretamente
- Manutenção centralizada é possível

✅ **Documentação está completa**
- README explicando uso dos templates
- Parâmetros documentados
- Exemplos funcionais

### 9.2 Teste de Reutilização

1. Crie um segundo projeto/repositório
2. Use os templates criados
3. Customize apenas os parâmetros necessários
4. Verifique se o pipeline funciona sem modificações nos templates

## Resultado Esperado

Ao final deste exercício, você terá:
- ✅ Sistema completo de templates reutilizáveis
- ✅ Templates para steps, jobs e stages
- ✅ Pipeline parametrizado e flexível
- ✅ Documentação completa
- ✅ Capacidade de manter pipelines de forma centralizada
- ✅ Redução significativa de duplicação de código

**Tempo estimado**: 45-60 minutos

## Próximo Passo
Prossiga para o **Exercício 4: Integração com Azure Key Vault** para aprender sobre gerenciamento seguro de secrets e configurações.