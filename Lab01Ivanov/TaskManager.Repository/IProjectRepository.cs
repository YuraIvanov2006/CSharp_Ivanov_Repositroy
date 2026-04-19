using TaskManager.DBModels;

namespace TaskManager.Repository
{
    /// <summary>
    /// Async contract for project/task data access.
    /// Services depend on this interface (Dependency Inversion Principle).
    /// </summary>
    public interface IProjectRepository
    {
        Task InitializeAsync();

        // Projects
        Task<IReadOnlyList<ProjectDbModel>> GetAllProjectsAsync();
        Task<ProjectDbModel?> GetProjectByIdAsync(int id);
        Task<int> AddProjectAsync(ProjectDbModel project);
        Task UpdateProjectAsync(ProjectDbModel project);
        Task DeleteProjectAsync(int id);

        // Tasks
        Task<IReadOnlyList<TaskDbModel>> GetTasksByProjectIdAsync(int projectId);
        Task<TaskDbModel?> GetTaskByIdAsync(int taskId);
        Task<int> AddTaskAsync(TaskDbModel task);
        Task UpdateTaskAsync(TaskDbModel task);
        Task DeleteTaskAsync(int id);
    }
}
