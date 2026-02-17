using TaskManagementSystem.Web.Models;
using TaskState = TaskManagementSystem.Web.Models.TaskStatus;

namespace TaskManagementSystem.Web.ViewModels;

public sealed class TaskListViewModel
{
    public required PagedResult<TaskItem> PagedTasks { get; init; }

    public string? SearchTerm { get; init; }

    public TaskState? Status { get; init; }

    public IReadOnlyList<TaskState> Statuses { get; init; } = Array.Empty<TaskState>();
}
