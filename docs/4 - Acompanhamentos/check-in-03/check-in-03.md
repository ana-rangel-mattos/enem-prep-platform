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
- **Navegação de Salto (Skip Navigations):** Erros de configuração de chaves estrangeiras no relacionamento Muitos-para-Muitos do Entity Framework, solucionados com o mapeamento explícito via UsingEntity.

## 6. O que planeja fazer até o próximo check-in?

- **Ana Beatriz:** Iniciar o desenvolvimento do módulo de Simulados (Principal feature do Projeto), implementar os primeiros testes de integração utilizando TestContainers para validar o fluxo de autorização e iniciar a interface de Dashboard do Estudante.
