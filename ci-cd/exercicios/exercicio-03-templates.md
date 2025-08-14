# 🏗️ Exercício 3: Templates e Reutilização

## 🎯 Objetivo
Aprender a criar templates reutilizáveis para pipelines, promovendo consistência e manutenibilidade entre múltiplos projetos.

## 📋 Cenário
Sua empresa tem múltiplas APIs .NET e precisa padronizar os pipelines. Você deve criar templates que podem ser reutilizados por diferentes equipes, mantendo flexibilidade para customizações específicas.

## ✅ Pré-requisitos
- 🏆 Exercícios 1 e 2 completados
- 📄 Conhecimento básico de YAML
- 📁 Entendimento de parâmetros e reutilização

## 💡 Sobre este Exercício
**🎆 Foco no Aprendizado:** Este exercício usa templates **simulados** (como exercício 2) para ensinar conceitos de reutilização sem complexidades de infraestrutura real.

## 📦 Código Pronto
**🎉 Boa notícia!** Os templates de exemplo já estão prontos na pasta `codigo-exemplo/exercicio-03-templates/`!

Você pode **focar 100% no aprendizado de templates** sem perder tempo escrevendo código.

## 📁 Parte 1: Estrutura dos Templates

### 📂 1.1 Copiar Templates de Exemplo

1. **📥 Copie** todo o conteúdo da pasta `codigo-exemplo/exercicio-03-templates/` 
2. **📤 Cole** na raiz do seu repositório Azure DevOps
3. **✅ Verifique** a estrutura:

```
seu-repositorio/
├── templates/                  # Templates reutilizáveis
│   ├── steps/
│   │   ├── dotnet-build.yml   # Build .NET
│   │   └── dotnet-test.yml    # Testes .NET
│   └── jobs/
│       └── build-job.yml      # Job completo
├── projeto-a/                 # Projeto exemplo A
│   └── src/
│       ├── WebApi/            # Products API
│       └── WebApi.Tests/
└── projeto-b/                 # Projeto exemplo B (você criará)
    └── src/
        ├── WebApi/            # Orders API  
        └── WebApi.Tests/
```

### 🧠 1.2 Conceitos que Vamos Aprender

- 📝 **Parameters**: Customização de templates
- 🔄 **Reutilização**: Um template, múltiplos projetos
- 📊 **Hierarquia**: Steps → Jobs → Stages
- 🎯 **DRY Principle**: Don't Repeat Yourself
- 📁 **Organização**: Estrutura clara de templates

## ⚙️ Parte 2: Entendendo os Templates

### 🔍 2.1 Analisar Template de Steps

Abra o arquivo `templates/steps/dotnet-build.yml` (já criado):

```yaml
# Template para build .NET
parameters:
- name: buildConfiguration
  type: string
  default: 'Release'
- name: projects
  type: string
  default: '**/*.csproj'
- name: restorePackages
  type: boolean
  default: true

steps:
- ${{ if eq(parameters.restorePackages, true) }}:
  - task: DotNetCoreCLI@2
    displayName: 'Restore NuGet packages'
    inputs:
      command: 'restore'
      projects: ${{ parameters.projects }}

- ${{ if eq(parameters.restorePackages, true) }}:
  - task: DotNetCoreCLI@2
    displayName: 'Build application'
    inputs:
      command: 'build'
      projects: ${{ parameters.projects }}
      arguments: '--configuration ${{ parameters.buildConfiguration }} --no-restore'
- ${{ else }}:
  - task: DotNetCoreCLI@2
    displayName: 'Build application'
    inputs:
      command: 'build'
      projects: ${{ parameters.projects }}
      arguments: '--configuration ${{ parameters.buildConfiguration }}'
```

**📚 Conceitos importantes:**
- 📝 **Parameters**: Tornam template flexível
- 🔧 **Default values**: Valores padrão para conveniência  
- ⚖️ **Conditions**: Lógica condicional (${{ if }})
- 🔄 **Template expressions**: ${{ parameters.name }}

### 🔍 2.2 Analisar Template de Job

