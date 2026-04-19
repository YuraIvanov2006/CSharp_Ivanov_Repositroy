using TaskManager.Services;
using TaskManager.UIModels;

namespace Lab01Ivanov.Pages
{
    /// <summary>
    /// Page 1: Shows all projects.
    /// Receives ITaskManagerRepository via constructor injection (DI).
    /// </summary>
    public partial class ProjectsPage : ContentPage
    {
        private readonly ITaskManagerRepository _repository;

        public ProjectsPage(ITaskManagerRepository repository)
        {
            InitializeComponent();
            _repository = repository;
        }

        // Reload projects every time the page appears (handles back-navigation)
        protected override void OnAppearing()
        {
            base.OnAppearing();

            var projects = _repository.GetAllProjects();

            // Load tasks for each project so Progress can be calculated on the list
            foreach (var project in projects)
                _repository.LoadTasksForProject(project);

            ProjectsCollection.ItemsSource = projects;
        }

        // Navigate to ProjectDetailPage, passing the selected project's Id
        private async void OnProjectSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is ProjectUiModel project)
            {
                ((CollectionView)sender).SelectedItem = null; // deselect
                await Shell.Current.GoToAsync($"{nameof(ProjectDetailPage)}?projectId={project.Id}");
            }
        }
    }
}
