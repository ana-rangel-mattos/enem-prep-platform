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
