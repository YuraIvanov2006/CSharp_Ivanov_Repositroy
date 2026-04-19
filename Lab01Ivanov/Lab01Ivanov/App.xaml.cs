using Microsoft.Extensions.DependencyInjection;

namespace Lab01Ivanov
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            // Phone-like dimensions (iPhone 14 size)
            window.Width  = 393;
            window.Height = 852;

            return window;
        }
    }
}