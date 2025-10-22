```instructions
# Copilot Instructions — eUIT (workspace-specific)

This repo contains a .NET 8 Web API backend (clean architecture) and a React Native mobile frontend. Key runtime pieces:
- Backend: `src/backend` (ASP.NET Core 8, EF Core, PostgreSQL)
- Mobile: `src/mobile` (React Native app entry `App.js`, `package.json`)
- DB seeds & SQL scripts: `scripts/database/sql` and `scripts/database/main_data`

Practical rules for AI contributors (concise):
1. Preserve API surface: controllers live in `src/backend/Controllers` and return DTOs in `src/backend/DTOs`.
2. Database access for custom queries uses raw SQL functions called via EF Core Database.SqlQuery<T>("SELECT * FROM func_xxx(...)"). Match the returned anonymous/class shape to the SQL result columns (see `StudentsController.cs`).
3. Migrations are in `src/backend/Migrations`. When adding new entities: add DbContext mapping in `src/backend/Data/eUITDbContext.cs` and create migrations via dotnet-ef.
4. Authentication: controllers are annotated with `[Authorize]` and JWT claims are used (see `StudentsController.cs` — ClaimTypes.NameIdentifier contains `mssv`). Do not remove auth attributes unless deliberate.
5. File serving: static files are referenced by path stored in DB and assembled in controllers with `Request.Scheme` + `Request.Host` + `/files/` prefix; keep this pattern when changing avatars or file endpoints.

Build, test, run (developer commands you can call):
- Build backend: `dotnet build src/backend` (Windows PowerShell syntax)
- Run backend locally: `dotnet run --project src/backend` or via IDE launch profiles
- Run migrations: `dotnet ef migrations add <Name> -p src/backend -s src/backend` and `dotnet ef database update -p src/backend -s src/backend`
- Run mobile app: `cd src/mobile; npm install; npx react-native start` (or use `package.json` scripts)
- Tests: `dotnet test tests` (runs test projects under `tests/`)

Project-specific conventions & patterns to follow:
- DTOs are simple POCOs in `src/backend/DTOs`. Controllers map query results to DTOs explicitly (avoid returning EF entities directly).
- Use `Database.SqlQuery<T>("SELECT * FROM func_name(...)")` for Postgres SQL functions that return composite types; create small private classes in the controller matching the column names.
- Error handling: controllers typically return `Forbid()`, `NotFound()`, `NoContent()` or `Ok(...)`. Preserve existing status code semantics when refactoring.
- Prefer async EF Core methods (e.g., `ToListAsync()`, `FirstOrDefaultAsync()`) throughout.

Key files to inspect for context:
- `src/backend/Program.cs` — app startup and DI registrations
- `src/backend/Data/eUITDbContext.cs` — DbContext mappings and DBSets
- `src/backend/Controllers/StudentsController.cs` — examples of raw SQL function calls, DTO mapping, and auth usage
- `scripts/database/sql/init.sql` and other `scripts/database` files — DB initialization and function definitions used by the app

If you change DB-related code:
- update `src/backend/Migrations` with an EF Core migration and run update; include a SQL script under `scripts/database/sql` when you modify DB functions or views.

When unsure, prefer small, reversible edits and include tests or a minimal manual verification: run the backend and call the modified endpoint with a test JWT (tests exist in `tests/`).

Ask for clarification if you need database credentials, deployment variables, or mobile build configurations — they are not stored in this repo.

``` 
