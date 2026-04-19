using Microsoft.Extensions.Logging;
using TaskManager.Services;
using Lab01Ivanov.Pages;

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

            // ── Inversion of Control / Dependency Injection ───────────────────
            // ITaskManagerRepository is registered as Singleton:
            //   one shared instance lives for the whole app lifetime.
            // Pages are Transient: a fresh instance is created on each navigation.
            builder.Services.AddSingleton<ITaskManagerRepository, TaskManagerRepository>();
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
