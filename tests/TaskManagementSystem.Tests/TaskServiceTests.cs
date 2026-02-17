using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TaskManagementSystem.Web.Data;
using TaskManagementSystem.Web.Models;
using TaskManagementSystem.Web.Services;
using TaskStatus = TaskManagementSystem.Web.Models.TaskStatus;

namespace TaskManagementSystem.Tests;

public sealed class TaskServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldPersistAndNormalizeTask()
    {
        await using var dbContext = CreateDbContext();
        var service = CreateService(dbContext);

        var created = await service.CreateAsync(new TaskItem
        {
            Name = "  Build tests  ",
            Description = "  cover business logic  ",
            Assignee = "  QA  ",
            Status = TaskStatus.InProgress,
        });

        created.Id.Should().NotBe(Guid.Empty);
        created.Name.Should().Be("Build tests");
        created.Description.Should().Be("cover business logic");
        created.Assignee.Should().Be("QA");
        created.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        var stored = await dbContext.Tasks.SingleAsync();
        stored.Name.Should().Be("Build tests");
        stored.Status.Should().Be(TaskStatus.InProgress);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldApplySearchStatusAndPaging()
    {
        await using var dbContext = CreateDbContext();
        dbContext.Tasks.AddRange(
            new TaskItem { Name = "Task alpha", Status = TaskStatus.New, Description = "First task", Assignee = "A" },
            new TaskItem { Name = "Task beta", Status = TaskStatus.InProgress, Description = "Second task", Assignee = "B" },
            new TaskItem { Name = "Task gamma", Status = TaskStatus.InProgress, Description = "Third task", Assignee = "C" });
        await dbContext.SaveChangesAsync();

        var service = CreateService(dbContext);
        var result = await service.GetPagedAsync(new TaskQueryOptions
        {
            SearchTerm = "task",
            Status = TaskStatus.InProgress,
            PageNumber = 1,
            PageSize = 1,
        });

        result.TotalCount.Should().Be(2);
        result.Items.Should().HaveCount(1);
        result.TotalPages.Should().Be(2);
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrueForExistingTask()
    {
        await using var dbContext = CreateDbContext();
        var existing = new TaskItem
        {
            Name = "Initial",
            Description = "Initial description",
            Assignee = "User",
            Status = TaskStatus.New,
        };
        dbContext.Tasks.Add(existing);
        await dbContext.SaveChangesAsync();

        var service = CreateService(dbContext);
        var updated = await service.UpdateAsync(new TaskItem
        {
            Id = existing.Id,
            Name = "Updated name",
            Description = "Updated description",
            Assignee = "Owner",
            Status = TaskStatus.Completed,
        });

        updated.Should().BeTrue();
        var stored = await dbContext.Tasks.SingleAsync();
        stored.Name.Should().Be("Updated name");
        stored.Status.Should().Be(TaskStatus.Completed);
        stored.UpdatedAt.Should().BeAfter(stored.CreatedAt);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalseWhenTaskIsMissing()
    {
        await using var dbContext = CreateDbContext();
        var service = CreateService(dbContext);

        var updated = await service.UpdateAsync(new TaskItem
        {
            Id = Guid.NewGuid(),
            Name = "Not found",
            Status = TaskStatus.New,
        });

        updated.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteExistingTask()
    {
        await using var dbContext = CreateDbContext();
        var existing = new TaskItem { Name = "Delete me", Status = TaskStatus.New };
        dbContext.Tasks.Add(existing);
        await dbContext.SaveChangesAsync();
        var service = CreateService(dbContext);

        var deleted = await service.DeleteAsync(existing.Id);

        deleted.Should().BeTrue();
        (await dbContext.Tasks.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalseWhenTaskIsMissing()
    {
        await using var dbContext = CreateDbContext();
        var service = CreateService(dbContext);

        var deleted = await service.DeleteAsync(Guid.NewGuid());

        deleted.Should().BeFalse();
    }

    [Fact]
    public async Task Scenario_ShouldCreateUpdateListAndDeleteTask()
    {
        await using var dbContext = CreateDbContext();
        var service = CreateService(dbContext);

        var created = await service.CreateAsync(new TaskItem
        {
            Name = "Scenario task",
            Description = "Scenario",
            Status = TaskStatus.New,
            Assignee = "Tester",
        });

        var updated = await service.UpdateAsync(new TaskItem
        {
            Id = created.Id,
            Name = "Scenario task updated",
            Description = "Scenario updated",
            Status = TaskStatus.InProgress,
            Assignee = "Tester",
        });

        var list = await service.GetPagedAsync(new TaskQueryOptions
        {
            SearchTerm = "Scenario",
            Status = TaskStatus.InProgress,
            PageNumber = 1,
            PageSize = 10,
        });

        var deleted = await service.DeleteAsync(created.Id);

        updated.Should().BeTrue();
        list.TotalCount.Should().Be(1);
        list.Items.Single().Name.Should().Be("Scenario task updated");
        deleted.Should().BeTrue();
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"TaskManagementTests-{Guid.NewGuid()}")
            .Options;

        return new AppDbContext(options);
    }

    private static TaskService CreateService(AppDbContext dbContext)
    {
        return new TaskService(dbContext, NullLogger<TaskService>.Instance);
    }
}
