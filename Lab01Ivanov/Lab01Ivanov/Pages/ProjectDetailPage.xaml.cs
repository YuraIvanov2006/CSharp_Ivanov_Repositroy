using Lab01Ivanov.ViewModels;

namespace Lab01Ivanov.Pages
{
    public partial class ProjectDetailPage : ContentPage
    {
        private readonly ProjectDetailViewModel _viewModel;

        public ProjectDetailPage(ProjectDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        // Allowed by task: reload on appear so edits/deletes are reflected
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.ReloadAsync();
        }
    }
}
