# enem-prep-platform
Plataforma de Preparação para o ENEM (Projeto de Extensão) Desenvolvimento de uma ferramenta Full-Stack voltada para a democratização do acesso ao estudo para o ENEM.

## 🛠️ Pré-requisitos
- PostgreSQL (porta padrão `5432`)
- .NET SDK (Versão 9)
- Node.js & pnpm
- Python 3.x

## Configurar a Applicação:

1. Abra o seu banco de dados de preferência e execute o seguite comando:
```postgres
CREATE DATABASE enem_prep_db;
```

2. Fazer o Update do Banco de Dados no Back-End:
    - Fazer o update do Banco de Dados com o usuário postgres e subistiruir a senha:
    ```bash
    cd EnemPrep.Server/
    dotnet ef database update --project EnemPrep.Server/EnemPrep.Server.csproj --startup-project EnemPrep.Server/EnemPrep.Server.csproj --context EnemPrep.Persistence.EnemContext --configuration Debug --framework net9.0 20260521195544_Add_Saved_Question_Table --connection Host=localhost;Database=enem_prep_db;Username=postgres;Password=789633;
    ```

3. Com o banco de dados criado, execute os seguites scripts SQL:
```bash
cd ../sql_scripts
```
`castings.sql`
`privileges.sql`
`updated_at.sql`
`seed_admin.sql`
`user_streak.sql`

4. Execute o script python para popular a tabela das questões:
```bash
cd ../python_script
```
Preencha as informações do `.env.sample` com as informações do Postgres e o renomeie para `.env`:
```env
DB_USER=postgres
DB_DATABASE=enem_prep_db
DB_PASSWORD=
DB_PORT=5432
DB_HOST=localhost

DB_LANGUAGES_ID=
DB_SOCIAL_SCIENCES_ID=
DB_NATURAL_SCIENCES_ID=
DB_MATH_ID=
DB_POSTED_BY_ID=f1181460-9376-4bb7-9e3a-b4130fd17bf8
```

```bash
uv sync
python3 main.py
```

5. Inicie o Back-End:
```bash
cd ../EnemPrep.Server/EnemPrep.Server
dotnet run
```

6. Inicie o Front-End:
```bash
cd ../enemprep.client
pnpm install
pnpm dev
Abra http://localhost:3000 no navegador.
```
