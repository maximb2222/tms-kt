# Task Management System

Training project for CRUD development on .NET.

## Stack

- ASP.NET Core MVC (.NET 9)
- Entity Framework Core + SQLite
- Serilog (console + rolling file logs)
- xUnit + EF Core InMemory for tests

## Features

- Full CRUD for `TaskItem`
- Search and filter by status
- Pagination
- Data validation (length limits and required fields)
- Persistent storage in SQLite
- Structured logging in `logs/`

## Domain Model

`TaskItem` fields:

- `Id` (`Guid`)
- `Name` (3-50 chars)
- `Description` (up to 255 chars)
- `Status` (`New`, `InProgress`, `Completed`)
- `Assignee` (optional, up to 50 chars)
- `CreatedAt`
- `UpdatedAt`

## Run

```bash
dotnet restore
dotnet run --project src/TaskManagementSystem.Web
```

## Test

```bash
dotnet test
dotnet test --collect:"XPlat Code Coverage"
```

## Documentation

See the `docs/` directory for KT1-KT4 materials.
