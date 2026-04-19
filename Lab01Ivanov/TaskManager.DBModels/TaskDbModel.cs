using TaskManager.DBModels.Enums;

namespace TaskManager.DBModels
{
    /// <summary>
    /// Storage model for the Task entity.
    /// Responsibility: storing raw task data only.
    /// Rules:
    ///   - No computed fields (e.g. IsOverdue is NOT here).
    ///   - No reference/navigation to the parent Project object — only ProjectId is stored.
    ///   - Id has no setter — it cannot be changed after creation.
    /// </summary>
    public class TaskDbModel
    {
        /// <summary>Unique identifier. Set once at creation, never changed.</summary>
        public int Id { get; }

        /// <summary>
        /// Foreign key: the Id of the project this task belongs to.
        /// A task must belong to exactly one project.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>Name/title of the task.</summary>
        public string Name { get; set; }

        /// <summary>Detailed description or technical specification.</summary>
        public string Description { get; set; }

        /// <summary>Priority level of the task.</summary>
        public TaskPriority Priority { get; set; }

        /// <summary>The deadline by which the task must be completed.</summary>
        public DateTime DueDate { get; set; }

        /// <summary>Whether the task has been marked as completed.</summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Creates a new TaskDbModel with all required fields.
        /// </summary>
        public TaskDbModel(int id, int projectId, string name, string description,
            TaskPriority priority, DateTime dueDate, bool isCompleted)
        {
            Id          = id;
            ProjectId   = projectId;
            Name        = name;
            Description = description;
            Priority    = priority;
            DueDate     = dueDate;
            IsCompleted = isCompleted;
        }
    }
}
