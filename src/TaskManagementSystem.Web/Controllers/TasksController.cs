using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.Models;
using TaskManagementSystem.Web.Services;
using TaskManagementSystem.Web.ViewModels;
using TaskState = TaskManagementSystem.Web.Models.TaskStatus;

namespace TaskManagementSystem.Web.Controllers;

public sealed class TasksController : Controller
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? searchTerm, TaskState? status, int page = 1, CancellationToken cancellationToken = default)
    {
        var pagedTasks = await _taskService.GetPagedAsync(
            new TaskQueryOptions
            {
                SearchTerm = searchTerm,
                Status = status,
                PageNumber = page,
                PageSize = 10,
            },
            cancellationToken);

        return View(new TaskListViewModel
        {
            PagedTasks = pagedTasks,
            SearchTerm = searchTerm,
            Status = status,
            Statuses = Enum.GetValues<TaskState>(),
        });
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new TaskItemInputModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TaskItemInputModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _taskService.CreateAsync(
            new TaskItem
            {
                Name = model.Name,
                Description = model.Description,
                Assignee = model.Assignee,
                Status = model.Status,
            },
            cancellationToken);

        TempData["Success"] = "Task created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _taskService.GetByIdAsync(id, cancellationToken);
        if (item is null)
        {
            return NotFound();
        }

        return View(item);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _taskService.GetByIdAsync(id, cancellationToken);
        if (item is null)
        {
            return NotFound();
        }

        return View(new TaskItemEditModel
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Assignee = item.Assignee,
            Status = item.Status,
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TaskItemEditModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var updated = await _taskService.UpdateAsync(
            new TaskItem
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Assignee = model.Assignee,
                Status = model.Status,
            },
            cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        TempData["Success"] = "Task updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _taskService.GetByIdAsync(id, cancellationToken);
        if (item is null)
        {
            return NotFound();
        }

        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken = default)
    {
        var deleted = await _taskService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        TempData["Success"] = "Task deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
