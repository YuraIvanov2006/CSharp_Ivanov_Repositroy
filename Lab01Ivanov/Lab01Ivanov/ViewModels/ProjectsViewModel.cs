using Lab01Ivanov.Pages;
using TaskManager.DTOModels.Project;
using TaskManager.Services;

namespace Lab01Ivanov.ViewModels
{
    /// <summary>
    /// ViewModel for the Projects list page.
    /// Loads projects via IProjectService (injected by IoC).
    /// Handles navigation to ProjectDetailPage.
    /// </summary>
    public class ProjectsViewModel : BaseViewModel
    {
        private readonly IProjectService _projectService;

        private List<ProjectListDto> _projects = new();
        public List<ProjectListDto> Projects
        {
            get => _projects;
            set => SetProperty(ref _projects, value);
        }

        private ProjectListDto? _selectedProject;
        /// <summary>
        /// Binding target for CollectionView.SelectedItem.
        /// Navigation triggers automatically when a project is selected.
        /// </summary>
        public ProjectListDto? SelectedProject
        {
            get => _selectedProject;
            set
            {
                if (_selectedProject == value) return;
                _selectedProject = value;
                OnPropertyChanged();
                if (value != null)
                {
                    NavigateToProject(value);
                    _selectedProject = null;
                    OnPropertyChanged();
                }
            }
        }

        public ProjectsViewModel(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>Called by the page in OnAppearing to load/refresh the list.</summary>
        public void LoadProjects()
        {
            Projects = _projectService.GetAllProjects();
        }

        private async void NavigateToProject(ProjectListDto project)
        {
            await Shell.Current.GoToAsync($"{nameof(ProjectDetailPage)}?projectId={project.Id}");
        }
    }
}
