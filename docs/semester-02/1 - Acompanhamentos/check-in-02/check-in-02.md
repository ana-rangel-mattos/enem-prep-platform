# Registro de Acompanhamento - 07/09/2026

**Projeto:** Website para auxiliar estudantes do ENEM
**Estudante:** Ana Beatriz Rangel Mattos
**Status:** Em Desenvolvimento (Implementação de Perfil de Usuário, Metas de Estudo e Módulo de Simulados)

## 1. O que foi desenvolvido durante este período?

- **Página de Perfil e Preferências do Usuário (`/profile`):**
    - Implementação completa dos endpoints de backend e contratos de serviço para gerenciamento de perfil e preferências (`UserPreferencesService`, `UserPreferencesController`, `GetUserPreferencesDto`).
    - Criação do `PreferencesBootstrap` no front-end e sincronização com o estado global no Redux (`preferencesSlice`).
    - Interface do perfil do usuário para exibição e atualização de dados cadastrais e preferências de estudo.
- **Módulo de Metas de Estudo (`User Goals`):**
    - Desenvolvimento dos componentes de interface para criação, edição e exclusão de metas (`CreateUpdateGoalForm`, `DeleteUserGoalForm`).
    - Integração com o backend através da criação da camada de API (`userGoalService`) e tipagens dedicadas (`user-goal.types.ts`).
- **Módulo de Simulados (`Exam Component` - Em andamento):**
    - Estruturação do fluxo completo de rotas no Next.js (`/exams`, `/exams/new`, `/exams/[id]/take`).
    - Desenvolvimento dos serviços e DTOs no backend para criação e execução de exames (`ExamsService`, `GetExamDto`, `TakeExamDto`, `SubmitAnswerRequest`).
    - Implementação dos componentes de renderização de questões (`ExamQuestion`) e gerenciamento de tempo (`TimerCountdown` acompanhado do hook customizado `use-countdown`).
    - Ajustes de persistência no PostgreSQL, incluindo migração para aumento do limite de caracteres do título do simulado (`ChangeExamTitleMaxLength`) e correções nas triggers de acompanhamento do usuário (`user_streak.sql`).

## 2. Quantidade de horas

### 2.1 Horas Alcançadas

- **Ana Beatriz:** Total 28:05 horas.

### 2.2 Horas Pretendidas

- **Ana Beatriz:** 160 horas.

## 3. Evidências de Progresso

- **Repositório GitHub:** [Projeto no GitHub](https://github.com/ana-rangel-mattos/enem-prep-platform)
- **Gerenciamento de projeto no Jira:** [Projeto Jira](https://enemprep.atlassian.net/jira/core/projects/ENEMP/board?filter=&groupBy=priority&atlOrigin=eyJpIjoiZTQ0YjE0YjIyMDdkNGRjZGI3NDk4NGM0MTkwOTlhY2MiLCJwIjoiaiJ9)


## 4. Quais foram os principais avanços?

- **Arquitetura e Fluxo de Simulados:** Mapeamento e construção da infraestrutura completa de geração de provas e execução cronometrada no front-end.
- **Personalização e Estado Global:** Integração bem-sucedida do bootstrap de preferências de usuário ao ciclo de vida da aplicação via Redux Toolkit.
- **Acompanhamento de Metas:** Disponibilização de CRUD completo no front-end para que o estudante possa definir e gerenciar suas metas de preparação.
- **Ajuste Banco de Dados:** Resolução de limitações de schema em tabelas de simulado e ajuste em procedures PL/pgSQL.

## 5. Quais dificuldades você encontrou?

- **Tratamento de Exceções e Triggers no PostgreSQL:** Identificação e correção de erros em funções engatadas (`triggers`) de atualização de streak do usuário e estouro de tamanho de colunas (`varchar`), contornados via novas migrações no EF Core.
- **Sincronização do Cronômetro de Simulados:** Garantia de que o hook de contagem regressiva (`useCountdown`) e o componente de UI (`TimerCountdown`) mantivessem estado consistente durante ciclos de renderização e acionamento por eventos de usuário.

## 6. O que planeja fazer até o próximo check-in?

- **Finalização do Fluxo do Simulado:** Concluir a submissão das respostas em lote e exibição da tela de resultados do simulado.
- **Integração de Métricas de Desempenho:** Conectar os dados de exames concluídos aos gráficos e resumos exibidos na Dashboard e na página de Perfil.
- **Refinamentos de UI/UX:** Aplicar validações de formulário adicionais e melhorias de responsividade nos componentes do simulado.