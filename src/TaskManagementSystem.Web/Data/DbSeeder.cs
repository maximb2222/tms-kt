using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.Models;
using TaskState = TaskManagementSystem.Web.Models.TaskStatus;

namespace TaskManagementSystem.Web.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.Tasks.AnyAsync(cancellationToken))
        {
            return;
        }

        var now = DateTime.UtcNow;
        var items = new[]
        {
            new TaskItem
            {
                Name = "Define domain entities",
                Description = "Document task model and status model.",
                Status = TaskState.InProgress,
                Assignee = "Developer",
                CreatedAt = now.AddDays(-2),
                UpdatedAt = now.AddDays(-1),
            },
            new TaskItem
            {
                Name = "Implement CRUD endpoints",
                Description = "Build create, read, update and delete flows.",
                Status = TaskState.New,
                Assignee = "Developer",
                CreatedAt = now.AddDays(-1),
                UpdatedAt = now.AddDays(-1),
            },
            new TaskItem
            {
                Name = "Prepare documentation",
                Description = "Complete KT1-KT4 artifacts.",
                Status = TaskState.Completed,
                Assignee = "Analyst",
                CreatedAt = now.AddDays(-3),
                UpdatedAt = now.AddHours(-10),
            },
        };

        await dbContext.Tasks.AddRangeAsync(items, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
