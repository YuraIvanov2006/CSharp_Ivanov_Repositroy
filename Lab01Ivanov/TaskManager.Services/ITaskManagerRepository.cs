using TaskManager.UIModels;

namespace TaskManager.Services
{
    /// <summary>
    /// Contract for all storage interactions.
    /// Applying Dependency Inversion Principle: UI layers depend on this
    /// abstraction, not on the concrete TaskManagerRepository class.
    /// Swap FakeStorage for a real DB later without touching UI code.
    /// </summary>
    public interface ITaskManagerRepository
    {
        /// <summary>Returns all projects (tasks not yet loaded).</summary>
        List<ProjectUiModel> GetAllProjects();

        /// <summary>Loads tasks into the given project. Sets TasksLoaded = true.</summary>
        void LoadTasksForProject(ProjectUiModel project);

        /// <summary>Returns a project with all its tasks loaded, or null.</summary>
        ProjectUiModel? GetProjectById(int id);

        /// <summary>Returns a single task by projectId + taskId, or null.</summary>
        TaskUiModel? GetTaskById(int projectId, int taskId);
    }
}
