# KT1: Work Plan

## Objective

Build an autonomous CRUD application with relational storage, tests, logging and deliverables for KT1-KT4.

## Scope

- Implement `Task Management System` (recommended variant).
- Build web interface with task list, creation, editing, deletion.
- Add search, filtering by status and pagination.
- Add persistence and seed data.
- Provide tests and documentation.

## Stages and Timeline

| Stage | Tasks | Duration | Deliverable |
| --- | --- | --- | --- |
| 1. Requirement decomposition | Analyze assignment and acceptance criteria | 4h | Scope statement |
| 2. Architecture and DB design | Define modules, entities, data flow | 8h | KT2 architecture doc |
| 3. Implementation | Build MVC pages, services, persistence | 16h | Working project |
| 4. Testing | Unit/scenario tests, fixes | 8h | Test suite and report |
| 5. Documentation | Final report and presentation material | 4h | KT3 and KT4 docs |

Total: 40 hours.

## Resources

- .NET SDK 9
- IDE (Visual Studio or VS Code)
- SQLite
- NuGet packages (`Microsoft.EntityFrameworkCore.*`, `Serilog.*`, `xUnit`)

## Risks and Mitigation

- Version incompatibility: pin EF Core to `9.0.3`.
- Scope growth: keep one domain entity and complete it end-to-end.
- Test instability: isolate tests via unique in-memory DB names.
