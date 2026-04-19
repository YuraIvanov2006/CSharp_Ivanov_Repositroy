using SQLite;
using TaskManager.DBModels;
using TaskManager.DBModels.Enums;

namespace TaskManager.Repository
{
    /// <summary>
    /// Manages the SQLite connection, table creation, and first-run seeding.
    /// Registered as Singleton in the IoC container.
    /// The database path is injected from the MAUI app (FileSystem.AppDataDirectory).
    /// </summary>
    public class DatabaseContext
    {
        private readonly SQLiteAsyncConnection _db;
        private bool _initialized;

        public DatabaseContext(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
        }

        /// <summary>
        /// Creates tables if missing. Seeds data ONLY on first run (when Projects table is empty).
        /// Safe to call multiple times — idempotent after first init.
        /// </summary>
        public async Task InitializeAsync()
        {
            if (_initialized) return;

            await _db.CreateTableAsync<ProjectDbModel>();
            await _db.CreateTableAsync<TaskDbModel>();

            if (await _db.Table<ProjectDbModel>().CountAsync() == 0)
                await SeedAsync();

            _initialized = true;
        }

        // ── CRUD: Projects ────────────────────────────────────────────────────

        public Task<List<ProjectDbModel>> GetAllProjectsAsync()
            => _db.Table<ProjectDbModel>().ToListAsync();

        public Task<ProjectDbModel?> GetProjectByIdAsync(int id)
            => _db.Table<ProjectDbModel>().Where(p => p.Id == id).FirstOrDefaultAsync();

        public Task<int> InsertProjectAsync(ProjectDbModel p)  => _db.InsertAsync(p);
        public Task      UpdateProjectAsync(ProjectDbModel p)  => _db.UpdateAsync(p);
        public Task      DeleteProjectAsync(ProjectDbModel p)  => _db.DeleteAsync(p);

        // ── CRUD: Tasks ───────────────────────────────────────────────────────

        public Task<List<TaskDbModel>> GetTasksByProjectIdAsync(int projectId)
            => _db.Table<TaskDbModel>().Where(t => t.ProjectId == projectId).ToListAsync();

        public Task<TaskDbModel?> GetTaskByIdAsync(int taskId)
            => _db.Table<TaskDbModel>().Where(t => t.Id == taskId).FirstOrDefaultAsync();

        public Task<int> InsertTaskAsync(TaskDbModel t) => _db.InsertAsync(t);
        public Task      UpdateTaskAsync(TaskDbModel t) => _db.UpdateAsync(t);
        public Task      DeleteTaskAsync(TaskDbModel t) => _db.DeleteAsync(t);

        /// <summary>Deletes all tasks belonging to a project (cascade delete).</summary>
        public async Task DeleteTasksByProjectIdAsync(int projectId)
        {
            var tasks = await GetTasksByProjectIdAsync(projectId);
            foreach (var t in tasks)
                await _db.DeleteAsync(t);
        }

        // ── Seed data (first run only) ────────────────────────────────────────

        private async Task SeedAsync()
        {
            var p1 = new ProjectDbModel("Corporate Website Redesign",
                "Redesign and rebuild the company website with modern UI and improved performance.",
                ProjectType.Work);
            var p2 = new ProjectDbModel("C# University Course",
                "Complete all laboratory works for the C# and OOP university course.",
                ProjectType.Educational);
            var p3 = new ProjectDbModel("Home Renovation Planning",
                "Plan and organise the apartment renovation — budget, timeline, and contractors.",
                ProjectType.Personal);

            await _db.InsertAsync(p1);
            await _db.InsertAsync(p2);
            await _db.InsertAsync(p3);

            // 10 tasks for project 1
            var tasks = new[]
            {
                new TaskDbModel(p1.Id, "Audit existing website",
                    "Analyse site structure and find performance bottlenecks.",
                    TaskPriority.High, new DateTime(2025, 12, 10), true),
                new TaskDbModel(p1.Id, "Define target audience and goals",
                    "Conduct stakeholder interviews and document personas.",
                    TaskPriority.High, new DateTime(2025, 12, 20), true),
                new TaskDbModel(p1.Id, "Create UI/UX wireframes",
                    "Design low-fidelity wireframes for all main pages.",
                    TaskPriority.Medium, new DateTime(2026, 1, 15), true),
                new TaskDbModel(p1.Id, "Design visual style guide",
                    "Define colours, typography, spacing, and component library.",
                    TaskPriority.Medium, new DateTime(2026, 2, 1), true),
                new TaskDbModel(p1.Id, "Develop homepage layout",
                    "Implement homepage in HTML/CSS based on wireframes.",
                    TaskPriority.High, new DateTime(2026, 2, 20), false),
                new TaskDbModel(p1.Id, "Implement responsive design",
                    "Ensure all pages work on mobile, tablet, and desktop.",
                    TaskPriority.High, new DateTime(2026, 3, 5), false),
                new TaskDbModel(p1.Id, "Set up backend REST API",
                    "Create API endpoints for contact form and CMS integration.",
                    TaskPriority.Critical, new DateTime(2026, 3, 20), false),
                new TaskDbModel(p1.Id, "Integrate headless CMS",
                    "Connect the website to a CMS for the marketing team.",
                    TaskPriority.Critical, new DateTime(2026, 4, 5), false),
                new TaskDbModel(p1.Id, "Conduct QA and user testing",
                    "Run end-to-end tests and collect feedback from users.",
                    TaskPriority.Medium, new DateTime(2026, 4, 30), false),
                new TaskDbModel(p1.Id, "Deploy to production",
                    "Deploy website, configure DNS and SSL.",
                    TaskPriority.Critical, new DateTime(2026, 5, 15), false),

                // 2 tasks for project 2
                new TaskDbModel(p2.Id, "Complete Laboratory Work 1",
                    "Implement class models, storage, and console app.",
                    TaskPriority.High, new DateTime(2026, 4, 25), true),
                new TaskDbModel(p2.Id, "Complete Laboratory Work 2",
                    "Add MAUI UI with MVVM architecture.",
                    TaskPriority.Medium, new DateTime(2026, 5, 10), false),
            };

            await _db.InsertAllAsync(tasks);
        }
    }
}