Abra o arquivo `templates/jobs/build-job.yml` (já criado):

```yaml
# Template para job de build completo
parameters:
- name: vmImage
  type: string
  default: 'ubuntu-latest'
- name: buildConfiguration
  type: string
  default: 'Release'
- name: projectPath
  type: string
  default: 'src'

jobs:
- job: Build
  displayName: 'Build and Test'
  pool:
    vmImage: ${{ parameters.vmImage }}
  
  steps:
  - task: UseDotNet@2
    displayName: 'Install .NET SDK'
    inputs:
      version: '6.x'

  - template: ../steps/dotnet-build.yml
    parameters:
      buildConfiguration: ${{ parameters.buildConfiguration }}
      projects: '${{ parameters.projectPath }}/**/*.csproj'

  # Simulação de deployment (sem recursos Azure)
  - script: |
      echo "🚀 Simulating deployment..."
      echo "Project: ${{ parameters.projectPath }}"
      echo "Configuration: ${{ parameters.buildConfiguration }}"
      echo "✅ Deployment simulation completed!"
    displayName: '🚀 Simulate Deployment'
```

**📚 Conceitos importantes:**
- 🏗️ **Template composition**: Template chama outro template
- 📂 **Relative paths**: `../steps/dotnet-build.yml`
- 🔗 **Parameter passing**: Repassar parâmetros entre templates

## 🎯 Parte 3: Usando Templates em Projeto A

### 📄 3.1 Pipeline do Projeto A

Crie o arquivo `projeto-a/azure-pipelines.yml`:

```yaml
# Pipeline do Projeto A (Products API) usando templates
trigger:
  branches:
    include:
    - main
  paths:
    include:
    - projeto-a/*

variables:
  buildConfiguration: 'Release'

stages:
# Build Stage usando template
- stage: BuildProjectA
  displayName: '🏗️ Build Products API'
  jobs:
  - template: ../templates/jobs/build-job.yml
    parameters:
      vmImage: 'ubuntu-latest'
      buildConfiguration: $(buildConfiguration)
      projectPath: 'projeto-a/src'

# Quality Stage (simulado)
- stage: QualityProjectA
  displayName: '🔍 Quality Check'
  dependsOn: BuildProjectA
  jobs:
  - job: QualityGates
    displayName: 'Quality Analysis'
    pool:
      vmImage: 'ubuntu-latest'
    steps:
    - script: |
        echo "🔍 Running quality analysis for Products API..."
        echo "Code Coverage: 92%"
        echo "Security Scan: ✅ No issues"
        echo "Performance: ✅ All benchmarks passed"
        echo "✅ Products API quality check completed!"
      displayName: '🔍 Simulate Quality Analysis'

# Deploy Stage (simulado)
- stage: DeployProjectA
  displayName: '🚀 Deploy Products API'
  dependsOn: QualityProjectA
  jobs:
  - job: DeployProducts
    displayName: 'Deploy Products API'
    pool:
      vmImage: 'ubuntu-latest'
    steps:
    - script: |
        echo "🚀 Deploying Products API..."
        echo "Environment: Development"
        echo "URL: https://products-api-dev.company.com (simulated)"
        echo "✅ Products API deployed successfully!"
      displayName: '🚀 Simulate Products Deployment'
```

### 🧪 3.2 Testar Pipeline do Projeto A

1. **📤 Commit** o arquivo `projeto-a/azure-pipelines.yml`
2. **🔵 Azure DevOps** → **New Pipeline** 
3. **📂 Existing YAML file** → `projeto-a/azure-pipelines.yml`
4. **▶️ Run** e verificar execução

## 🔄 Parte 4: Reutilizando em Projeto B

### 📦 4.1 Criar Projeto B (Orders API)

Crie a estrutura do Projeto B:

```bash
# Criar estrutura do Projeto B
mkdir -p projeto-b/src/WebApi/Controllers
mkdir -p projeto-b/src/WebApi.Tests
```

### 🛒 4.2 Orders API (Projeto B)

**projeto-b/src/WebApi/WebApi.csproj:**
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.2.3" />
  </ItemGroup>
