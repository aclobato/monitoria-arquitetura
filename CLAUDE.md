# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository Overview

This is an educational repository focused on software architecture mentoring ("monitoria de arquitetura"). It contains documentation and practical exercises covering various architecture topics, currently with a focus on CI/CD with Azure DevOps.

## Repository Structure

```
monitoria_arquitetura/
├── ci-cd/                    # CI/CD Integration and Continuous Delivery
│   ├── documentacao/         # Theoretical documentation
│   │   ├── 01-fundamentos-cicd.md
│   │   ├── 02-azure-pipelines-guia.md
│   │   └── 03-referencias-estudo.md
│   └── exercicios/          # Practical exercises
│       ├── exercicio-01-pipeline-basico.md
│       ├── exercicio-02-pipeline-multi-stage.md
│       └── exercicio-03-templates.md
├── microservicos/           # Microservices Architecture (planned)
├── design-patterns/         # Design Patterns (planned)
├── cloud/                   # Cloud Architecture (planned)
└── observabilidade/         # Monitoring and Observability (planned)
```

## Content Areas

### CI/CD Section
The main content currently focuses on Azure DevOps CI/CD practices:
- **Documentation**: Fundamental CI/CD concepts with Azure DevOps
- **Exercises**: Pipeline creation, automated deployment, and release strategies
- **Prerequisites**: Azure DevOps account, Azure subscription, Git, VS Code, .NET SDK 6.0+

### Exercise Structure
Exercises are organized by difficulty:
- **Beginner**: Basic pipelines (30-45 min)
- **Intermediate**: Multi-stage pipelines and templates (45-90 min)  
- **Advanced**: Key Vault integration, Blue-Green deployments (60-120 min)

## Content Guidelines

When working with this repository:
- This is educational content in Portuguese
- Maintain the existing pedagogical structure with clear objectives and durations
- Follow the established pattern of theory → exercises → practical validation
- Exercises should include step-by-step instructions, success criteria, and extension suggestions
- All Azure DevOps content should reference current best practices and official documentation

## File Naming Conventions

- Documentation files: Use numbered descriptive names like `01-fundamentos-cicd.md` for sequential learning
- Exercise files: Follow pattern `exercicio-##-description.md`
- Keep file names in Portuguese to match the content language

## Target Audience

Content is designed for students and professionals learning software architecture concepts, with exercises progressing from basic to advanced levels. Each topic area will eventually include both theoretical foundations and hands-on practice.