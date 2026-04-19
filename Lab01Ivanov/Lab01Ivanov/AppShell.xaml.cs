using Lab01Ivanov.Pages;

namespace Lab01Ivanov
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register sub-page routes so Shell can navigate to them
            Routing.RegisterRoute(nameof(ProjectDetailPage), typeof(ProjectDetailPage));
            Routing.RegisterRoute(nameof(TaskDetailPage),    typeof(TaskDetailPage));
        }
    }
}
