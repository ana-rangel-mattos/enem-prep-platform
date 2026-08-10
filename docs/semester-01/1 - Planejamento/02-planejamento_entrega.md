# Planejamento para entrega final

- [Planejamento para entrega final](#planejamento-para-entrega-final)
  - [Tarefas](#tarefas)
    - [Back-End](#back-end)
    - [Front-End](#front-end)
    - [DevOps](#devops)

## Tarefas

### Back-End

- Adcionar enunciado, alternativa escolhida e alternativa correta ao `SolvedQuestionDto`.
- Implementar um serviço para controlar a preferência do usuário (`UserPreferencesService`)
  - Métodos: `update`, `reset`.
- Implementar um serciço para gerenciar os Exames resolvidos pelos usuários.
  - Métodos: `create`, `update status`, `fetch solved exam by id`, `fetch solved exams`.
- Implementar Endpoints em `QuestionsController` para as questões salvas pelo usuário:
  - `POST /api/questions/{id}/save`
  - `DELETE /api/questions/{id}/unsave`
  - `GET /api/questions/saved`
    - Usar arquitetura páginada para retornar lista de questões resolvidas.
    - Com filtros.
- Implementar Swagger API para a documentação do projeto. 

### Front-End

- Adicionar as dependências (`shadcn`, `Next.js`)
- Implementar uma `sidebar` com links para as páginas principais.
- Páginas:
  - Profile:
    - Ver o perfil dos usuários.
    - Configurar o perfil do usuário.
  - Configurações:
    - Configurar as preferências do usuário (tema, linguagem dos exames, número de questões por dia).
  - Metas:
    - Visualizar metas definidas pelo usuário.
    - Configuração das metas.
  - Banco de Questões:
    - Salvar questão.
    - Deletar questão salva.
    - Resolver questão.
  - Histórico de questões resolvidas:
    - Visualização de questões resolvidas.
    - Estatística simples sobre os acertos do usuário.
  - Exames:
    - Visualizar exames resolvidos.
    - Resolver novo exame com filtros (por área, ano).
  - Dashboard de administrador:
    - Funções que são únicas do cargo de administrador como deletar usuário, criar questões, deletar questões entre outras.

### DevOps

- Criar um único arquivo `⁠docker-compose.yml`⁠ que inicie tanto a instância de produção do PostgreSQL (com os esquemas e extensões corretos inicializados) quanto o backend .NET compilado.
