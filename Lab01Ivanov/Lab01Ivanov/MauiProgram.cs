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

            // ── IoC / Dependency Injection registrations ──────────────────────
            //
            // Layer 1 — Repository (Singleton: one shared data source)
            builder.Services.AddSingleton<IProjectRepository, ProjectRepository>();

            // Layer 2 — Services (Singleton: stateless converters)
            builder.Services.AddSingleton<IProjectService, ProjectService>();

            // Layer 3 — ViewModels (Transient: fresh instance per navigation)
            builder.Services.AddTransient<ProjectsViewModel>();
            builder.Services.AddTransient<ProjectDetailViewModel>();
            builder.Services.AddTransient<TaskDetailViewModel>();

            // Pages (Transient — resolved with injected ViewModels)
            builder.Services.AddTransient<ProjectsPage>();
            builder.Services.AddTransient<ProjectDetailPage>();
            builder.Services.AddTransient<TaskDetailPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
