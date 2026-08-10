# Casos de uso

[UC 01 - Criar conta](#caso-de-uso-01---criar-conta)
[UC 02 - Fazer Login](#caso-de-uso-02---fazer-login)
[UC 03 - Preferências de Usuário](#caso-de-uso-03---preferências-de-usuário)
[UC 04 - Resolver questões em modo estudo livre](#caso-de-uso-04---resolver-questões-em-modo-estudo-livre)
[UC 05 - Realizar simulado](#caso-de-uso-05---realizar-simulado)
[UC 06 - Visualizar Resultado do Simulado](#caso-de-uso-06---visualizar-resultado-do-simulado)
[UC 07 - Acessar as estatísticas](#caso-de-uso-07---acessar-as-estatísticas)
[UC 08 - Acessar histórico](#caso-de-uso-08---acessar-o-histórico)
[UC 09 - Criar e Gerenciar cronograma semanal](#caso-de-uso-09---criar-e-gerenciar-cronograma-semanal)
[UC 10 - Marcar questões para revisar mais tarde](#caso-de-uso-10---marcar-questões-para-revisar-mais-tarde)
[UC 11 - Definir meta de aprovação](#caso-de-uso-11---definir-meta-de-aprovação)

## Caso de Uso 01 - Criar Conta

### Ator:

Aluno

### Descrição:

Permite que o aluno crie uma conta no sistema para que seu progresso seja salvo.

### Fluxo Principal

1. Aluno acessa a página de cadastro;
2. Informa os dados necessários para fazer o cadastro (email, username, senha, nome completo, data de nascimento);
3. Sistema valida os dados;
4. Sistema cria a conta;
5. O aluno é redirecionado para a página de login.

## Caso de Uso 02 - Fazer Login

### Ator:

Aluno

### Descrição:

Permite que o aluno acesse sua conta no sistema.

### Fluxo Principal:

1. Aluno informa o email e senha;
2. O sistema valida as credenciais passadas pelo aluno;
3. O sistema autentica o usuário;
4. O aluno acessa o sistema.

## Caso de Uso 03 - Preferências de Usuário

### Ator:

Aluno

### Descrição:

Permite que o aluno modifique suas preferências no sistema.

### Fluxo Principal:

1. Aluno acessa a área de preferências;
2. O aluno escolhe o idioma extrangeiro preferido (inglês ou espanhol);
3. O aluno escolhe o número de questões que deseja resolver diariamente (por área de conhecimento);
4. O aluno escolhe o tema da aplicação web (light, dark ou default OS);
5. O sistema salva as preferências do aluno.

## Caso de Uso 04 - Resolver questões em modo estudo livre

### Ator:

Aluno

### Descrição:

Permite que o aluno estude resolvendo questões com feedback imediato.

### Fluxo Principal:

1. O aluno acessa o modo estudo livre;
2. O aluno escolhe os filtros (anos, áreas, idioma) e o número de questões desejadas;
3. O aluno responde a questão;
4. O sistema informa com feedback visual se o aluno acertou ou errou;
5. O sistema registra o resultado no banco de dados;
6. O aluno passa para a próxima questão;

## Caso de Uso 05 - Realizar simulado

### Ator:

Aluno

### Descrição:

Permite que o aluno simule uma prova do ENEM com tempo controlado.

### Fluxo Principal:

1. O aluno acessa a área de simulados;
2. O aluno escolhe:
   1. O idioma;
   2. Áreas do conhecimento.
3. Sistema inicia o cronometro;
4. Aluno responde às questões;
5. O aluno finaliza o simulado ou o tempo se esgota;
6. Sistema encerra o simulado;
7. Sistema calcula o resultado do exame estimado;
8. Sistema registra o resultado no banco de dados.

## Caso de Uso 06 - Visualizar Resultado do Simulado

### Ator:

Aluno

### Descrição:

Apresenta o desempenho do aluno em um simulado.

### Fluxo Principal:

1. Sistema retorna um feedback de um simulado realizado contendo:
   1. Número de questões;
   2. Acertos, erros e Questões não respondidas;
   3. Nota estimada;
   4. Desempenho por área;
   5. Tempo total gasto na prova;
   6. Tempo médio gasto por questão;
2. Aluno pode visualizar a lista de erros com:
   1. Enunciado da questão;
   2. Alternativa correta;
   3. Alternativa escolhida;
   4. Tempo gasto;

## Caso de Uso 07 - Acessar as estatísticas

### Ator:

Aluno

### Descrição:

Permite acompanhar o progresso ao longo do tempo.

### Fluxo Principal:

1. O aluno acessa as estatísticas;
2. Sistema apresenta as estatísticas:
   1. Percentual de acertos por área;
   2. Por idioma;
   3. Por ano.
3. O aluno analisa os dados apresentados.

## Caso de Uso 08 - Acessar o histórico

### Ator:

Aluno

### Descrição:

Permite que o aluno acompanhe seu histórico de questões já resolvidas.

### Fluxo Principal:

1. O aluno acessa o histórico;
2. Sistema apresenta questões já resolvidas pelo aluno no modo estudo livre, contendo:
   1. Enunciado da questão;
   2. Alternativa escolhida;
   3. Alternativa correta;
   4. Tempo gasto na questão.
3. O aluno tem a opção de revisar a questão;

## Caso de Uso 09 - Criar e Gerenciar cronograma semanal

### Ator:

Aluno

### Descrição:

Permite que o aluno organize os estudos por meio de um cronograma semanal.

### Fluxo Principal:

1. O aluno acessa o cronograma;
2. Distribui as áreas de conhecimento nos dias da semana;
3. Sistema salva o cronograma;
4. Sistema distribui questões diárias por área de conhecimento;
5. Aluno visualiza o cronograma semanal;

## Caso de Uso 10 - Marcar questões para revisar mais tarde

### Ator:

Aluno

### Descrição:

Permite que o aluno marque questões para uma revisão futura.

### Fluxo Principal:

1. O aluno acessa uma questão;
2. O aluno marca essa questão para revisão;
3. O sistema adiciona essa questão para a lista de questões em revisão;
4. Aluno acessa a lista de revisão;
5. Aluno aplica filtros (questões erradas, certas, não respondidas).

## Caso de Uso 11 - Definir meta de aprovação

### Ator:

Aluno

### Descrição:

Permite que o aluno defina um objetivo acadêmico.

### Fluxo Principal:

1. O aluno acessa as metas;
2. O preenche um formulário contendo:
   1. Universidade desejada;
   2. Curso desejado;
   3. Nota de corte da modalidade que ele ocupa.
3. O sistema salva as metas;
4. O aluno acompanha o progresso em relação ao seu objetivo.
