using SQLite;
using TaskManager.DBModels.Enums;

namespace TaskManager.DBModels
{
    /// <summary>
    /// Storage model for a Project entity.
    /// Mapped to the "Projects" SQLite table via sqlite-net-pcl attributes.
    /// Rules: no computed fields, no task collections.
    /// </summary>
    [Table("Projects")]
    public class ProjectDbModel
    {
        /// <summary>Auto-incremented PK set by SQLite on insert.</summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name        { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ProjectType Type   { get; set; }

        /// <summary>Parameterless constructor required by SQLite ORM.</summary>
        public ProjectDbModel() { }

        public ProjectDbModel(string name, string description, ProjectType type)
        {
            Name        = name;
            Description = description;
            Type        = type;
        }
    }
}
