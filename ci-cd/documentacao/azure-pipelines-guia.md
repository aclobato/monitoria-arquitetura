# Azure Pipelines - Guia Completo

## Estrutura de um Pipeline YAML

### Pipeline Básico
```yaml
# azure-pipelines.yml
trigger:
- main

pool:
  vmImage: 'ubuntu-latest'

variables:
  buildConfiguration: 'Release'

steps:
- task: UseDotNet@2
  inputs:
    version: '6.x'
    
- task: DotNetCoreCLI@2
  displayName: 'Restore packages'
  inputs:
    command: 'restore'
    projects: '**/*.csproj'

- task: DotNetCoreCLI@2
  displayName: 'Build project'
  inputs:
    command: 'build'
    projects: '**/*.csproj'
    arguments: '--configuration $(buildConfiguration)'

- task: DotNetCoreCLI@2
  displayName: 'Run tests'
  inputs:
    command: 'test'
    projects: '**/*Tests.csproj'
    arguments: '--configuration $(buildConfiguration) --collect "Code coverage"'
```

### Pipeline Multi-Stage
```yaml
trigger:
- main

variables:
  vmImageName: 'ubuntu-latest'
  buildConfiguration: 'Release'

stages:
- stage: Build
  displayName: 'Build and Test'
  jobs:
  - job: Build
    pool:
      vmImage: $(vmImageName)
    steps:
    - task: DotNetCoreCLI@2
      displayName: 'Build'
      inputs:
        command: 'build'
        arguments: '--configuration $(buildConfiguration)'
    
    - task: DotNetCoreCLI@2
      displayName: 'Test'
      inputs:
        command: 'test'
        arguments: '--configuration $(buildConfiguration) --logger trx --collect "Code coverage"'
    
    - task: PublishTestResults@2
      inputs:
        testResultsFormat: 'VSTest'
        testResultsFiles: '**/*.trx'

- stage: Deploy_Dev
  displayName: 'Deploy to Development'
  dependsOn: Build
  condition: succeeded()
  jobs:
  - deployment: DeployDev
    environment: 'development'
    pool:
      vmImage: $(vmImageName)
    strategy:
      runOnce:
        deploy:
          steps:
          - task: AzureWebApp@1
            inputs:
              azureSubscription: 'Azure-Connection'
              appName: 'myapp-dev'
              package: '$(Pipeline.Workspace)/**/*.zip'

- stage: Deploy_Prod
  displayName: 'Deploy to Production'
  dependsOn: Deploy_Dev
  condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))
  jobs:
  - deployment: DeployProd
    environment: 'production'
    pool:
      vmImage: $(vmImageName)
    strategy:
      runOnce:
        deploy:
          steps:
          - task: AzureWebApp@1
            inputs:
              azureSubscription: 'Azure-Connection'
              appName: 'myapp-prod'
              package: '$(Pipeline.Workspace)/**/*.zip'
```

## Templates e Reutilização

### Job Template
```yaml
# templates/build-template.yml
parameters:
- name: buildConfiguration
  type: string
  default: 'Release'
- name: projects
  type: string
  default: '**/*.csproj'

jobs:
- job: Build
  pool:
    vmImage: 'ubuntu-latest'
  steps:
  - task: DotNetCoreCLI@2
    displayName: 'Restore'
    inputs:
      command: 'restore'
      projects: ${{ parameters.projects }}
  
  - task: DotNetCoreCLI@2
    displayName: 'Build'
    inputs:
      command: 'build'
      projects: ${{ parameters.projects }}
      arguments: '--configuration ${{ parameters.buildConfiguration }}'
```

### Usando Templates
```yaml
# azure-pipelines.yml
trigger:
- main

extends:
  template: templates/build-template.yml
  parameters:
    buildConfiguration: 'Release'
    projects: 'src/**/*.csproj'
```

## Service Connections

### Azure Resource Manager
1. Project Settings → Service connections
2. New service connection → Azure Resource Manager
3. Service principal (automatic/manual)
4. Configurar escopo (Subscription/Resource Group)

### Container Registry
```yaml
resources:
  containers:
  - container: mycontainer
    image: myregistry.azurecr.io/myapp:latest
    endpoint: MyACRConnection

steps:
- task: Docker@2
  inputs:
    containerRegistry: 'MyACRConnection'
    command: 'build'
    Dockerfile: '**/Dockerfile'
    tags: |
      $(Build.BuildId)
      latest
```

## Environments e Approvals

