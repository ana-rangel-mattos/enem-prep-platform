# Registro de Acompanhamento - 23/02/2026

**Projeto:** Website para auxiliar estudantes do ENEM
**Estudante:** Ana Beatriz Rangel Mattos
**Status:** Em Planejamento

## 1. O que foi desenvolvido durante este período?

- **Documentação de Requisitos:** Finalização da especificação de requisitos funcionais e não-funcionais (Performance, Segurança, Usabilidade).
- **Modelagem de Casos de Uso:** Elaboração de 11 Casos de Uso detalhando fluxos de autenticação, simulados e cronograma.
- **Modelagem Arquitetural:** Criação de Diagramas de Atividades e de Estado para mapear o comportamento dinâmico do sistema.
- **Modelagem de Dados:** Elaboração do Diagrama Entidade Relacionamento
- **Configuração de Ambiente:** Setup inicial do projeto Full-Stack (React/TypeScript + ASP.NET Core).

## 2. Quantidade de horas

- **Ana Beatriz:** 10:35 horas

## 3. Evidências de Progresso

- **Repositório GitHub:** [Projeto no GitHub](https://github.com/ana-rangel-mattos/enem-prep-platform)
- **Estrutura de Documentação:** Arquivos de requisitos, casos de uso e diagramas técnicos organizados na pasta /docs do repositório.

## 4. Quais foram os principais avanços?

- Definição completa da lógica de negócios e priorização de funcionalidades indispensáveis.

- Estruturação da entidades do banco de dados para garantir a integridade dos resultados dos simulados e estatísticas de desempenho.

- Escolha e configuração da stack do projeto (React + TypeScript, ASP.NET Core).

## 5. Quais dificuldades você encontrou?

- Conciliação dos dados da API externa de questões do ENEM com a estrutura interna de histórico do usuário.

## 6. O que planeja fazer até o próximo check-in?

- **Ana Beatriz:** Continuar a preparação dos diagramas de Estado das Entidades do sistema.
- **Ana Beatriz:** Incluir os estados das entidades no diagrama Entidade Relactionamento.
- **Ana Beatriz:** Criar os scripts de criação do banco de dados com PostgreSQL.
- **Ana Beatriz:** Desenvolver o sistema de autenticação e autorização do usuário.

# Registro de Acompanhamento - 09/03/2026

**Projeto:** Website para auxiliar estudantes do ENEM
**Estudante:** Ana Beatriz Rangel Mattos
**Status:** Em Desenvolvimento (Fase de Banco de Dados)

## 1. O que foi desenvolvido durante este período?

- **Refinamento da Modelagem de Dados:** Evolução do MER para uma estrutura multi-schema (auth, content, planning, tracking), garantindo a separação de responsabilidades e organização lógica.
- **Implementação de Scripts DDL:** Criação dos scripts de criação de tabelas com tipos de dados otimizados, como UUID para chaves primárias e JSONB para armazenamento de questões flexíveis.
- **Automação via Triggers e Functions:** Desenvolvimento de triggers em plpgsql para atualização automática da coluna (updated_at) ao houver atualização na tabela diretamente no banco de dados.
- **Sistema de Gamificação:** Implementação da lógica de Daily Streak com funções e triggers do PostgreSQL, garantindo a integridade dos dados independente do backend.
- **Segurança e Privilégios:** Estruturação de Roles e Users de banco de dados seguindo o princípio de Menor Privilégio para proteger o sistema contra ataques.

## 2. Quantidade de horas

- **Ana Beatriz:** 18:40 horas

## 3. Evidências de Progresso

- **Repositório GitHub:** [Projeto no GitHub](https://github.com/ana-rangel-mattos/enem-prep-platform)
- **Banco de Dados:** Scripts SQL estão presentes na pasta `/sql_scripts`
- **Diagrama ER Atualizado:** Versão final do diagrama refletindo a nova arquitetura de tabelas e relacionamentos Many-to-Many.

## 4. Quais foram os principais avanços?

- Implementação da lógica de streaks, o sistema agora identifica automaticamente se o usuário manteve a constância de estudos diária, incrementando ou resetando a contagem sem intervenção manual do backend.

- Separação entre o usuário da aplicação e o administrador, diminuindo riscos de segurança.

## 5. Quais dificuldades você encontrou?

- Ajuste da função de contar streaks para considerar fusos horários (`timestamptz`) e garantir que a lógica funcione corretamente.

## 6. O que planeja fazer até o próximo check-in?

- **Ana Beatriz:** Implementar o `SessionService` no ASP.NET Core para gerenciar a persistência de login via PostgreSQL.
- **Ana Beatriz:** Criar a "Streak Board" uma Leaderboard utilizando Views com PostgreSQL para garantir otimização do ranking dos usuários.
- **Ana Beatriz:** Realizar o mapeamento das entidades no EF Core seguindo o padrão do banco de dados.

# Registro de Acompanhamento - 20/04/2026

**Projeto:** Website para auxiliar estudantes do ENEM
**Estudante:** Ana Beatriz Rangel Mattos
**Status:** Em Desenvolvimento (Fase de Infraestrutura e Segurança)

## 1. O que foi desenvolvido durante este período?

- **Implementação de Persistência de Sessão Distribuída:** Desenvolvimento da lógica de autenticação baseada em sessões, utilizando o PostgreSQL como cache distribuído para garantir a persistência e o controle centralizado do ciclo de vida do usuário.
- **Sistema de Autorização:** Implementação de `AuthorizationHandler` customizado no ASP.NET Core, permitindo uma validação de acesso baseada em permissões de domínio em vez de cargos estáticos.
- **Modelagem de RBAC (Role-Based Access Control):** Estruturação de relacionamentos Many-to-Many entre Usuários, Roles e Permissions, refletindo uma arquitetura escalável no banco de dados.
- **Refatoração Arquitetural (DDD):** Reorganização do código de identidade e segurança para atender aos padrões de Domain-Driven Design, separando as preocupações de infraestrutura da lógica de domínio.
- **Normalização de Tipos Primitivos:** Implementação de Casts SQL customizados para compatibilização de tipos enumerados entre a camada de persistência (PostgreSQL) e o C#.

## 2. Quantidade de horas

- **Ana Beatriz:** Total 37:35 horas

## 3. Evidências de Progresso

- **Repositório GitHub:** [Projeto no GitHub](https://github.com/ana-rangel-mattos/enem-prep-platform)
- **Middleware de Segurança:** Implementação do PermissionAuthorizationHandler no projeto de Infrastructure.
- **Persistência:** Schema auth atualizado com as tabelas user_role e role_permission.
- **Logs de Migrations:** Snapshot do Entity Framework Core refletindo o novo mapeamento Many-to-Many.

## 4. Quais foram os principais avanços?

- **Segurança por Camadas:** O sistema agora diferencia autenticação básica (acesso à plataforma) de autorização administrativa (gestão de conteúdo/questões) de forma automática.
- **Flexibilidade de Cargos:** Um usuário pode agora acumular múltiplas funções (ex: Admin e Student) herdando permissões de ambas as fontes.
- **Independência de Framework:** A lógica de permissões foi desacoplada do ASP.NET Identity padrão, permitindo controle total sobre o schema do banco de dados e as regras de negócio.

## 5. Quais dificuldades você encontrou?

- **Incompatibilidade de Tipos EF Core vs. Postgres**: O mapeamento nativo de Enums do C# apresentou falhas na conversão para tipos enumerados do PostgreSQL. A solução exigiu a implementação de Casts manuais em nível de banco de dados e ajustes na Fluent API para garantir a integridade dos dados.
- **Complexidade de Ciclo de Vida (Lifetimes)**: Dificuldades na gestão de dependências (Scoped vs Singleton) dentro do middleware de autorização, resolvidas através do uso de IServiceScopeFactory para acessar serviços de sessão com segurança.

## 6. O que planeja fazer até o próximo check-in?

- **Ana Beatriz:** Iniciar o desenvolvimento do módulo de Simulados (Principal feature do Projeto), implementar os primeiros testes de integração utilizando TestContainers para validar o fluxo de autorização e iniciar a interface de Dashboard do Estudante.

# Registro de Acompanhamento - 15/05/2026

**Projeto:** Website para auxiliar estudantes do ENEM
**Estudante:** Ana Beatriz Rangel Mattos
**Status:** Em Desenvolvimento (Fase de Implementação de Serviços e Testes)

## 1. O que foi desenvolvido durante este período?

- **Sistema de Convite (Invite System):** Implementação de fluxo administrativo que permite a geração de códigos únicos para convite de novos usuários com cargos específicos.
- **Implementação de TestContainers:** Configuração e debugging do ambiente Docker para instanciar bancos PostgreSQL efêmeros durante a execução dos testes.
- **Ciclo de Vida (IAsyncLifetime):** Implementação de setup e teardown assíncronos para garantir o isolamento atômico de cada caso de teste.
- **Limpeza de Dados (Respawn):** Integração da biblioteca Respawn para resetar o estado do banco entre os testes sem necessidade de recriar o container.
- **Refatoração do AuthController:** Ajuste do controlador de autenticação para suportar os requisitos dos testes de integração e correção de fluxos de login/registro.
- **Segurança e Autorização Customizada:** Implementação de um **Authentication Scheme customizado** com um **Session Authorization Handler**. Esta mudança foi crucial para resolver erros de `405 Method Not Allowed` e substituir a autenticação padrão baseada em cookies por uma lógica vinculada à sessão do sistema.
- **Serviço de Questões (Question Service):**
  - Implementação de busca com **Paged Response** (respostas paginadas).
  - Uso extensivo de `IQueryable` e `QueryableExtensions` para filtragem eficiente.
  - Criação de um objeto de filtro de consulta (_Question Query Filter_) para buscas dinâmicas.
- **Mapeamento de Enums no PostgreSQL:** Refatoração da configuração do `NpgsqlDataSource` no .NET 9 para suporte nativo a tipos enumerados no banco de dados.
- **População de Dados:** Script Python utilizando `psycopg2` para ingestão de questões via API externa para a base de dados local.

## 2. Quantidade de horas

- **Ana Beatriz:** Total 59:00 horas

## 3. Evidências de Progresso

- **Repositório GitHub:** [Projeto no GitHub](https://github.com/ana-rangel-mattos/enem-prep-platform)
- **Suíte de Testes:** Implementação de `QuestionsControllerTests` e `AuthControllerTests` utilizando `WebApplicationFactory`.

## 4. Quais foram os principais avanços?

- **Estabilização da Infraestrutura de Testes:** A resolução dos problemas com o `Respawner` e o ciclo de vida do Docker agora permitem que o projeto cresça com segurança através de testes automatizados.
- **Motor de Busca de Questões:** A implementação de paginação e filtros dinâmicos via `IQueryable` permite que a plataforma suporte grandes volumes de dados (ENEM) sem perda de performance.
- **Segurança Granular:** A transição para um `Authorization Handler` customizado permite um controle muito mais fino sobre permissões (`HasPermission`) do que o sistema padrão de cookies.

## 5. Quais dificuldades você encontrou?

- **Gerenciamento de Estado no Docker:** Grande dificuldade em garantir que o TestContainers deletasse os dados corretamente entre as execuções, o que exigiu uma reconfiguração profunda do `Respawner` e das permissões de esquema.
- **Mapeamento de Tipos Complexos:** O uso de Enums do PostgreSQL com o Entity Framework Core exigiu ajustes finos na versão do driver para garantir que o banco reconhecesse os tipos customizados (`content.language`, etc).
- **Shadowing de Autenticação:** Conflitos entre o esquema de autenticação padrão e a lógica de sessão customizada resultaram em erros de `405 Method Not Allowed`, resolvidos com a implementação de um esquema de autenticação proprietário.

## 6. O que planeja fazer até o próximo check-in?

- **Ana Beatriz:** Implementar o Serviço de Exames (`Exams Service`).
- **Ana Beatriz:** Implementar um serviço para a submissão de respostas de questões.
- **Ana Beatriz:** Iniciar a construção da interface Front-End para a listagem paginada de questões.
- **Ana Beatriz:** Implementar o dashboard administrativo para visualização de métricas de uso dos códigos de convite.

# Registro de Acompanhamento - 05/06/2026

**Projeto:** Website para auxiliar estudantes do ENEM
**Estudante:** Ana Beatriz Rangel Mattos
**Status:** Em Desenvolvimento (Fase de Implementação da Interface)

## 1. O que foi desenvolvido durante este período?

- **Global Exception Handling Middleware:** Implementação de um Middleware para cuidar de Excessões inesperadas da infraestrutura de forma segura.
- **Result Pattern:** Implementação da arquitetura genérica `Result<T>`, método extensão `Match` para ser usado nos controlers e refatoração dos blocos de código principais para remover `exception-throwing` para regras de negócio.
- **Saved Question Service, Controller e Paginatação:** Implementação das rotas do Controller, implementação do serviço com o backend com o `IQueryable` que serve dados eficientemente para o frontend.
- **Saved Questions Database Mapping & EF Migration:** Definição das relações em `SavedQuestionConfiguration`, adcionado um `DbSet` ao `DbContext`, execução e aplicação da migração EF.
- **User Goal Controller e Serviços:** Design as regras de domínio para as métricas do estudante, processando a regra de négocio (como enforçar o limite de um objetivo), e a criação das rotas correspondentes.
- **Subjects Service, Controller:** Foi contruído o serviço `SubjectService` e controller `SubjectsController`.
- **Refatoração do SolvedQuestionDto:** Adicionar enunciado, alternativa correta e escolhida ao SolvedQuestionDto.
- **Refatoração do SolvedQuestionsService:** Vários controllers estavam usando as funcionalidades de converter STRING para JsonElement, então a solução encontrada foi criar métodos de extensão para algumas classes como `Question` model.
- **Implementação do SavedQuestionService:** Foi implementado um serviço que permite ao usuário salvar questões para revisar mais tarde.
- **SavedQuestionsController:** Implementação das rotas do `SavedQuestionService`.
- **Planejamento dos próximos passos:** Planejamento da entrega final com os items prioritários e essenciais.
- **Exams Service:** Implementação de um serviço complexo de simulados, que têm persistência de respostas e avaliação do desempenho do estudante de forma mais realista possível.
- **Exams Controller:** Implementação das rotas do serviço `ExamsService`.
- **UserPreferences Service & Controller:** Implementação de um serviço e controller para as preferências do usuário.
- **Scaffold e configuarção do Front-End:** Scaffold com Next, shadcn/ui e tailwind, começo da implementação da sidebar.
- **Funcionalidade UserService:** Foi implementado um método ao serviço do usuário que permite buscar informações sobre um usuário específico e a implementação da rota no controller.
- **Solução dos problemas com Enums:** Através da implementação de Value Converters customizados no EF Core, garantindo compatibilidade com APIs externas em português.

## 2. Quantidade de horas

- **Ana Beatriz:** Total 87:45 horas

## 3. Evidências de Progresso

- **Repositório GitHub:** [Projeto no GitHub](https://github.com/ana-rangel-mattos/enem-prep-platform)
- **Planejamento para a entraga final:** [Planejamento final](../1%20-%20Planejamento/02-planejamento_entrega.md)

## 4. Quais foram os principais avanços?

- **Finalização do Motor de Simulados:** Implementação completa da lógica de geração de provas, persistência de respostas e o ScoreCalculator para cálculo de desempenho.
- **Arquitetura de Contexto de Usuário:** Criação do UserContext para abstrair a identificação do usuário logado em toda a camada de serviço, aumentando a segurança.
- **Migração e Setup de Front-End Moderno:** Decisão técnica de migrar o cliente de uma SPA simples (Vite) para um framework robusto (Next.js), garantindo melhor performance com Server-side Rendering e segurança via Middleware de borda.
- **Resolução de Conflitos de Infraestrutura de Dados:** Superação do impasse técnico com Enums nativos do PostgreSQL através da implementação de Value Converters customizados no EF Core, garantindo compatibilidade com APIs externas em português.
- **Saneamento de Dados no DTO:** Expansão do SolvedQuestionDto para incluir enunciados e alternativas, permitindo que o estudante revise suas escolhas e aprenda com os erros.

## 5. Quais dificuldades você encontrou?

- **Incompatibilidade de Tipos no Banco de Dados:** O driver Npgsql apresentou instabilidade ao mapear tipos Enum nativos do PostgreSQL para o C#, resultando em erros de "expression is of type text". A dificuldade foi solucionada através de uma mudança na estratégia de persistência para text com conversores de valor no mapeamento do EF Core.
- **Configuração de Ambiente Front-End:** Dificuldade inicial na organização do monorepo ao tentar mesclar configurações de build do Vite com as convenções do Next.js. A solução exigiu o deleção do diretório anterior e um novo scaffold limpo utilizando Shadcn/UI e Tailwind v4.
- **Gestão de Escopo vs. Prazo:** O volume de regras de negócio para o cálculo de pontuação e filtragem dinâmica de questões demandou mais tempo do que o previsto, exigindo foco total na estabilidade do motor do sistema em detrimento de funcionalidades secundárias.

## 6. O que planeja fazer até o próximo check-in?

- **Ana Beatriz:** Contrução da interface Front-End para a listagem paginada de questões.
- **Ana Beatriz:** Implementar o Dashboard Administrativo para visualização de métricas e gerenciamento de códigos de convite.
- **Ana Beatriz:** Configurar o AuthProvider no Next.js para persistência de sessão e proteção de rotas privadas via Middleware.

# Registro de Acompanhamento - 15/06/2026

**Projeto:** Website para auxiliar estudantes do ENEM
**Estudante:** Ana Beatriz Rangel Mattos
**Status:** Concluído (Entrega e Encerramento do Escopo)

## 1. O que foi desenvolvido durante este período?

- **Gerenciamento de Estado de Autenticação (AuthSlice):** Implementação e estruturação do slice de autenticação com Redux Toolkit para controle síncrono do perfil do usuário e estado global de login.
- **Formulário e Página de Login:** Construção da interface visual da página de login integrada com validações de campos e feedback visual.
- **Formulário e Página de Cadastro (Register):** Desenvolvimento da tela de registro de novos usuários com mapeamento dos campos necessários para persistência no banco de dados.
- **Páginas de Questões e Resolução (Iniciadas):** Estruturação da casca e dos componentes visuais estáticos para a listagem geral de questões e exibição de alternativas.
- **Customização do Tema da Aplicação:** Integração e aplicação de um tema escuro exclusivo de alto contraste via Tailwind v4 e Shadcn UI, priorizando a ergonomia visual do estudante.
- **Refatoração do Client de API e Logout:** Ajustes na camada de requisições do Axios e implementação do fluxo de encerramento de sessão (Sign-out) com expurgo de cookies.

## 2. Quantidade de horas

- **Ana Beatriz:** Total 104:45 horas

## 3. Evidências de Progresso

- **Repositório GitHub:** [Projeto no GitHub](https://github.com/ana-rangel-mattos/enem-prep-platform)

## 4. Quais foram os principais avanços?

- **Ciclo de Vida do Usuário Completo:** Fechamento do fluxo de ponta a ponta no cliente (registro de usuário, login, inicialização da sessão com persistência no recarregamento e logout seguro).
- **Consolidação da Identidade Visual:** Padronização estética profissional em todo o ecossistema do front-end com um design moderno e responsivo.
- **Infraestrutura Pronta para Componentes de Questões:** Criação das primitivas de renderização e componentes utilitários (como exibições estáticas de alternativas), deixando a fundação preparada para renderizar dados complexos.

## 5. Quais dificuldades você encontrou?

- **Gestão de Projeto na Reta Final:** Os prazos e outras avaliações acadêmicas limitaram o tempo. A dificuldade foi superada através de uma decisão consciente de congelamento de escopo, optando por garantir a estabilidade e a qualidade das rotas principais (Autenticação e Questões) em vez de entregar a lógica de resolução de simulados incompleta.
