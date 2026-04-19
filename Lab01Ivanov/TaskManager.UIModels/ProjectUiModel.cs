using TaskManager.DBModels;
using TaskManager.DBModels.Enums;

namespace TaskManager.UIModels
{
    /// <summary>
    /// UI/display model for the Project entity.
    /// Responsibility: wrapping ProjectDbModel data, holding the associated tasks collection,
    /// and adding computed properties + display methods.
    /// </summary>
    public class ProjectUiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectType Type { get; set; }

        /// <summary>
        /// Collection of tasks belonging to this project.
        /// Populated on-demand by TaskManagerRepository (lazy loading pattern).
        /// </summary>
        public List<TaskUiModel> Tasks { get; set; } = new List<TaskUiModel>();

        /// <summary>
        /// Tracks whether tasks have been loaded for this project yet.
        /// Used by the console app to avoid redundant repository calls.
        /// </summary>
        public bool TasksLoaded { get; set; } = false;

        /// <summary>
        /// Computed: percentage of completed tasks out of total tasks (0–100).
        /// Returns 0.0 if the project has no tasks.
        /// </summary>
        public double Progress =>
            Tasks.Count == 0
                ? 0.0
                : (double)Tasks.Count(t => t.IsCompleted) / Tasks.Count * 100.0;

        /// <summary>
        /// Creates a ProjectUiModel by copying data from a storage model (ProjectDbModel).
        /// Tasks list starts empty and is filled later via the repository.
        /// </summary>
        public ProjectUiModel(ProjectDbModel dbModel)
        {
            Id          = dbModel.Id;
            Name        = dbModel.Name;
            Description = dbModel.Description;
            Type        = dbModel.Type;
        }

        /// <summary>
        /// Displays a short one-line summary (used in list views).
        /// </summary>
        public void DisplaySummary(int index)
        {
            Console.WriteLine($"  {index,2}. [{Type,-11}] {Name}");
        }

        /// <summary>
        /// Displays full details of this project including task progress (if tasks are loaded).
        /// </summary>
        public void DisplayDetails()
        {
            Console.WriteLine($"  Project #{Id}: {Name}");
            Console.WriteLine($"  Type        : {Type}");
            Console.WriteLine($"  Description : {Description}");

            if (TasksLoaded)
            {
                int done = Tasks.Count(t => t.IsCompleted);
                int overdue = Tasks.Count(t => t.IsOverdue);
                Console.WriteLine($"  Tasks       : {Tasks.Count} total  |  {done} completed  |  {overdue} overdue");
                Console.WriteLine($"  Progress    : {Progress:F1}%");
            }
            else
            {
                Console.WriteLine($"  Tasks       : not loaded yet");
            }
        }
    }
}
