namespace TaskManagementSystem.Web.Models;

public static class TaskStatusExtensions
{
    public static string ToDisplayName(this TaskStatus status)
    {
        return status switch
        {
            TaskStatus.New => "New",
            TaskStatus.InProgress => "In Progress",
            TaskStatus.Completed => "Completed",
            _ => status.ToString(),
        };
    }
}
