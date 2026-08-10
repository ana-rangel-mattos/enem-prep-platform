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
- **Planejamento para a entrega final:** [Planejamento final](../../1%20-%20Planejamento/02-planejamento_entrega.md)

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
