# KT2: Testing Artifacts

## Strategy

- Unit tests for `TaskService`.
- Scenario-style test for full lifecycle.
- Data layer isolation with EF Core InMemory.

## Covered Scenarios

1. Create task with normalization and persistence.
2. Query with search + status + pagination.
3. Update existing task.
4. Update missing task.
5. Delete existing task.
6. Delete missing task.
7. End-to-end scenario: create -> update -> list -> delete.

## Execution

```bash
dotnet test
dotnet test --collect:"XPlat Code Coverage"
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./TestResults/coverage-services.xml /p:Include="[TaskManagementSystem.Web]TaskManagementSystem.Web.Services.*"
```

## Expected Result

- All tests pass.
- Coverage is focused on business logic branch behavior in `TaskService`.

## Actual Result

- Test cases: `7/7` passed.
- Service-layer coverage:
  - Line: `94.04%`
  - Branch: `78.57%`
  - Method: `85.71%`

## Notes

- Coverage threshold is assessed from test output artifacts.
- If stricter threshold enforcement is needed, add CI gate with `coverlet.msbuild`.
