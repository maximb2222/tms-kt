using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Web.Models;
using TaskState = TaskManagementSystem.Web.Models.TaskStatus;

namespace TaskManagementSystem.Web.ViewModels;

public class TaskItemInputModel
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? Assignee { get; set; }

    [Required]
    public TaskState Status { get; set; } = TaskState.New;
}
