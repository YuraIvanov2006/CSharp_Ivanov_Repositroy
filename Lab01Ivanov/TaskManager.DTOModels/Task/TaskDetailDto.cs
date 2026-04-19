namespace TaskManager.DTOModels.Task
{
    /// <summary>
    /// DTO for the task detail page.
    /// Contains all fields visible to the user, including computed display strings.
    /// </summary>
    public class TaskDetailDto
    {
        public int    Id              { get; set; }
        public int    ProjectId       { get; set; }
        public string Name            { get; set; } = string.Empty;
        public string Description     { get; set; } = string.Empty;
        public string PriorityDisplay { get; set; } = string.Empty;
        public string DueDateDisplay  { get; set; } = string.Empty;
        public bool   IsCompleted     { get; set; }
        public bool   IsOverdue       { get; set; }

        /// <summary>Human-readable status string for the UI banner.</summary>
        public string StatusText =>
            IsCompleted ? "✅  Completed" :
            IsOverdue   ? "🔴  Overdue"  :
                          "🔵  In Progress";
    }
}
