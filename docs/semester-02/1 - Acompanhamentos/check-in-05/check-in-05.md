# Registro de Acompanhamento - 35/11/2026

**Projeto:** Website para auxiliar estudantes do ENEM
**Estudante:** Ana Beatriz Rangel Mattos
**Status:** Em Desenvolvimento (Módulo de Artigos e Playlists para Professores)

## 1. O que foi desenvolvido durante este período?

- **Refatoração da Arquitetura Backend e Serviços:** Reestruturação da camada de aplicação (`EnemPrep.Application`) para uma abordagem orientada a funcionalidades (*feature-first*) utilizando serviços e interfaces puros em substituição ao MediatR, incluindo a refatoração completa do `SubjectsService`, atualizações nas permissões de acesso e ajustes na entidade `ScheduleSubject` com chave primária UUID para suportar a repetição de disciplinas na semana.
- **Quadro Kanban de Cronograma (Drag-and-Drop):** Finalização da interface interativa de organização semanal no frontend utilizando `@dnd-kit/react`, implementando a adição de disciplinas via popover (`AddSubjectPopover`), reordenação e movimentação entre colunas sincronizadas com a API, além da atualização reativa de estado sem recarregamento de página.
- **Criação de Artigos em Markdown/Rich-Text:** Desenvolvimento avançado da página de criação de artigos (`/articles/new`) integrada ao editor TipTap, contemplando controles customizados de formatação e extensões próprias para inserção de blocos de playlists (com a visualização pública planejada para a próxima etapa).
- **Documentação da API e Testes de Integração:** Configuração de documentação interativa via Swagger/OpenAPI (incluindo suporte a autenticação/autorização) e ampliação da suíte de testes de integração automatizados no backend com `WebApplicationFactory` (incluindo `SchedulesControllerTests`).
- **Conteinerização e Documentação do Projeto:** Criação dos arquivos `Dockerfile` e Docker Compose (`compose.yaml`, `compose.backend.yaml` e `compose.frontend.yaml`) para execução completa ou isolada da aplicação, além da atualização do `README.md` com instruções de arquitetura e execução.

## 2. Quantidade de horas

### 2.1 Horas Alcançadas

- **Ana Beatriz:** Total 88:18 horas.

### 2.2 Horas Pretendidas

- **Ana Beatriz:** 160 horas.

## 3. Evidências de Progresso

- **Repositório GitHub:** [Projeto no GitHub](https://github.com/ana-rangel-mattos/enem-prep-platform)
- **Gerenciamento de projeto no Jira:** [Projeto Jira](https://enemprep.atlassian.net/jira/core/projects/ENEMP/board?filter=&groupBy=priority&atlOrigin=eyJpIjoiZTQ0YjE0YjIyMDdkNGRjZGI3NDk4NGM0MTkwOTlhY2MiLCJwIjoiaiJ9)


## 4. Quais foram os principais avanços?

- Consolidação de uma arquitetura backend mais limpa, modular e de fácil manutenção, eliminando acoplamentos desnecessários e facilitando a criação de novos recursos e testes automatizados.
- Conclusão da experiência interativa do cronograma de estudos semanal, permitindo que os estudantes organizem e reordenem suas disciplinas dinamicamente com persistência imediata no banco de dados.
- Estruturação do fluxo de autoria de conteúdos educacionais com editor rico e suporte a blocos customizados de playlists.
- Padronização do ambiente de desenvolvimento e implantação por meio do Docker Compose, garantindo reprodutibilidade entre backend, banco de dados PostgreSQL e frontend.

## 5. Quais dificuldades você encontrou?

- **Compatibilidade e Eventos no Drag-and-Drop:** Houve conflito inicial entre pacotes distintos do ecossistema `@dnd-kit` (`@dnd-kit/core` vs. `@dnd-kit/react`), fazendo com que os cartões retornassem à posição original ao serem soltos; o problema foi resolvido unificando os hooks e o `DragDropProvider` sob `@dnd-kit/react`.
- **Serialização de Enums entre TypeScript e C#:** Durante a integração do endpoint de reordenação (`UpdateScheduleOrder`), ocorreram erros `400 Bad Request` devido à divergência entre a representação em string das colunas no frontend e o mapeamento esperado pelo enum `DayOfTheWeek` no backend.
- **Autenticação de Sessão e Permissões:** Ajustes necessários na validação de cookies de sessão e permissões de acesso (`SessionAuthenticationHandler` e `HasPermission`) durante o consumo das rotas protegidas de cronogramas e disciplinas.

## 6. O que planeja fazer até o próximo check-in?

- **Refinamento do Módulo de Artigos:**
    - Concluir os ajustes finais da criação de artigos e desenvolver a página de listagem e visualização pública dos artigos e playlists para os estudantes.
    - Implementar a funcionalidade de exclusão direta de disciplinas nos cartões do quadro Kanban do cronograma.
- **Polimento da Página de Perfil:** Finalizar a integração e renderização das métricas gamificadas do estudante.
- **Detalhamento do Simulado:** Concluir a funcionalidade de detalhes do exame ao clicar no `ExamCard`.
- **Testes:** Expandir a cobertura de testes de integração para os novos endpoints de artigos, playlists e disciplinas.
- **Temas:** Fazer o frontend responder ao tema selecionado pelo usuário.
