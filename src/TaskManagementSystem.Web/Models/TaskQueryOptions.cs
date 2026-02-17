namespace TaskManagementSystem.Web.Models;

public sealed class TaskQueryOptions
{
    public string? SearchTerm { get; init; }

    public TaskStatus? Status { get; init; }

    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}
