using Lab01Ivanov.ViewModels;

namespace Lab01Ivanov.Pages
{
    public partial class TaskDetailPage : ContentPage
    {
        public TaskDetailPage(TaskDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
