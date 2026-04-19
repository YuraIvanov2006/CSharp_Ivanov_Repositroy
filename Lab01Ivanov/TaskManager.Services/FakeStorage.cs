using TaskManager.DBModels;
using TaskManager.DBModels.Enums;

namespace TaskManager.Services
{
    /// <summary>
    /// Artificial in-memory data storage with seed data.
    ///
    /// Access rules:
    ///   - This class is INTERNAL — only TaskManagerRepository (in this assembly) may use it.
    ///   - External projects cannot access FakeStorage directly.
    ///
    /// Data initialised in the static constructor once on first use:
    ///   - 3 projects (Work, Educational, Personal)
    ///   - 12 tasks: 10 for Project 1, 2 for Project 2, 0 for Project 3
    /// </summary>
    internal static class FakeStorage
    {
        private static readonly List<ProjectDbModel> _projects;
        private static readonly List<TaskDbModel> _tasks;

        /// <summary>
        /// Static constructor — runs once when the class is first accessed.
        /// Populates the storage with realistic seed data.
        /// </summary>
        static FakeStorage()
        {
            _projects = SeedProjects();
            _tasks    = SeedTasks();
        }

        /// <summary>Returns a read-only list of all projects.</summary>
        internal static IReadOnlyList<ProjectDbModel> GetAllProjects()
            => _projects.AsReadOnly();

        /// <summary>Returns a read-only list of tasks for a given project id.</summary>
        internal static IReadOnlyList<TaskDbModel> GetTasksByProjectId(int projectId)
            => _tasks.Where(t => t.ProjectId == projectId).ToList().AsReadOnly();

        // ──────────────────────────────────────────────
        // Seed data
        // ──────────────────────────────────────────────

        private static List<ProjectDbModel> SeedProjects()
        {
            return new List<ProjectDbModel>
            {
                // Project 1 — Work — contains 10 tasks
                new ProjectDbModel(
                    id: 1,
                    name: "Corporate Website Redesign",
                    description: "Redesign and rebuild the company website with modern UI and improved performance.",
                    type: ProjectType.Work),

                // Project 2 — Educational — contains 2 tasks
                new ProjectDbModel(
                    id: 2,
                    name: "C# University Course",
                    description: "Complete all laboratory works for the C# and OOP university course.",
                    type: ProjectType.Educational),

                // Project 3 — Personal — no tasks yet
                new ProjectDbModel(
                    id: 3,
                    name: "Home Renovation Planning",
                    description: "Plan and organise the apartment renovation — budget, timeline, and contractors.",
                    type: ProjectType.Personal)
            };
        }

        private static List<TaskDbModel> SeedTasks()
        {
            // NOTE: today is approximately 2026-04-20, so tasks due before this date
            // that are not completed will be marked as overdue by the UI model.
            return new List<TaskDbModel>
            {
                // ── Project 1: Corporate Website Redesign (10 tasks) ──

                new TaskDbModel(
                    id: 1, projectId: 1,
                    name: "Audit existing website",
                    description: "Analyse current site structure, find performance bottlenecks and outdated content.",
                    priority: TaskPriority.High,
                    dueDate: new DateTime(2025, 12, 10),
                    isCompleted: true),

                new TaskDbModel(
                    id: 2, projectId: 1,
                    name: "Define target audience and goals",
                    description: "Conduct stakeholder interviews and document user personas and business goals.",
                    priority: TaskPriority.High,
                    dueDate: new DateTime(2025, 12, 20),
                    isCompleted: true),

                new TaskDbModel(
                    id: 3, projectId: 1,
                    name: "Create UI/UX wireframes",
                    description: "Design low-fidelity wireframes for homepage, about, services, and contact pages.",
                    priority: TaskPriority.Medium,
                    dueDate: new DateTime(2026, 1, 15),
                    isCompleted: true),

                new TaskDbModel(
                    id: 4, projectId: 1,
                    name: "Design visual style guide",
                    description: "Define brand colour palette, typography, spacing rules, and component library.",
                    priority: TaskPriority.Medium,
                    dueDate: new DateTime(2026, 2, 1),
                    isCompleted: true),

                new TaskDbModel(
                    id: 5, projectId: 1,
                    name: "Develop homepage layout",
                    description: "Implement homepage in HTML/CSS based on approved wireframes and style guide.",
                    priority: TaskPriority.High,
                    dueDate: new DateTime(2026, 2, 20),
                    isCompleted: false),  // overdue

                new TaskDbModel(
                    id: 6, projectId: 1,
                    name: "Implement responsive design",
                    description: "Ensure all pages render correctly on mobile, tablet, and desktop.",
                    priority: TaskPriority.High,
                    dueDate: new DateTime(2026, 3, 5),
                    isCompleted: false),  // overdue

                new TaskDbModel(
                    id: 7, projectId: 1,
                    name: "Set up backend REST API",
                    description: "Create API endpoints for contact form, newsletter signup, and CMS integration.",
                    priority: TaskPriority.Critical,
                    dueDate: new DateTime(2026, 3, 20),
                    isCompleted: false),  // overdue

                new TaskDbModel(
                    id: 8, projectId: 1,
                    name: "Integrate headless CMS",
                    description: "Connect the website to a CMS so the marketing team can manage content.",
                    priority: TaskPriority.Critical,
                    dueDate: new DateTime(2026, 4, 5),
                    isCompleted: false),  // overdue

                new TaskDbModel(
                    id: 9, projectId: 1,
                    name: "Conduct QA and user testing",
                    description: "Run end-to-end tests and collect feedback from a group of internal users.",
                    priority: TaskPriority.Medium,
                    dueDate: new DateTime(2026, 4, 30),
                    isCompleted: false),  // upcoming

                new TaskDbModel(
                    id: 10, projectId: 1,
                    name: "Deploy to production",
                    description: "Deploy the finished website to the production server, configure DNS and SSL.",
                    priority: TaskPriority.Critical,
                    dueDate: new DateTime(2026, 5, 15),
                    isCompleted: false),  // upcoming

                // ── Project 2: C# University Course (2 tasks) ──

                new TaskDbModel(
                    id: 11, projectId: 2,
                    name: "Complete Laboratory Work 1",
                    description: "Implement class models, fake storage, repository, and console app for the Task Manager.",
                    priority: TaskPriority.High,
                    dueDate: new DateTime(2026, 4, 25),
                    isCompleted: false),

                new TaskDbModel(
                    id: 12, projectId: 2,
                    name: "Complete Laboratory Work 2",
                    description: "Add LINQ queries, filtering, and sorting to the Task Manager application.",
                    priority: TaskPriority.Medium,
                    dueDate: new DateTime(2026, 5, 10),
                    isCompleted: false)
            };
        }
    }
}
