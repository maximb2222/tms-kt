using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.Data;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Services;

public sealed class TaskService : ITaskService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<TaskService> _logger;

    public TaskService(AppDbContext dbContext, ILogger<TaskService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PagedResult<TaskItem>> GetPagedAsync(
        TaskQueryOptions options,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = Math.Max(1, options.PageNumber);
        var pageSize = Math.Clamp(options.PageSize, 1, 50);
        var searchTerm = options.SearchTerm?.Trim();

        IQueryable<TaskItem> query = _dbContext.Tasks.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var pattern = $"%{searchTerm}%";
            query = query.Where(item =>
                EF.Functions.Like(item.Name, pattern) ||
                (item.Description != null && EF.Functions.Like(item.Description, pattern)) ||
                (item.Assignee != null && EF.Functions.Like(item.Assignee, pattern)));
        }

        if (options.Status is not null)
        {
            query = query.Where(item => item.Status == options.Status);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(item => item.UpdatedAt)
            .ThenBy(item => item.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        _logger.LogInformation(
            "Loaded tasks page {PageNumber}/{PageSize}. Found {Count} records.",
            pageNumber,
            pageSize,
            totalCount);

        return new PagedResult<TaskItem>(items, totalCount, pageNumber, pageSize);
    }

    public Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Tasks.AsNoTracking().SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
    }

    public async Task<TaskItem> CreateAsync(TaskItem item, CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;
        item.Id = item.Id == Guid.Empty ? Guid.NewGuid() : item.Id;
        item.Name = item.Name.Trim();
        item.Description = NormalizeOptionalText(item.Description);
        item.Assignee = NormalizeOptionalText(item.Assignee);
        item.CreatedAt = utcNow;
        item.UpdatedAt = utcNow;

        _dbContext.Tasks.Add(item);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Task {TaskId} created.", item.Id);
        return item;
    }

    public async Task<bool> UpdateAsync(TaskItem item, CancellationToken cancellationToken = default)
    {
        var current = await _dbContext.Tasks.SingleOrDefaultAsync(task => task.Id == item.Id, cancellationToken);
        if (current is null)
        {
            _logger.LogWarning("Update skipped. Task {TaskId} was not found.", item.Id);
            return false;
        }

        current.Name = item.Name.Trim();
        current.Description = NormalizeOptionalText(item.Description);
        current.Assignee = NormalizeOptionalText(item.Assignee);
        current.Status = item.Status;
        current.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Task {TaskId} updated.", current.Id);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var current = await _dbContext.Tasks.SingleOrDefaultAsync(task => task.Id == id, cancellationToken);
        if (current is null)
        {
            _logger.LogWarning("Delete skipped. Task {TaskId} was not found.", id);
            return false;
        }

        _dbContext.Tasks.Remove(current);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Task {TaskId} deleted.", id);
        return true;
    }

    private static string? NormalizeOptionalText(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }
}
