namespace TaskManager.DTOModels.Project
{
    /// <summary>
    /// DTO for displaying a project in a list.
    /// Contains only the fields needed for the list view — no child collections.
    /// </summary>
    public class ProjectListDto
    {
        public int    Id              { get; set; }
        public string Name            { get; set; } = string.Empty;
        public string TypeDisplay     { get; set; } = string.Empty;
        public string Description     { get; set; } = string.Empty;
        public double Progress        { get; set; }  // 0–100
        public double ProgressFraction { get; set; } // 0.0–1.0 for ProgressBar
    }
}
