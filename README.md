# CC-APPSDEV22 Midterm Week 5 — Student Roster DB API

ASP.NET Core Web API for a University Student Roster System, upgraded from the Week 4 in-memory `List<Student>` to a SQLite database using Entity Framework Core (Code-First).

## Setup

```bash
dotnet tool install --global dotnet-ef
dotnet restore
dotnet ef database update
```

## Run

```bash
dotnet watch run
```

Open Swagger UI at `/swagger`. Data is stored in `studentroster.db` and persists across server restarts.
