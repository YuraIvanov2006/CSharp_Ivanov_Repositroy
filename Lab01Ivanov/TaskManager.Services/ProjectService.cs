using TaskManager.DBModels;
using TaskManager.DTOModels.Project;
using TaskManager.DTOModels.Task;
using TaskManager.Repository;

namespace TaskManager.Services
{
    /// <summary>
    /// Converts DBModels → DTOs and applies filtering/sorting.
    /// Registered as Singleton. Receives IProjectRepository via DI.
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repo;

        public ProjectService(IProjectRepository repo) => _repo = repo;

        public Task InitializeAsync() => _repo.InitializeAsync();

        // ── Projects ──────────────────────────────────────────────────────────

        public async Task<List<ProjectListDto>> GetAllProjectsAsync(
            string? search = null, string? sortBy = null)
        {
            var projects = await _repo.GetAllProjectsAsync();
            var result   = new List<ProjectListDto>();

            foreach (var p in projects)
            {
                if (!string.IsNullOrWhiteSpace(search) &&
                    !p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) &&
                    !p.Description.Contains(search, StringComparison.OrdinalIgnoreCase))
                    continue;

                var tasks    = await _repo.GetTasksByProjectIdAsync(p.Id);
                double prog  = tasks.Count == 0 ? 0.0
                             : (double)tasks.Count(t => t.IsCompleted) / tasks.Count * 100.0;

                result.Add(new ProjectListDto
                {
                    Id               = p.Id,
                    Name             = p.Name,
                    TypeDisplay      = p.Type.ToString(),
                    Description      = p.Description,
                    Progress         = prog,
                    ProgressFraction = prog / 100.0
                });
            }

            return sortBy switch
            {
                "Name ↑"      => result.OrderBy(p => p.Name).ToList(),
                "Name ↓"      => result.OrderByDescending(p => p.Name).ToList(),
                "Type"        => result.OrderBy(p => p.TypeDisplay).ToList(),
                "Progress ↑"  => result.OrderBy(p => p.Progress).ToList(),
                "Progress ↓"  => result.OrderByDescending(p => p.Progress).ToList(),
                _             => result
            };
        }

        public async Task<ProjectDetailDto?> GetProjectDetailAsync(
            int projectId, string? taskSearch = null, string? taskSortBy = null)
        {
            var p = await _repo.GetProjectByIdAsync(projectId);
            if (p == null) return null;

            var tasks    = await _repo.GetTasksByProjectIdAsync(projectId);
            double prog  = tasks.Count == 0 ? 0.0
                         : (double)tasks.Count(t => t.IsCompleted) / tasks.Count * 100.0;

            IEnumerable<TaskDbModel> filtered = tasks;
            if (!string.IsNullOrWhiteSpace(taskSearch))
                filtered = filtered.Where(t =>
                    t.Name.Contains(taskSearch, StringComparison.OrdinalIgnoreCase));

            IEnumerable<TaskDbModel> sorted = taskSortBy switch
            {
                "Name ↑"       => filtered.OrderBy(t => t.Name),
                "Name ↓"       => filtered.OrderByDescending(t => t.Name),
                "Priority ↑"   => filtered.OrderBy(t => t.Priority),
                "Priority ↓"   => filtered.OrderByDescending(t => t.Priority),
                "Due Date ↑"   => filtered.OrderBy(t => t.DueDate),
                "Due Date ↓"   => filtered.OrderByDescending(t => t.DueDate),
                _              => filtered
            };

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
                Tasks            = sorted.Select(MapToTaskListDto).ToList()
            };
        }

        public async Task AddProjectAsync(CreateProjectDto dto)
        {
            var model = new ProjectDbModel(dto.Name, dto.Description, dto.Type);
            await _repo.AddProjectAsync(model);
        }

        public async Task UpdateProjectAsync(UpdateProjectDto dto)
        {
            var model = await _repo.GetProjectByIdAsync(dto.Id);
            if (model == null) return;
            model.Name        = dto.Name;
            model.Description = dto.Description;
            model.Type        = dto.Type;
            await _repo.UpdateProjectAsync(model);
        }

        public Task DeleteProjectAsync(int id) => _repo.DeleteProjectAsync(id);

        // ── Tasks ─────────────────────────────────────────────────────────────

        public async Task<TaskDetailDto?> GetTaskDetailAsync(int taskId)
        {
            var t = await _repo.GetTaskByIdAsync(taskId);
            if (t == null) return null;
            bool overdue = !t.IsCompleted && t.DueDate < DateTime.Today;
            return new TaskDetailDto
            {
                Id              = t.Id,
                ProjectId       = t.ProjectId,
                Name            = t.Name,
                Description     = t.Description,
                PriorityDisplay = t.Priority.ToString(),
                DueDateDisplay  = t.DueDate.ToString("yyyy-MM-dd"),
                IsCompleted     = t.IsCompleted,
                IsOverdue       = overdue
            };
        }

        public async Task AddTaskAsync(CreateTaskDto dto)
        {
            var model = new TaskDbModel(dto.ProjectId, dto.Name, dto.Description,
                                        dto.Priority, dto.DueDate, dto.IsCompleted);
            await _repo.AddTaskAsync(model);
        }

        public async Task UpdateTaskAsync(UpdateTaskDto dto)
        {
            var model = await _repo.GetTaskByIdAsync(dto.Id);
            if (model == null) return;
            model.Name        = dto.Name;
            model.Description = dto.Description;
            model.Priority    = dto.Priority;
            model.DueDate     = dto.DueDate;
            model.IsCompleted = dto.IsCompleted;
            await _repo.UpdateTaskAsync(model);
        }

        public Task DeleteTaskAsync(int id) => _repo.DeleteTaskAsync(id);

        // ── Edit form helpers ─────────────────────────────────────────────────

        public async Task<UpdateProjectDto?> GetProjectForEditAsync(int id)
        {
            var p = await _repo.GetProjectByIdAsync(id);
            if (p == null) return null;
            return new UpdateProjectDto { Id = p.Id, Name = p.Name, Description = p.Description, Type = p.Type };
        }

        public async Task<UpdateTaskDto?> GetTaskForEditAsync(int taskId)
        {
            var t = await _repo.GetTaskByIdAsync(taskId);
            if (t == null) return null;
            return new UpdateTaskDto { Id = t.Id, ProjectId = t.ProjectId, Name = t.Name,
                Description = t.Description, Priority = t.Priority, DueDate = t.DueDate, IsCompleted = t.IsCompleted };
        }

        // ── Private helpers ───────────────────────────────────────────────────

        private static TaskListDto MapToTaskListDto(TaskDbModel t)
        {
            bool overdue = !t.IsCompleted && t.DueDate < DateTime.Today;
            return new TaskListDto { Id = t.Id, ProjectId = t.ProjectId, Name = t.Name,
                PriorityDisplay = t.Priority.ToString(), DueDate = t.DueDate,
                IsCompleted = t.IsCompleted, IsOverdue = overdue };
        }
    }
}
