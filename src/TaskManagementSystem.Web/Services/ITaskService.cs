using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Services;

public interface ITaskService
{
    Task<PagedResult<TaskItem>> GetPagedAsync(TaskQueryOptions options, CancellationToken cancellationToken = default);

    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TaskItem> CreateAsync(TaskItem item, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(TaskItem item, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
