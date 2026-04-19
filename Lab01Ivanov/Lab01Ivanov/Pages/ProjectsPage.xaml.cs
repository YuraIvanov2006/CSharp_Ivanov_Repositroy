using Lab01Ivanov.ViewModels;

namespace Lab01Ivanov.Pages
{
    public partial class ProjectsPage : ContentPage
    {
        private readonly ProjectsViewModel _viewModel;

        public ProjectsPage(ProjectsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        // Allowed by task: async OnAppearing to trigger data load
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadProjectsAsync();
        }
    }
}
