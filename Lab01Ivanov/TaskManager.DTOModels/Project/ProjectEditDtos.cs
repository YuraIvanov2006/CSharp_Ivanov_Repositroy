using TaskManager.DBModels.Enums;

namespace TaskManager.DTOModels.Project
{
    /// <summary>DTO for creating a new project (no Id — assigned by storage).</summary>
    public class CreateProjectDto
    {
        public string Name        { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ProjectType Type   { get; set; }
    }

    /// <summary>DTO for updating an existing project (includes Id).</summary>
    public class UpdateProjectDto
    {
        public int    Id          { get; set; }
        public string Name        { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ProjectType Type   { get; set; }
    }
}
