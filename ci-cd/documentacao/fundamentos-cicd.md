# Fundamentos de CI/CD com Azure DevOps

## O que é CI/CD?

**Continuous Integration (CI)** e **Continuous Deployment/Delivery (CD)** são práticas de desenvolvimento que automatizam a integração, teste e entrega de código.

### Continuous Integration (CI)
- **Objetivo**: Integrar mudanças de código frequentemente
- **Benefícios**: Detecção precoce de bugs, redução de conflitos de merge
- **Práticas**: Commits frequentes, builds automatizados, testes automáticos

### Continuous Deployment/Delivery (CD)
- **Continuous Delivery**: Código sempre pronto para produção
- **Continuous Deployment**: Deploy automático em produção
- **Benefícios**: Releases mais rápidos e confiáveis, menor risco

## Azure DevOps Overview

O Azure DevOps é uma plataforma completa para desenvolvimento que inclui:

### Azure Repos
- Repositórios Git privados
- Controle de versão distribuído
- Branch policies e pull requests

### Azure Pipelines
- CI/CD como código (YAML)
- Suporte multiplataforma (Windows, Linux, macOS)
- Agentes hospedados e self-hosted

### Azure Boards
- Planejamento ágil
- Tracking de work items
- Dashboards e relatórios

### Azure Artifacts
- Gerenciamento de pacotes
- Feed de NuGet, npm, Maven, PyPI
- Versionamento de artefatos

## Componentes de um Pipeline Azure DevOps

### 1. Triggers
```yaml
trigger:
  branches:
    include:
      - main
      - develop
  paths:
    include:
      - src/*
    exclude:
      - docs/*
```

### 2. Variables
```yaml
variables:
  buildConfiguration: 'Release'
  vmImageName: 'ubuntu-latest'
```

### 3. Stages
```yaml
stages:
- stage: Build
  jobs:
  - job: BuildJob
    steps:
    - task: DotNetCoreCLI@2
```

### 4. Jobs e Steps
```yaml
jobs:
- job: BuildAndTest
  pool:
    vmImage: $(vmImageName)
  steps:
  - task: UseDotNet@2
    inputs:
      version: '6.x'
  - task: DotNetCoreCLI@2
    inputs:
      command: 'build'
```

## Estratégias de Branching

### GitFlow
- **main**: Código de produção
- **develop**: Desenvolvimento
- **feature**: Novas funcionalidades
- **release**: Preparação de release
- **hotfix**: Correções urgentes

### GitHub Flow
- **main**: Código de produção
- **feature**: Branches de funcionalidades
- Pull requests para main

## Estratégias de Deploy

### Blue-Green Deployment
- Dois ambientes idênticos (Blue e Green)
- Switch instantâneo entre ambientes
- Rollback rápido em caso de problemas

### Canary Deployment
- Deploy gradual para subconjunto de usuários
- Monitoramento de métricas
- Aumento gradual do tráfego

### Rolling Deployment
- Atualização incremental de instâncias
- Sem downtime
- Rollback por versão anterior

## Boas Práticas

### Pipeline Design
1. **Pipeline como código**: Versionar arquivos YAML
2. **Idempotência**: Pipelines devem produzir mesmo resultado
3. **Fail fast**: Falhar rapidamente em caso de erro
4. **Paralelização**: Executar jobs em paralelo quando possível

### Segurança
1. **Secrets**: Usar Azure Key Vault para dados sensíveis
2. **Service Connections**: Configurar conexões seguras
3. **Least Privilege**: Permissões mínimas necessárias
4. **Code Analysis**: SAST/DAST tools

### Monitoramento
1. **Application Insights**: Telemetria de aplicação
2. **Pipeline Analytics**: Métricas de pipeline
3. **Alerting**: Notificações de falhas
4. **Dashboards**: Visualização de métricas

## Ferramentas Complementares

### Qualidade de Código
- **SonarCloud**: Análise estática de código
- **WhiteSource**: Scanning de vulnerabilidades
- **Checkmarx**: Security scanning

### Testes
- **Unit Tests**: xUnit, NUnit, MSTest
- **Integration Tests**: TestContainers
- **Load Tests**: Azure Load Testing
- **UI Tests**: Selenium, Playwright

### Infrastructure as Code
- **ARM Templates**: Azure Resource Manager
- **Terraform**: Multi-cloud infrastructure
- **Bicep**: Domain-specific language para Azure

## Métricas Importantes

### DORA Metrics
1. **Deployment Frequency**: Frequência de deploys
2. **Lead Time for Changes**: Tempo de código até produção
3. **Change Failure Rate**: Taxa de falha em mudanças
4. **Mean Time to Recovery**: Tempo médio de recuperação