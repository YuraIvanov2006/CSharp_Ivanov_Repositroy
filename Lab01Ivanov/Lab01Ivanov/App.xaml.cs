using Microsoft.Extensions.DependencyInjection;

namespace Lab01Ivanov
{
    public partial class App : Application
    {
        public App(AppShell shell)
        {
            InitializeComponent();
            MainPage = shell;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(MainPage!);
            window.Width  = 393;
            window.Height = 852;
            return window;
        }
    }
}