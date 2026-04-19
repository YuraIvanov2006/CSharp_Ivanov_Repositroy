using TaskManager.DBModels;
using TaskManager.DTOModels.Project;
using TaskManager.DTOModels.Task;
using TaskManager.Repository;

namespace TaskManager.Services
{
    /// <summary>
    /// Service layer — receives IProjectRepository via DI, converts DBModels into DTOs,
    /// and returns them to the UI layer.
    /// Registered in IoC: AddSingleton&lt;IProjectService, ProjectService&gt;
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;

        /// <summary>
        /// IProjectRepository is injected by the IoC container — the service
        /// never creates it directly (Dependency Inversion Principle).
        /// </summary>
        public ProjectService(IProjectRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public List<ProjectListDto> GetAllProjects()
        {
            var projects = _repository.GetAllProjects();

            return projects.Select(p =>
            {
                var tasks    = _repository.GetTasksByProjectId(p.Id);
                double prog  = tasks.Count == 0 ? 0.0
                             : (double)tasks.Count(t => t.IsCompleted) / tasks.Count * 100.0;

                return new ProjectListDto
                {
                    Id               = p.Id,
                    Name             = p.Name,
                    TypeDisplay      = p.Type.ToString(),
                    Description      = p.Description,
                    Progress         = prog,
                    ProgressFraction = prog / 100.0
                };
            }).ToList();
        }

        /// <inheritdoc/>
        public ProjectDetailDto? GetProjectDetail(int projectId)
        {
            ProjectDbModel? p = _repository.GetProjectById(projectId);
            if (p == null) return null;

            var tasks    = _repository.GetTasksByProjectId(projectId);
            double prog  = tasks.Count == 0 ? 0.0
                         : (double)tasks.Count(t => t.IsCompleted) / tasks.Count * 100.0;

            return new ProjectDetailDto
            {
                Id               = p.Id,
                Name             = p.Name,
                TypeDisplay      = p.Type.ToString(),
                Description      = p.Description,
                Progress         = prog,
                ProgressFraction = prog / 100.0,
                TotalTasks       = tasks.Count,
                CompletedTasks   = tasks.Count(t => t.IsCompleted),
                OverdueTasks     = tasks.Count(t => !t.IsCompleted && t.DueDate < DateTime.Today),
                Tasks            = tasks.Select(MapToTaskListDto).ToList()
            };
        }

        /// <inheritdoc/>
        public TaskDetailDto? GetTaskDetail(int projectId, int taskId)
        {
            TaskDbModel? t = _repository.GetTaskById(projectId, taskId);
            if (t == null) return null;

            bool isOverdue = !t.IsCompleted && t.DueDate < DateTime.Today;
            return new TaskDetailDto
            {
                Id              = t.Id,
                ProjectId       = t.ProjectId,
                Name            = t.Name,
                Description     = t.Description,
                PriorityDisplay = t.Priority.ToString(),
                DueDateDisplay  = t.DueDate.ToString("yyyy-MM-dd"),
                IsCompleted     = t.IsCompleted,
                IsOverdue       = isOverdue
            };
        }

        // ── Private helpers ───────────────────────────────────────────────────

        private static TaskListDto MapToTaskListDto(TaskDbModel t)
        {
            bool isOverdue = !t.IsCompleted && t.DueDate < DateTime.Today;
            return new TaskListDto
            {
                Id              = t.Id,
                ProjectId       = t.ProjectId,
                Name            = t.Name,
                PriorityDisplay = t.Priority.ToString(),
                DueDate         = t.DueDate,
                IsCompleted     = t.IsCompleted,
                IsOverdue       = isOverdue
            };
        }
    }
}
