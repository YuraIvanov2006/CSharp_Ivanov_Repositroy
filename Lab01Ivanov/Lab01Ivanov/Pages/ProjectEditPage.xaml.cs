using Lab01Ivanov.ViewModels;

namespace Lab01Ivanov.Pages
{
    public partial class ProjectEditPage : ContentPage
    {
        public ProjectEditPage(ProjectEditViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
