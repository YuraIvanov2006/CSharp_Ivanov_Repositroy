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
    public class TaskManagerRepository
    {
        /// <summary>
        /// Returns all projects as UI models with an empty Tasks list.
        /// Tasks are NOT loaded here — call <see cref="LoadTasksForProject"/> separately.
        /// </summary>
        public List<ProjectUiModel> GetAllProjects()
        {
            IReadOnlyList<ProjectDbModel> dbProjects = FakeStorage.GetAllProjects();

            // Convert each storage model to a UI model
            return dbProjects
                .Select(db => new ProjectUiModel(db))
                .ToList();
        }

        /// <summary>
        /// Loads all tasks that belong to the given project and attaches them to it.
        /// Sets <see cref="ProjectUiModel.TasksLoaded"/> to true after loading.
        /// Safe to call multiple times — tasks are reloaded each time.
        /// </summary>
        public void LoadTasksForProject(ProjectUiModel project)
        {
            IReadOnlyList<TaskDbModel> dbTasks = FakeStorage.GetTasksByProjectId(project.Id);

            project.Tasks = dbTasks
                .Select(db => new TaskUiModel(db))
                .ToList();

            project.TasksLoaded = true;
        }
    }
}
