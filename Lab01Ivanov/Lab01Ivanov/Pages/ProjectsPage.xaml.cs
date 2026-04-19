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

        // OnAppearing triggers the ViewModel to load data
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadProjects();
        }
    }
}
