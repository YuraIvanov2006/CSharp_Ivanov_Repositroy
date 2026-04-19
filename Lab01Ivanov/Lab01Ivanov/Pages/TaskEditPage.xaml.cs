using Lab01Ivanov.ViewModels;

namespace Lab01Ivanov.Pages
{
    public partial class TaskEditPage : ContentPage
    {
        public TaskEditPage(TaskEditViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
