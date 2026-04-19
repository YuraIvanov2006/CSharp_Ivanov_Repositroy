using SQLite;
using TaskManager.DBModels.Enums;

namespace TaskManager.DBModels
{
    /// <summary>
    /// Storage model for a Task entity.
    /// Mapped to the "Tasks" SQLite table.
    /// Rules: no computed fields (IsOverdue), only stores ProjectId as FK.
    /// </summary>
    [Table("Tasks")]
    public class TaskDbModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>FK to Projects table — defines ownership.</summary>
        [Indexed]
        public int ProjectId { get; set; }

        public string Name        { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; }
        public DateTime DueDate   { get; set; }
        public bool IsCompleted   { get; set; }

        /// <summary>Parameterless constructor required by SQLite ORM.</summary>
        public TaskDbModel() { }

        public TaskDbModel(int projectId, string name, string description,
            TaskPriority priority, DateTime dueDate, bool isCompleted = false)
        {
            ProjectId   = projectId;
            Name        = name;
            Description = description;
            Priority    = priority;
            DueDate     = dueDate;
            IsCompleted = isCompleted;
        }
    }
}
