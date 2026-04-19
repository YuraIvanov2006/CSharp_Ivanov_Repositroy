using TaskManager.DBModels.Enums;

namespace TaskManager.DBModels
{
    /// <summary>
    /// Storage model for the Project entity.
    /// Responsibility: storing raw project data only.
    /// Rules:
    ///   - No computed fields (e.g. Progress is NOT here).
    ///   - No collection of child Task objects.
    ///   - Id has no setter — it cannot be changed after creation.
    /// </summary>
    public class ProjectDbModel
    {
        /// <summary>Unique identifier. Set once at creation, never changed.</summary>
        public int Id { get; }

        /// <summary>Name/title of the project.</summary>
        public string Name { get; set; }

        /// <summary>Short description of the project's goal.</summary>
        public string Description { get; set; }

        /// <summary>Category/type of the project.</summary>
        public ProjectType Type { get; set; }

        /// <summary>
        /// Creates a new ProjectDbModel with all required fields.
        /// </summary>
        public ProjectDbModel(int id, string name, string description, ProjectType type)
        {
            Id          = id;
            Name        = name;
            Description = description;
            Type        = type;
        }
    }
}
