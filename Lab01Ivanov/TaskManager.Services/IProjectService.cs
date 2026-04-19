using TaskManager.DTOModels.Project;
using TaskManager.DTOModels.Task;

namespace TaskManager.Services
{
    /// <summary>
    /// Full async service contract for the Task Manager app.
    /// UI depends only on this interface — never on repositories or DBModels.
    /// </summary>
    public interface IProjectService
    {
        Task InitializeAsync();

        // ── Projects ──────────────────────────────────────────────────────────
        Task<List<ProjectListDto>> GetAllProjectsAsync(string? search = null, string? sortBy = null);
        Task<ProjectDetailDto?> GetProjectDetailAsync(int projectId, string? taskSearch = null, string? taskSortBy = null);
        Task AddProjectAsync(CreateProjectDto dto);
        Task UpdateProjectAsync(UpdateProjectDto dto);
        Task DeleteProjectAsync(int id);

        // ── Tasks ─────────────────────────────────────────────────────────────
        Task<TaskDetailDto?> GetTaskDetailAsync(int taskId);
        Task AddTaskAsync(CreateTaskDto dto);
        Task UpdateTaskAsync(UpdateTaskDto dto);
        Task DeleteTaskAsync(int id);

        // ── For edit forms ────────────────────────────────────────────────────
        Task<UpdateProjectDto?> GetProjectForEditAsync(int id);
        Task<UpdateTaskDto?> GetTaskForEditAsync(int taskId);
    }
}
