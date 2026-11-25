# EnemPrep

EnemPrep is a full-stack study and exam preparation platform built with **ASP.NET Core (.NET)**, **Entity Framework Core**, **PostgreSQL**, and **Next.js (React, TypeScript, Tailwind CSS)**. It features weekly study schedule management with drag-and-drop Kanban reordering, exam/question practice, and a rich-text article and playlist authoring experience powered by TipTap.

---

## Architecture & Tech Stack

### Backend (`EnemPrep.Server`)
* **API Layer (`EnemPrep.Api`)**: REST controllers (`Auth`, `Schedules`, `Subjects`, `Articles`, `Playlists`, `Exams`, `Questions`, `SolvedQuestions`, `User`, `UserGoal`, `UserPreferences`, `UserProfile`), global exception middleware, Swagger/OpenAPI documentation, and session-based authentication/authorization.
* **Application Layer (`EnemPrep.Application`)**: Feature-organized business logic using plain services and interfaces (`IScheduleService`, `IArticleService`, `IPlaylistService`, etc.), DTOs, mapping extensions, pagination/filtering utilities, and domain result handling.
* **Domain Layer (`EnemPrep.Domain`)**: Core entities, enums, permissions, and domain error definitions.
* **Infrastructure & Persistence (`EnemPrep.Infrastructure`, `EnemPrep.Persistence`)**: EF Core with PostgreSQL, database migrations, session/permission authorization handlers, and `UserContext`.
* **Testing (`EnemPrep.Tests`)**: Integration testing suite built with `WebApplicationFactory`.

### Frontend (`enemprep.client`)
* **Framework & Styling**: Next.js (App Router), React, TypeScript, and Tailwind CSS.
* **Study Scheduler Kanban**: Custom drag-and-drop board built with `@dnd-kit/react`, paired with schedule selection, editing, and subject creation popovers.
* **Rich-Text Editor**: TipTap-based editor (`ArticleEditor`) with custom extensions (such as playlist blocks) and article creation page (`/articles/new`).

---

## Key Features

* **Weekly Study Schedules**: Create and manage study schedules, toggle weekend inclusion, add subjects to specific weekdays (including repeating a subject across multiple days), and reorder/move items across columns with drag-and-drop synced to the backend.
* **Content & Playlists**: Organize subjects, articles, and playlists, with an interactive editor for creating articles.
* **Exams & Question Bank**: Browse questions, take exams, track solved questions, and configure study goals and preferences.
* **Session Authentication & Permissions**: Cookie-based session authentication with permission-based endpoint authorization.

---

## Running with Docker

The repository provides Dockerfiles and Docker Compose configurations for running the full stack or individual services.

### Prerequisites
* [Docker](https://docs.docker.com/get-docker/) and [Docker Compose](https://docs.docker.com/compose/) installed.

### 1. Clone this Application
Clone the repository with git and enter in the root directory.

```bash
git clone https://github.com/ana-rangel-mattos/enem-prep-platform.git
```

```bash
cd enem-prep-platform
```

### 2. Fill the .env files

* Filling the `.env.example` of each project (when necessary) and renaming it to `.env`.
  * `infra/seed_db/.env.example`.
  * `infra/postgres/.env.example`.
  * `enemprep.client/.env.example`.
  * `EnemPrep.Server/.env.example`.

### 3. Run the Full Stack
From the root of the repository, start the services using the root `compose.yaml`:

```bash
docker compose up --build -d
```

To remove the container:

```bash
docker compose down -v
```
