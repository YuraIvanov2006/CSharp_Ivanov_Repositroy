using TaskManager.DBModels;
using TaskManager.UIModels;

namespace TaskManager.Services
{
    /// <summary>
    /// Repository class — the single entry point for accessing storage data.
    ///
    /// Responsibilities:
    ///   1. Retrieve ProjectDbModel / TaskDbModel objects from FakeStorage.
    ///   2. Convert storage models (DBModels) into UI models (UIModels).
    ///   3. Hide FakeStorage from all other projects — external code only talks to this class.
    ///
    /// In future labs this class can be swapped to read from a real database
    /// without changing any code in UIModels or the console app.
    /// </summary>
    /// <summary>
    /// Concrete implementation of ITaskManagerRepository.
    /// Registered in MauiProgram.cs via DI: AddSingleton&lt;ITaskManagerRepository, TaskManagerRepository&gt;
    /// </summary>
    public class TaskManagerRepository : ITaskManagerRepository
    {
        /// <inheritdoc/>
        public List<ProjectUiModel> GetAllProjects()
        {
            IReadOnlyList<ProjectDbModel> dbProjects = FakeStorage.GetAllProjects();
            return dbProjects
                .Select(db => new ProjectUiModel(db))
                .ToList();
        }

        /// <inheritdoc/>
        public void LoadTasksForProject(ProjectUiModel project)
        {
            IReadOnlyList<TaskDbModel> dbTasks = FakeStorage.GetTasksByProjectId(project.Id);
            project.Tasks      = dbTasks.Select(db => new TaskUiModel(db)).ToList();
            project.TasksLoaded = true;
        }

        /// <inheritdoc/>
        public ProjectUiModel? GetProjectById(int id)
        {
            var dbProject = FakeStorage.GetAllProjects().FirstOrDefault(p => p.Id == id);
            if (dbProject == null) return null;

            var project = new ProjectUiModel(dbProject);
            LoadTasksForProject(project);
            return project;
        }

        /// <inheritdoc/>
        public TaskUiModel? GetTaskById(int projectId, int taskId)
        {
            var dbTask = FakeStorage.GetTasksByProjectId(projectId)
                                    .FirstOrDefault(t => t.Id == taskId);
            return dbTask != null ? new TaskUiModel(dbTask) : null;
        }
    }
}
