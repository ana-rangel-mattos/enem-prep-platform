# Registro de Acompanhamento - 23/10/2026

**Projeto:** Website para auxiliar estudantes do ENEM
**Estudante:** Ana Beatriz Rangel Mattos
**Status:** Em Desenvolvimento (Backend e Kanban do Cronograma de Estudos, Módulo de Artigos e Playlists para Professores)

## 1. O que foi desenvolvido durante este período?

- **Módulo de Cronograma de Estudos (Backend & Frontend Kanban):**
    - Implementação completa dos serviços e controlador de backend para gerenciamento de cronogramas semanais (`ScheduleService`, `SchedulesController`) com suporte a reordenação de matérias e dias da semana.
    - Desenvolvimento da interface visual em estilo Kanban no front-end (`ScheduleKanban`, `ScheduleSelection`, `CreateSchedulePopover`, `EditSchedulePopover`), permitindo a visualização e organização da rotina de estudos (com CRUD em andamento).
- **Sistema de Artigos e Playlists de Exercícios (`Teacher Role`):**
    - Modelagem de domínio e persistência para a criação de artigos baseados em blocos polimórficos (Article, ArticleBlock, ArticlePlaylist) e playlists de exercícios associadas a questões (Playlist, PlaylistQuestion).
    - Criação de DTOs, validadores, migrações do Entity Framework Core e a camada de serviços (`ArticleService`) para suportar a publicação de conteúdos pedagógicos por professores.
- **Ajustes de Estado Global e Componentes de UI:**
    - Correção na mutação de estado do Redux Toolkit no preferencesSlice e unificação do fluxo de carregamento de preferências junto ao bootstrap de autenticação.
    - Adição de componentes auxiliares de UI (`kanban`, `native-select`, `progress`).

## 2. Quantidade de horas

### 2.1 Horas Alcançadas

- **Ana Beatriz:** Total 59:43 horas.

### 2.2 Horas Pretendidas

- **Ana Beatriz:** 160 horas.

## 3. Evidências de Progresso

- **Repositório GitHub:** [Projeto no GitHub](https://github.com/ana-rangel-mattos/enem-prep-platform)
- **Gerenciamento de projeto no Jira:** [Projeto Jira](https://enemprep.atlassian.net/jira/core/projects/ENEMP/board?filter=&groupBy=priority&atlOrigin=eyJpIjoiZTQ0YjE0YjIyMDdkNGRjZGI3NDk4NGM0MTkwOTlhY2MiLCJwIjoiaiJ9)


## 4. Quais foram os principais avanços?

- **Arquitetura de Conteúdo Flexível:** Implementação de suporte a blocos polimórficos no backend para artigos, permitindo estruturar páginas complexas (misturando textos, cabeçalhos e playlists de exercícios) de forma escalável.
- **Experiência de Organização Visual:** Introdução do componente Kanban para o cronograma de estudos, facilitando a interação do estudante com suas matérias semanais.
- **Consolidação dos Serviços de Backend:** Expansão robusta das entidades e contratos de serviço no .NET para cobrir tanto o gerenciamento de rotinas quanto os recursos avançados de ensino.

## 5. Quais dificuldades você encontrou?

- **Sincronização de Estado Local no Kanban:** Gerenciamento da reatividade e atualização de colunas/itens ao alternar entre diferentes cronogramas e manipular o estado de arrastar e soltar (drag-and-drop).
- **Serialização Polimórfica de Blocos:** Configuração correta do mapeamento de tipos derivados usando System.Text.Json para persistir e recuperar estruturas de blocos customizadas em colunas jsonb no PostgreSQL.

## 6. O que planeja fazer até o próximo check-in?

- **Conclusão do CRUD de Cronogramas no Front-end:** Finalizar as operações de criação, edição e exclusão de itens diretamente pelo painel Kanban.
- **Refinamento do Módulo de Artigos:** Avançar na interface de criação e visualização de artigos para os professores no cliente Next.js.
- **Polimento da Página de Perfil:** Finalizar a integração e renderização das métricas gamificadas do estudante.
- **Detalhamento do Simulado:** Concluir a funcionalidade de detalhes do exame ao clicar no `ExamCard`.