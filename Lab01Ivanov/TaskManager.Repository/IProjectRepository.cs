using TaskManager.DBModels;

namespace TaskManager.Repository
{
    /// <summary>
    /// Contract for project data access.
    /// Dependency Inversion: Services depend on this interface, not the concrete class.
    /// </summary>
    public interface IProjectRepository
    {
        IReadOnlyList<ProjectDbModel> GetAllProjects();
        ProjectDbModel? GetProjectById(int id);
        IReadOnlyList<TaskDbModel> GetTasksByProjectId(int projectId);
        TaskDbModel? GetTaskById(int projectId, int taskId);
    }
}
