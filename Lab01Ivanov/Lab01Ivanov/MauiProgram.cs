using Microsoft.Extensions.Logging;
using Lab01Ivanov.Pages;
using Lab01Ivanov.ViewModels;
using TaskManager.Repository;
using TaskManager.Services;

namespace Lab01Ivanov
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // ── Layer 1: Repository (SQLite) ──────────────────────────────────
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "taskmanager.db3");
            builder.Services.AddSingleton(new DatabaseContext(dbPath));
            builder.Services.AddSingleton<IProjectRepository, ProjectRepository>();

            // ── Layer 2: Services ─────────────────────────────────────────────
            builder.Services.AddSingleton<IProjectService, ProjectService>();

            // ── Layer 3: ViewModels (Transient — fresh per navigation) ────────
            builder.Services.AddTransient<ProjectsViewModel>();
            builder.Services.AddTransient<ProjectDetailViewModel>();
            builder.Services.AddTransient<TaskDetailViewModel>();
            builder.Services.AddTransient<ProjectEditViewModel>();
            builder.Services.AddTransient<TaskEditViewModel>();

            // ── Pages + Shell ─────────────────────────────────────────────────
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<ProjectsPage>();
            builder.Services.AddTransient<ProjectDetailPage>();
            builder.Services.AddTransient<TaskDetailPage>();
            builder.Services.AddTransient<ProjectEditPage>();
            builder.Services.AddTransient<TaskEditPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