### Environment Configuration
```yaml
- deployment: DeployToProduction
  environment: production
  strategy:
    runOnce:
      deploy:
        steps:
        - task: AzureWebApp@1
          inputs:
            azureSubscription: 'Azure-Connection'
            appName: 'myapp-prod'
```

### Manual Approval
1. Environments → Select environment
2. Approvals and checks → Add check
3. Approvals → Configure approvers

## Variable Groups e Key Vault

### Variable Group
```yaml
variables:
- group: 'MyVariableGroup'
- name: 'customVariable'
  value: 'customValue'

steps:
- script: echo "$(secretVariable)"
  displayName: 'Use secret variable'
```

### Azure Key Vault Integration
1. Library → Variable groups
2. Link secrets from Azure Key Vault
3. Configure Key Vault connection

```yaml
variables:
- group: 'KeyVaultSecrets'

steps:
- task: AzureKeyVault@2
  inputs:
    azureSubscription: 'Azure-Connection'
    KeyVaultName: 'MyKeyVault'
    SecretsFilter: '*'
    RunAsPreJob: true
```

## Agents e Self-Hosted

### Microsoft-Hosted Agents
```yaml
pool:
  vmImage: 'ubuntu-latest'  # ubuntu-20.04, windows-latest, macOS-latest
```

### Self-Hosted Agents
```yaml
pool:
  name: 'MyAgentPool'
  demands:
  - Agent.Name -equals MySpecificAgent
```

### Agent Installation
```bash
# Linux
wget https://vstsagentpackage.azureedge.net/agent/2.x.x/vsts-agent-linux-x64-2.x.x.tar.gz
tar zxvf vsts-agent-linux-x64-2.x.x.tar.gz
./config.sh
sudo ./svc.sh install
sudo ./svc.sh start
```

## Artifacts e Package Management

### Publishing Artifacts
```yaml
- task: PublishBuildArtifacts@1
  inputs:
    PathtoPublish: '$(Build.ArtifactStagingDirectory)'
    ArtifactName: 'drop'
    publishLocation: 'Container'
```

### NuGet Package
```yaml
- task: NuGetCommand@2
  inputs:
    command: 'pack'
    packagesToPack: '**/*.csproj'
    versioningScheme: 'byBuildNumber'

- task: NuGetCommand@2
  inputs:
    command: 'push'
    packagesToPush: '$(Build.ArtifactStagingDirectory)/**/*.nupkg'
    nuGetFeedType: 'internal'
    publishVstsFeed: 'MyFeed'
```

## Testing e Code Coverage

### Unit Tests
```yaml
- task: DotNetCoreCLI@2
  displayName: 'Run Unit Tests'
  inputs:
    command: 'test'
    projects: '**/*Tests.csproj'
    arguments: '--configuration $(buildConfiguration) --logger trx --collect "Code coverage"'

- task: PublishTestResults@2
  inputs:
    testResultsFormat: 'VSTest'
    testResultsFiles: '**/*.trx'
    mergeTestResults: true
```

### SonarCloud Integration
```yaml
- task: SonarCloudPrepare@1
  inputs:
    SonarCloud: 'SonarCloud'
    organization: 'myorg'
    scannerMode: 'MSBuild'
    projectKey: 'myproject'

- task: DotNetCoreCLI@2
  inputs:
    command: 'build'

- task: SonarCloudAnalyze@1

- task: SonarCloudPublish@1
  inputs:
    pollingTimeoutSec: '300'
```

## Conditional Logic

### Conditions
```yaml
steps:
- task: PowerShell@2
  condition: eq(variables['Build.SourceBranch'], 'refs/heads/main')
  inputs:
    script: 'Write-Host "Running on main branch"'

- task: PowerShell@2
  condition: and(succeeded(), eq(variables['Build.Reason'], 'Manual'))
  inputs:
    script: 'Write-Host "Manual build succeeded"'
```

### Custom Conditions
```yaml
- task: PowerShell@2
  condition: |
    and(
      succeeded(),
      or(
        eq(variables['Build.SourceBranch'], 'refs/heads/main'),
        startsWith(variables['Build.SourceBranch'], 'refs/heads/release/')
      )
    )
```

## Troubleshooting

### Debug Mode
```yaml
variables:
  system.debug: true
```

### Common Issues
1. **Permission errors**: Check service connection permissions
2. **Agent issues**: Verify agent capabilities and demands
3. **Variable scope**: Ensure variables are accessible in the right scope
4. **Template errors**: Validate YAML syntax and parameter passing