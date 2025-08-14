# ⚙️ Azure Pipelines - Guia Completo

🎆 **Bem-vindo ao mundo dos Azure Pipelines!** Este guia irá transformá-lo de iniciante a especialista em pipelines YAML. Prepare-se para uma jornada emocionante pelo universo da automação! 🚀

📝 **O que você vai aprender:**
- 🏗️ Como estruturar pipelines profissionais
- 📄 YAML do básico ao avançado
- 🔗 Integrações com Azure e ferramentas externas
- 🔧 Troubleshooting e otimizações

## 📄 Estrutura de um Pipeline YAML

🤔 **YAML**: Yet Another Markup Language (ou YAML Ain't Markup Language) é uma linguagem de serialização de dados human-friendly. No Azure DevOps, usamos YAML para definir nossos pipelines como código!

✨ **Vantagens do YAML**:
- 📂 Versionamento junto com o código
- 👥 Code review para mudanças no pipeline
- 🔄 Reutilização através de templates
- 📝 Auto-documentação

### 🏁 Pipeline Básico

🔍 **Anatomia de um pipeline simples**: Vamos começar com o exemplo mais básico possível para construir uma aplicação .NET.
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

### 🏆 Pipeline Multi-Stage

🎆 **Evoluindo para profissional**: Agora vamos criar um pipeline completo com múltiplos estágios que representa um workflow real de desenvolvimento.

📈 **Benefícios dos Multi-Stage Pipelines**:
- 🔀 Fluxo visual claro
- 🚪 Controle de aprovações
- 🔄 Dependências entre estágios
- 🌍 Deploy em múltiplos ambientes
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

## 🗂️ Templates e Reutilização

🎨 **DRY Principle**: Don't Repeat Yourself! Templates são a chave para manter seus pipelines organizados, consistentes e fáceis de manter.

🔥 **Por que usar templates**:
- ♾️ Evita duplicação de código
- 🔄 Padronização entre projetos
- 🛠️ Manutenção centralizada
- 🏆 Boas práticas enforced

### 🎯 Job Template

💻 **Criando seu primeiro template**: Um template de job é perfeito para padronizar steps de build que serão reutilizados.
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

### 🚀 Usando Templates

🔗 **Conectando as peças**: Agora vamos ver como usar o template que criamos acima.
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

## 🔗 Service Connections

🌉 **A ponte para o mundo externo**: Service Connections são a forma segura de conectar seus pipelines com serviços externos como Azure, AWS, Docker registries, etc.

🔐 **Segurança em primeiro lugar**: As connections usam autenticação baseada em Service Principal, garantindo que credenciais nunca sejam expostas.

### ☁️ Azure Resource Manager

🎯 **Setup passo-a-passo**:
1. Project Settings → Service connections
2. New service connection → Azure Resource Manager
3. Service principal (automatic/manual)
4. Configurar escopo (Subscription/Resource Group)

### 📦 Container Registry

🐳 **Para o mundo dos containers**: Configurando conexão com Azure Container Registry para builds de Docker.
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

## 🌍 Environments e Approvals

🚪 **Governança e Controle**: Environments permitem controlar exatamente quando e como deployments acontecem, adicionando layers de segurança e compliance.

✨ **Recursos dos Environments**:
- 👥 Aprovações manuais
- 📋 Check gates automatizados
- 📈 Histórico de deployments
- 🔒 Controle de acesso

### ⚙️ Environment Configuration

🎯 **Referenciando environments**: Como usar environments em seus deployment jobs.
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

### 👍 Manual Approval

🚪 **Configurando aprovações**: Passo-a-passo para adicionar aprovações manuais.
1. Environments → Select environment
2. Approvals and checks → Add check
3. Approvals → Configure approvers

## 📋 Variable Groups e Key Vault

🔑 **Gerenciamento Seguro de Configurações**: Variables Groups centralizam configurações e secrets, enquanto Key Vault fornece segurança enterprise-grade.

🎆 **Benefícios**:
- 🔄 Reutilização entre pipelines
- 🔐 Segurança aprimorada
- 🔧 Gestão centralizada
- 📋 Organização por ambiente

### 📋 Variable Group

🔗 **Vinculando Variable Groups**: Como referenciar seus grupos de variáveis.
```yaml
variables:
- group: 'MyVariableGroup'
- name: 'customVariable'
  value: 'customValue'

steps:
- script: echo "$(secretVariable)"
  displayName: 'Use secret variable'
```

### 🔐 Azure Key Vault Integration

🎯 **Integração enterprise**: Configurando Key Vault para segurança máxima.
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

## 🤖 Agents e Self-Hosted

🏗️ **Os trabalhadores incansáveis**: Agents são as máquinas que executam seus pipelines. Você pode usar agents da Microsoft (hosted) ou seus próprios (self-hosted).

🤔 **Quando usar cada tipo**:
- **☁️ Microsoft-hosted**: Rápido para começar, sem manutenção
- **🏠 Self-hosted**: Controle total, softwares específicos, performance

### ☁️ Microsoft-Hosted Agents

🚀 **Plug-and-play**: Agents gerenciados pela Microsoft, sempre atualizados.

📋 **Agents disponíveis**:
- `ubuntu-latest` (Ubuntu 22.04) - 🐧 Mais rápido para builds
- `windows-latest` (Windows Server 2022) - 💻 Para .NET Framework
- `macOS-latest` (macOS 12) - 🍎 Para apps iOS/macOS
```yaml
pool:
  vmImage: 'ubuntu-latest'  # ubuntu-20.04, windows-latest, macOS-latest
```

### 🏠 Self-Hosted Agents

🔧 **Controle total**: Seus próprios agents com configurações personalizadas.
```yaml
pool:
  name: 'MyAgentPool'
  demands:
  - Agent.Name -equals MySpecificAgent
```

### 📥 Agent Installation

🏃‍♂️ **Instalando na prática**: Script para instalação no Linux.
```bash
# Linux
wget https://vstsagentpackage.azureedge.net/agent/2.x.x/vsts-agent-linux-x64-2.x.x.tar.gz
tar zxvf vsts-agent-linux-x64-2.x.x.tar.gz
./config.sh
sudo ./svc.sh install
sudo ./svc.sh start
```

## 📦 Artifacts e Package Management

🎆 **Preservando o trabalho**: Artifacts são os outputs dos seus builds - binários, packages, reports, etc. É essencial gerê-los corretamente!

📈 **Tipos de artifacts**:
- 📝 Build artifacts (binários, JARs, etc.)
- 📦 Package artifacts (NuGet, npm, Docker)
- 📊 Test artifacts (relatórios, coverage)
- 📄 Documentation artifacts

### 📤 Publishing Artifacts

🎯 **Salvando os outputs**: Como publicar artifacts para uso posterior.
```yaml
- task: PublishBuildArtifacts@1
  inputs:
    PathtoPublish: '$(Build.ArtifactStagingDirectory)'
    ArtifactName: 'drop'
    publishLocation: 'Container'
```

### 📦 NuGet Package

🔧 **Criando e publicando packages**: Para bibliotecas .NET reutilizáveis.
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

## 🧪 Testing e Code Coverage

🎯 **Qualidade não é acidente**: Testes automatizados são o coração de um bom pipeline CI/CD. Sem eles, você está voando às cegas!

📈 **Pirâmide de Testes**:
- 🟢 Unit Tests (base) - Rápidos e muitos
- 🟡 Integration Tests (meio) - Médios em quantidade
- 🔴 E2E Tests (topo) - Poucos mas críticos

### 🎯 Unit Tests

⚡ **Testes rápidos e confiáveis**: A base da sua estratégia de testes.
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

### 🔍 SonarCloud Integration

🏆 **Qualidade de código nível enterprise**: SonarCloud analisa seu código em busca de bugs, vulnerabilidades e code smells.
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

## 🤔 Conditional Logic

🎨 **Inteligência nos pipelines**: Conditions permitem que seus pipelines tomem decisões baseadas no contexto - branch, trigger type, success/failure, etc.

✨ **Casos comuns**:
- Deploy apenas da branch main
- Steps diferentes para PR vs. CI
- Cleanup apenas em caso de falha

### ⚙️ Conditions

📄 **Conditions básicas**: Os building blocks para lógica condicional.
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

### 🤖 Custom Conditions

🎨 **Lógica avançada**: Combinando múltiplas condições para cenários complexos.
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

## 🔍 Troubleshooting

😱 **Quando as coisas dão errado**: Todo desenvolvedor passa por isso! Aqui estão as técnicas e ferramentas para debuggar pipelines.

💡 **Regra de ouro**: Sempre comece com logs detalhados antes de partir para soluções complexas.

### 🐛 Debug Mode

🔍 **Ativando logs detalhados**: Sua primeira linha de defesa para troubleshooting.
```yaml
variables:
  system.debug: true
```

### ⚠️ Common Issues

🎯 **Os suspeitos do costume**: Problemas mais frequentes e suas soluções.

**🚫 1. Permission errors**: 
- ✅ **Solução**: Verificar permissões de service connection
- 🔍 **Debug**: Checar logs de autenticação
- 🛠️ **Prevenção**: Usar least privilege principle

**🤖 2. Agent issues**: 
- ✅ **Solução**: Verificar capabilities e demands
- 🔍 **Debug**: Logs do agent
- 🛠️ **Prevenção**: Documentar requirements

**📋 3. Variable scope**: 
- ✅ **Solução**: Verificar escopo de variáveis
- 🔍 **Debug**: Imprimir valores das variáveis
- 🛠️ **Prevenção**: Usar naming conventions

**📄 4. Template errors**: 
- ✅ **Solução**: Validar sintaxe YAML
- 🔍 **Debug**: Verificar passagem de parâmetros
- 🛠️ **Prevenção**: Usar schema validation
1. **Permission errors**: Check service connection permissions
2. **Agent issues**: Verify agent capabilities and demands
3. **Variable scope**: Ensure variables are accessible in the right scope
4. **Template errors**: Validate YAML syntax and parameter passing