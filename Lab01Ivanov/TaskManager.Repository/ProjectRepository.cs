using TaskManager.DBModels;

namespace TaskManager.Repository
{
    /// <summary>
    /// Concrete async repository — delegates to DatabaseContext (SQLite).
    /// Registered as Singleton in IoC: AddSingleton&lt;IProjectRepository, ProjectRepository&gt;
    /// </summary>
    public class ProjectRepository : IProjectRepository
    {
        private readonly DatabaseContext _db;

        public ProjectRepository(DatabaseContext db) => _db = db;

        public Task InitializeAsync() => _db.InitializeAsync();

        // ── Projects ──────────────────────────────────────────────────────────

        public async Task<IReadOnlyList<ProjectDbModel>> GetAllProjectsAsync()
            => (await _db.GetAllProjectsAsync()).AsReadOnly();

        public Task<ProjectDbModel?> GetProjectByIdAsync(int id)
            => _db.GetProjectByIdAsync(id);

        public Task<int> AddProjectAsync(ProjectDbModel project)
            => _db.InsertProjectAsync(project);

        public async Task UpdateProjectAsync(ProjectDbModel project)
            => await _db.UpdateProjectAsync(project);

        public async Task DeleteProjectAsync(int id)
        {
            // Cascade: delete all child tasks first
            await _db.DeleteTasksByProjectIdAsync(id);
            var p = await _db.GetProjectByIdAsync(id);
            if (p != null) await _db.DeleteProjectAsync(p);
        }

        // ── Tasks ─────────────────────────────────────────────────────────────

        public async Task<IReadOnlyList<TaskDbModel>> GetTasksByProjectIdAsync(int projectId)
            => (await _db.GetTasksByProjectIdAsync(projectId)).AsReadOnly();

        public Task<TaskDbModel?> GetTaskByIdAsync(int taskId)
            => _db.GetTaskByIdAsync(taskId);

        public Task<int> AddTaskAsync(TaskDbModel task)
            => _db.InsertTaskAsync(task);

        public async Task UpdateTaskAsync(TaskDbModel task)
            => await _db.UpdateTaskAsync(task);

        public async Task DeleteTaskAsync(int id)
        {
            var t = await _db.GetTaskByIdAsync(id);
            if (t != null) await _db.DeleteTaskAsync(t);
        }
    }
}
