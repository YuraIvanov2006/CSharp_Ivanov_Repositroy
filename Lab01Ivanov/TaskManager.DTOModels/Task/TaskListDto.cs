namespace TaskManager.DTOModels.Task
{
    /// <summary>
    /// DTO for displaying a task in a list (inside the project detail page).
    /// Contains only the fields needed for the compact list row.
    /// </summary>
    public class TaskListDto
    {
        public int      Id              { get; set; }
        public int      ProjectId       { get; set; }
        public string   Name            { get; set; } = string.Empty;
        public string   PriorityDisplay { get; set; } = string.Empty;
        public DateTime DueDate         { get; set; }
        public bool     IsCompleted     { get; set; }
        public bool     IsOverdue       { get; set; }

        /// <summary>Emoji status icon for quick visual scan.</summary>
        public string StatusIcon =>
            IsCompleted ? "✅" : IsOverdue ? "🔴" : "🔵";
    }
}
