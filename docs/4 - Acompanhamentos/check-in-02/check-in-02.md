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
