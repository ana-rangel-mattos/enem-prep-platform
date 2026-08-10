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
