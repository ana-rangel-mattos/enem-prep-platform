# Registro de Acompanhamento - 28/09/2026

**Projeto:** Website para auxiliar estudantes do ENEM
**Estudante:** Ana Beatriz Rangel Mattos
**Status:** Em Desenvolvimento (Refatoração de Truncamento de Erros, Módulo de Configurações, Histórico de Simulados e Backend de Cronograma)

## 1. O que foi desenvolvido durante este período?

- **Padronização e Tratamento de Exceções (Full-stack):**
    - Refatoração do das respostas de erro no backend para retornar o objeto de erro customizado (`ProblemDetails`/`Error`) completo.
    - Refatoração de todos os blocos `catch` no front-end para utilizar tipagem estrita com AxiosError e interfaces TypeScript customizadas correspondentes ao modelo retornado pela API.
- **Módulo de Simulados e Histórico (Exam Component & History):**
    - Correção de bugs no fluxo de execução e submissão de exames (`TakeExam`).
    - Criação e ajuste do componente de cartão de histórico de simulados (`ExamCard`) para visualização de provas finalizadas.
- **Página de Configurações (`/settings`):**
    - Desenvolvimento da estrutura completa da página de configurações, dividida em subcomponentes para Conta (`Account`), Preferências (`Preferences`) e Perfil (`Profile`).
    - Refatoração do layout principal da aplicação, navegação superior (`site-header`) e barra lateral (`app-sidebar`).
- **Métricas de Perfil Gamificado (`User Profile Backend`):**
    - Criação dos serviços, DTOs e endpoints de backend para cálculo de estatísticas e progresso gamificado do estudante (`UserProfileService`, `UserProfileController`).
    - Início do desenvolvimento da interface gráfica para exibição dos dados gamificados no perfil (em fase de refinamento).
- **Módulo de Cronograma de Estudos (`Schedule Backend` - Em andamento):**
    - Modelagem e implementação inicial dos serviços, DTOs e migrações para o cronograma semanal do estudante (`ScheduleService`, `IScheduleService`, `ScheduleSubject`).

## 2. Quantidade de horas

### 2.1 Horas Alcançadas

- **Ana Beatriz:** Total 47:08 horas.

### 2.2 Horas Pretendidas

- **Ana Beatriz:** 160 horas.

## 3. Evidências de Progresso

- **Repositório GitHub:** [Projeto no GitHub](https://github.com/ana-rangel-mattos/enem-prep-platform)
- **Gerenciamento de projeto no Jira:** [Projeto Jira](https://enemprep.atlassian.net/jira/core/projects/ENEMP/board?filter=&groupBy=priority&atlOrigin=eyJpIjoiZTQ0YjE0YjIyMDdkNGRjZGI3NDk4NGM0MTkwOTlhY2MiLCJwIjoiaiJ9)


## 4. Quais foram os principais avanços?

- **Consistência na Gestão de Erros:** Unificação do contrato de mensagens de erro entre cliente e servidor, evitando falhas silenciosas ou genéricas no front-end.
- **Módulo de Configurações Completo:** Centralização da edição de dados cadastrais, preferências de estudo e bio do perfil em uma rota dedicada (`/settings`).
- **Evolução do Histórico de Exames:** Expansão do módulo de simulados para que o aluno possa acompanhar suas tentativas e notas anteriores.
- **Base do Cronograma Semanal:** Estruturação inicial do banco de dados e regras de negócio para suportar a montagem da rotina de estudos do usuário.

## 5. Quais dificuldades você encontrou?

- **Mapeamento e Serialização de Erros do Axios:** Garantir que todos os componentes de formulário e chamadas de API interpretassem corretamente o payload de erro customizado retornado pelo middleware sem quebrar o fluxo da UI.
- **Rastreamento de Entidades no EF Core (`ChangeTracker`):** Resolução de inconsistências ao utilizar updates diretos na tabela de exames sem perder o estado atualizado das propriedades no DTO de retorno.

## 6. O que planeja fazer até o próximo check-in?

- **Finalizar o Backend e Frontend do Cronograma (`Schedule`):** Concluir os endpoints de reordenação de matérias (`UpdateScheduleSubjectDayAsync`) e construir a interface visual estilo drag-and-drop.
- **Polimento da Página de Perfil:** Finalizar a integração e renderização das métricas gamificadas do estudante.
- **Detalhamento do Simulado:** Concluir a funcionalidade de detalhes do exame ao clicar no `ExamCard`.