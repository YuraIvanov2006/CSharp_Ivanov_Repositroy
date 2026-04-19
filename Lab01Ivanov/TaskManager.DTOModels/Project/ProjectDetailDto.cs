using TaskManager.DTOModels.Task;

namespace TaskManager.DTOModels.Project
{
    /// <summary>
    /// DTO for the project detail page.
    /// Contains all visible project fields plus the list of child task DTOs.
    /// </summary>
    public class ProjectDetailDto
    {
        public int    Id              { get; set; }
        public string Name            { get; set; } = string.Empty;
        public string TypeDisplay     { get; set; } = string.Empty;
        public string Description     { get; set; } = string.Empty;
        public double Progress        { get; set; }
        public double ProgressFraction { get; set; }
        public int    TotalTasks      { get; set; }
        public int    CompletedTasks  { get; set; }
        public int    OverdueTasks    { get; set; }

        /// <summary>Tasks to display in the detail list.</summary>
        public List<TaskListDto> Tasks { get; set; } = new();
    }
}
