# KT2: Architecture and Design

## Chosen Variant

`Task Management System` from recommended themes.

## Main Modules

1. `Controllers`
   - `TasksController`: CRUD routes, filter and pagination interaction.
2. `Services`
   - `ITaskService`, `TaskService`: business operations and data flow.
3. `Data`
   - `AppDbContext`: EF Core mapping.
   - `DbSeeder`: initial records.
4. `Models`
   - `TaskItem`, `TaskStatus`, query/paging models.
5. `Views`
   - Razor pages for list/forms/details.

## Data Model

Entity `TaskItem`:

- `Id: Guid`
- `Name: string (3..50)`
- `Description: string? (<=255)`
- `Status: enum {New, InProgress, Completed}`
- `Assignee: string? (<=50)`
- `CreatedAt: DateTime`
- `UpdatedAt: DateTime`

## CRUD Logic

- Create: validate and normalize input, set timestamps.
- Read: list with search/status filter/pagination.
- Update: update mutable fields and `UpdatedAt`.
- Delete: remove selected entity with confirmation view.

## Database

- Provider: SQLite
- ORM: Entity Framework Core
- Startup action: `EnsureCreated` + seed

## Non-functional Requirements

- Modular layers with separation of concerns.
- Structured logging to console and rolling file.
- Validation through data annotations.
- Testable service layer with in-memory DB provider.
