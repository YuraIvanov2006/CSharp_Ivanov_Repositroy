using Lab01Ivanov.Pages;
using TaskManager.Services;

namespace Lab01Ivanov
{
    public partial class AppShell : Shell
    {
        public AppShell(IProjectService projectService)
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ProjectDetailPage), typeof(ProjectDetailPage));
            Routing.RegisterRoute(nameof(TaskDetailPage),    typeof(TaskDetailPage));
            Routing.RegisterRoute(nameof(ProjectEditPage),   typeof(ProjectEditPage));
            Routing.RegisterRoute(nameof(TaskEditPage),      typeof(TaskEditPage));

            // Initialize database (create tables + seed on first run)
            _ = projectService.InitializeAsync();
        }
    }
}
