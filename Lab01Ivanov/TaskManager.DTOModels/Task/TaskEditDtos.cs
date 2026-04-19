using TaskManager.DBModels.Enums;

namespace TaskManager.DTOModels.Task
{
    /// <summary>DTO for creating a new task.</summary>
    public class CreateTaskDto
    {
        public int ProjectId      { get; set; }
        public string Name        { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; }
        public DateTime DueDate   { get; set; } = DateTime.Today.AddDays(7);
        public bool IsCompleted   { get; set; }
    }

    /// <summary>DTO for updating an existing task (includes Id).</summary>
    public class UpdateTaskDto
    {
        public int Id             { get; set; }
        public int ProjectId      { get; set; }
        public string Name        { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; }
        public DateTime DueDate   { get; set; }
        public bool IsCompleted   { get; set; }
    }
}
