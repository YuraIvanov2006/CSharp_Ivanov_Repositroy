using TaskManager.DBModels;

namespace TaskManager.Repository
{
    /// <summary>
    /// Concrete repository — retrieves raw DBModels from FakeStorage.
    /// Registered in IoC container as: AddSingleton&lt;IProjectRepository, ProjectRepository&gt;
    /// </summary>
    public class ProjectRepository : IProjectRepository
    {
        public IReadOnlyList<ProjectDbModel> GetAllProjects()
            => FakeStorage.GetAllProjects();

        public ProjectDbModel? GetProjectById(int id)
            => FakeStorage.GetProjectById(id);

        public IReadOnlyList<TaskDbModel> GetTasksByProjectId(int projectId)
            => FakeStorage.GetTasksByProjectId(projectId);

        public TaskDbModel? GetTaskById(int projectId, int taskId)
            => FakeStorage.GetTaskById(projectId, taskId);
    }
}
