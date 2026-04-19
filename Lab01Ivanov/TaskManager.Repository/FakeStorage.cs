using TaskManager.DBModels;
using TaskManager.DBModels.Enums;

namespace TaskManager.Repository
{
    /// <summary>
    /// Internal in-memory data source (fake database).
    /// ONLY accessible within the TaskManager.Repository assembly —
    /// all external layers must go through the repository interfaces.
    /// </summary>
    internal static class FakeStorage
    {
        private static readonly List<ProjectDbModel> _projects;
        private static readonly List<TaskDbModel>    _tasks;

        static FakeStorage()
        {
            _projects = SeedProjects();
            _tasks    = SeedTasks();
        }

        internal static IReadOnlyList<ProjectDbModel> GetAllProjects()
            => _projects.AsReadOnly();

        internal static ProjectDbModel? GetProjectById(int id)
            => _projects.FirstOrDefault(p => p.Id == id);

        internal static IReadOnlyList<TaskDbModel> GetTasksByProjectId(int projectId)
            => _tasks.Where(t => t.ProjectId == projectId).ToList().AsReadOnly();

        internal static TaskDbModel? GetTaskById(int projectId, int taskId)
            => _tasks.FirstOrDefault(t => t.ProjectId == projectId && t.Id == taskId);

        private static List<ProjectDbModel> SeedProjects() => new()
        {
            new ProjectDbModel(1, "Corporate Website Redesign",
                "Redesign and rebuild the company website with modern UI and improved performance.",
                ProjectType.Work),
            new ProjectDbModel(2, "C# University Course",
                "Complete all laboratory works for the C# and OOP university course.",
                ProjectType.Educational),
            new ProjectDbModel(3, "Home Renovation Planning",
                "Plan and organise the apartment renovation — budget, timeline, and contractors.",
                ProjectType.Personal)
        };

        private static List<TaskDbModel> SeedTasks() => new()
        {
            // ── Project 1: Corporate Website Redesign (10 tasks) ──
            new TaskDbModel(1,  1, "Audit existing website",
                "Analyse current site structure, find performance bottlenecks and outdated content.",
                TaskPriority.High, new DateTime(2025, 12, 10), true),
            new TaskDbModel(2,  1, "Define target audience and goals",
                "Conduct stakeholder interviews and document user personas and business goals.",
                TaskPriority.High, new DateTime(2025, 12, 20), true),
            new TaskDbModel(3,  1, "Create UI/UX wireframes",
                "Design low-fidelity wireframes for homepage, about, services, and contact pages.",
                TaskPriority.Medium, new DateTime(2026, 1, 15), true),
            new TaskDbModel(4,  1, "Design visual style guide",
                "Define brand colour palette, typography, spacing rules, and component library.",
                TaskPriority.Medium, new DateTime(2026, 2, 1), true),
            new TaskDbModel(5,  1, "Develop homepage layout",
                "Implement homepage in HTML/CSS based on approved wireframes and style guide.",
                TaskPriority.High, new DateTime(2026, 2, 20), false),
            new TaskDbModel(6,  1, "Implement responsive design",
                "Ensure all pages render correctly on mobile, tablet, and desktop.",
                TaskPriority.High, new DateTime(2026, 3, 5), false),
            new TaskDbModel(7,  1, "Set up backend REST API",
                "Create API endpoints for contact form, newsletter signup, and CMS integration.",
                TaskPriority.Critical, new DateTime(2026, 3, 20), false),
            new TaskDbModel(8,  1, "Integrate headless CMS",
                "Connect the website to a CMS so the marketing team can manage content.",
                TaskPriority.Critical, new DateTime(2026, 4, 5), false),
            new TaskDbModel(9,  1, "Conduct QA and user testing",
                "Run end-to-end tests and collect feedback from a group of internal users.",
                TaskPriority.Medium, new DateTime(2026, 4, 30), false),
            new TaskDbModel(10, 1, "Deploy to production",
                "Deploy the finished website, configure DNS and SSL.",
                TaskPriority.Critical, new DateTime(2026, 5, 15), false),

            // ── Project 2: C# University Course (2 tasks) ──
            new TaskDbModel(11, 2, "Complete Laboratory Work 1",
                "Implement class models, fake storage, repository, and console app for the Task Manager.",
                TaskPriority.High, new DateTime(2026, 4, 25), false),
            new TaskDbModel(12, 2, "Complete Laboratory Work 2",
                "Add LINQ queries, filtering, and sorting to the Task Manager application.",
                TaskPriority.Medium, new DateTime(2026, 5, 10), false)
        };
    }
}
