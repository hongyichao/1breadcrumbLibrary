# 1Breadcrumb Library App

A Crumb-to-Crumb book lending library — lets employees see who owns which books and borrow/return them.

## Tech Stack

| Layer | Tech |
|-------|------|
| Backend API | ASP.NET Core 8 Web API (C#) |
| Database project | EF Core 8 + Pomelo MySQL provider |
| Database | MySQL 8.0 (Docker) |
| Logging | Serilog (console + rolling file) |
| Frontend | React 18 + TypeScript (Create React App) |
| Unit tests | xUnit + Moq |
| Integration tests | xUnit + WebApplicationFactory + SQLite in-memory |

## Project Structure

```
1Breadcrumb/
├── docker-compose.yml          # MySQL container
├── README.md
├── backend/
│   ├── LibraryApp.sln
│   ├── LibraryApp.Api/         # Controllers, Repositories, Models, Middleware
│   ├── LibraryApp.Database/    # EF DbContext, Entities, Migrations
│   ├── LibraryApp.Tests.Unit/  # Controller unit tests (Moq)
│   └── LibraryApp.Tests.Integration/  # HTTP-level tests (SQLite in-memory)
└── frontend/                   # React + TypeScript (CRA)
```

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [Node.js 18+](https://nodejs.org/)
- `dotnet-ef` CLI tool (installed below)

---

## Quick Start

### 1. Start MySQL via Docker

```bash
docker compose up -d
```

This starts a MySQL 8.0 container on port `3306` with:
- Database: `library_db`
- User: `library_user` / Password: `library_password`

Check it's healthy:

```bash
docker compose ps
```

### 2. Install EF Core CLI (first time only)

```bash
dotnet tool install --global dotnet-ef
```

### 3. Apply the database migration

```bash
cd backend
dotnet ef database update \
  --project LibraryApp.Database/LibraryApp.Database.csproj \
  --startup-project LibraryApp.Api/LibraryApp.Api.csproj
```

This creates the `Books` table and inserts seed data (5 sample books).

### 4. Run the API

```bash
cd backend
dotnet run --project LibraryApp.Api/LibraryApp.Api.csproj
```

API listens on `http://localhost:5110`.

### 5. Run the Frontend

```bash
cd frontend
npm install
npm start
```

Opens at `http://localhost:3000`.

---

## API Endpoints

| Method | Path | Description |
|--------|------|-------------|
| GET | `/api/books` | List books (optional `?search=` and `?isAvailable=true/false`) |
| GET | `/api/books/{id}` | Get a single book |
| POST | `/api/books` | Add a book |
| PUT | `/api/books/{id}` | Update a book |
| DELETE | `/api/books/{id}` | Delete a book |
| PATCH | `/api/books/{id}/availability` | Toggle borrow/return |

---

## Running Tests

### Unit tests (no Docker required)

```bash
cd backend
dotnet test LibraryApp.Tests.Unit/LibraryApp.Tests.Unit.csproj
```

### Integration tests (no Docker required — uses SQLite in-memory)

```bash
cd backend
dotnet test LibraryApp.Tests.Integration/LibraryApp.Tests.Integration.csproj
```

### All tests

```bash
cd backend
dotnet test
```

---

## EF Core Commands Reference

```bash
# Create a new migration after changing entities
dotnet ef migrations add <MigrationName> \
  --project LibraryApp.Database/LibraryApp.Database.csproj \
  --startup-project LibraryApp.Api/LibraryApp.Api.csproj

# Apply pending migrations to the database
dotnet ef database update \
  --project LibraryApp.Database/LibraryApp.Database.csproj \
  --startup-project LibraryApp.Api/LibraryApp.Api.csproj

# Roll back to a specific migration
dotnet ef database update <MigrationName> \
  --project LibraryApp.Database/LibraryApp.Database.csproj \
  --startup-project LibraryApp.Api/LibraryApp.Api.csproj

# Drop the database entirely
dotnet ef database drop \
  --project LibraryApp.Database/LibraryApp.Database.csproj \
  --startup-project LibraryApp.Api/LibraryApp.Api.csproj

# Remove the last migration (if not yet applied)
dotnet ef migrations remove \
  --project LibraryApp.Database/LibraryApp.Database.csproj \
  --startup-project LibraryApp.Api/LibraryApp.Api.csproj
```

---

## Docker Commands Reference

```bash
# Start MySQL in background
docker compose up -d

# Stop MySQL
docker compose stop

# Stop and remove container + volume (resets all data)
docker compose down -v

# View MySQL logs
docker compose logs mysql

# Connect to MySQL shell
docker exec -it library_mysql mysql -u library_user -plibrary_password library_db
```

---

## Logs

API logs are written to the **console** using Microsoft's built-in logging.
Log levels are configured in `appsettings.json` under the `Logging` section.