</Project>
```

**projeto-b/src/WebApi/Program.cs:**
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

**projeto-b/src/WebApi/Controllers/OrdersController.cs:**
```csharp
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private static readonly List<Order> Orders = new()
    {
        new Order { Id = 1, CustomerName = "João Silva", Total = 199.99m },
        new Order { Id = 2, CustomerName = "Maria Santos", Total = 299.50m },
        new Order { Id = 3, CustomerName = "Pedro Costa", Total = 89.90m }
    };

    [HttpGet]
    public ActionResult<IEnumerable<Order>> Get()
    {
        return Ok(Orders);
    }

    [HttpGet("{id}")]
    public ActionResult<Order> Get(int id)
    {
        var order = Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
            return NotFound();
        
        return Ok(order);
    }
}

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Total { get; set; }
}
```

### 📄 4.3 Pipeline do Projeto B (Reutilizando Templates!)

**projeto-b/azure-pipelines.yml:**
```yaml
# Pipeline do Projeto B (Orders API) - REUTILIZANDO os mesmos templates!
trigger:
  branches:
    include:
    - main
  paths:
    include:
    - projeto-b/*

variables:
  buildConfiguration: 'Debug'  # Diferente do Projeto A!

stages:
# Build Stage - MESMO template, parâmetros diferentes!
- stage: BuildProjectB
  displayName: '🛒 Build Orders API'
  jobs:
  - template: ../templates/jobs/build-job.yml
    parameters:
      vmImage: 'windows-latest'  # VM diferente!
      buildConfiguration: $(buildConfiguration)
      projectPath: 'projeto-b/src'

# Quality Stage customizada
- stage: QualityProjectB
  displayName: '🔍 Quality Check Orders'
  dependsOn: BuildProjectB
  jobs:
  - job: QualityGates
    pool:
      vmImage: 'windows-latest'
    steps:
    - script: |
        echo "🔍 Running quality analysis for Orders API..."
        echo "Code Coverage: 88%"
        echo "Security Scan: ✅ No issues"
        echo "Business Rules: ✅ All validations passed"
        echo "✅ Orders API quality check completed!"
      displayName: '🔍 Quality Analysis - Orders'

# Deploy Stage customizado  
- stage: DeployProjectB
  displayName: '🛒 Deploy Orders API'
  dependsOn: QualityProjectB
  jobs:
  - job: DeployOrders
    pool:
      vmImage: 'windows-latest'
    steps:
    - script: |
        echo "🛒 Deploying Orders API..."
        echo "Environment: Staging"
        echo "URL: https://orders-api-staging.company.com (simulated)"
        echo "Database: Orders-Staging-DB"
        echo "✅ Orders API deployed successfully!"
      displayName: '🛒 Deploy Orders API'
```

## ✅ Parte 5: Validação

### 🏆 5.1 Critérios de Sucesso

✅ **Templates funcionam em múltiplos projetos**
- Template usado em Projeto A e Projeto B
- Parâmetros diferentes produzem resultados diferentes
- Mesmo código de template, comportamentos customizados

✅ **Reutilização é efetiva**
- Não há duplicação de código YAML
- Manutenção centralizada nos templates
- Customização através de parâmetros

✅ **Flexibilidade mantida**
- Projeto A usa Ubuntu, Projeto B usa Windows
- Diferentes configurações (Release vs Debug)
- Customização de steps específicos

### 🔍 5.2 Verificações Práticas

1. **🔄 Ambos pipelines executam:** Projeto A e B funcionam
2. **📊 Logs diferentes:** VMs e configurações diferentes aparecem nos logs
3. **⚙️ Templates iguais:** Mesmo código de template usado por ambos
4. **🛠️ Manutenção:** Alterar template afeta ambos os projetos

## 🚀 Parte 6: Melhorias Opcionais

### 📄 6.1 Template com Múltiplas Tecnologias

Crie `templates/jobs/multi-tech-build.yml`:

```yaml
# Template que suporta .NET e Node.js
parameters:
- name: technology
  type: string
  values:
  - dotnet
  - nodejs
- name: buildConfiguration
  type: string
  default: 'Release'

jobs:
- job: Build
  steps:
  - ${{ if eq(parameters.technology, 'dotnet') }}:
    - template: ../steps/dotnet-build.yml
      parameters:
        buildConfiguration: ${{ parameters.buildConfiguration }}
  
  - ${{ if eq(parameters.technology, 'nodejs') }}:
    - script: |
        echo "📦 Installing Node.js dependencies..."
        npm install
        echo "🏗️ Building Node.js application..."
        npm run build
        echo "✅ Node.js build completed!"
      displayName: '📦 Build Node.js App'
```

### 🌍 6.2 Template com Environments Dinâmicos

```yaml
# templates/stages/deploy-multi-env.yml
parameters:
- name: environments
  type: object
- name: projectName
  type: string

stages:
- ${{ each env in parameters.environments }}:
  - stage: Deploy_${{ env.name }}
    displayName: '🚀 Deploy to ${{ env.displayName }}'
    jobs:
    - job: Deploy
      steps:
      - script: |
          echo "🚀 Deploying ${{ parameters.projectName }} to ${{ env.displayName }}..."
          echo "URL: ${{ env.url }}"
          echo "✅ Deployment completed!"
        displayName: '🚀 Deploy ${{ parameters.projectName }}'
```

**Uso:**
```yaml
- template: templates/stages/deploy-multi-env.yml
  parameters:
    projectName: 'Products API'
    environments:
    - name: 'dev'
      displayName: 'Development'
      url: 'https://products-dev.company.com'
    - name: 'prod'
      displayName: 'Production'
      url: 'https://products.company.com'
```

## 🐛 Troubleshooting

### ⚠️ Problemas Comuns

**❌ Template não encontrado:**
- Verificar path relativo (`../templates/...`)
- Certificar-se de que arquivo existe

**❌ Parâmetro não reconhecido:**
- Verificar sintaxe: `${{ parameters.nome }}`
- Confirmar se parâmetro foi declarado

**❌ Template não executa:**
- Validar indentação YAML
- Verificar se todos os parâmetros obrigatórios foram passados

**❌ Erro: "The directive 'if' is not allowed in this context":**
- ❌ **Errado**: `arguments: '--config ${{ if eq(params.x, true) }}--flag'` 
- ✅ **Correto**: Usar `${{ if }}` como bloco completo separado:
```yaml
- ${{ if eq(parameters.flag, true) }}:
  - task: Build
    inputs:
      arguments: '--config --flag'
- ${{ else }}:
  - task: Build  
    inputs:
      arguments: '--config'
```

### 🔧 Debug Tips

1. **Echo parâmetros** para verificar valores:
```yaml
- script: echo "Config: ${{ parameters.buildConfiguration }}"
```

2. **Validar YAML** antes de commit
3. **Testar templates** isoladamente

## 🎉 Resultado Esperado

Ao final deste exercício, você terá:
- ✅ Templates reutilizáveis funcionando
- ✅ Dois projetos usando os mesmos templates
- ✅ Customização através de parâmetros
- ✅ Estrutura organizada e maintível
- ✅ Redução de 80%+ na duplicação de código
- ✅ Base para padronização em toda empresa

⏱️ **Tempo estimado**: 45-60 minutos

## ➡️ Próximo Passo
Prossiga para o **Exercício 4: Integração com Azure Key Vault** para aprender sobre gerenciamento seguro de secrets!

## 🎯 Benefícios dos Templates

**💼 Para a Empresa:**
- 📏 Padronização de pipelines
- 🔧 Manutenção centralizada
- 🚀 Time-to-market reduzido para novos projetos

**👨‍💻 Para Desenvolvedores:**
- ⏱️ Menos tempo criando pipelines
- ✅ Menos erros de configuração
- 🎯 Foco no código de negócio

**🏢 Para DevOps:**
- 🛠️ Governança consistente
- 📊 Padrões de qualidade enforced
- 🔄 Evolução de práticas centralizada