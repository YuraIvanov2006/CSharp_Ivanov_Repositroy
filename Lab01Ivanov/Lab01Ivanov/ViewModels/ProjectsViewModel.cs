using Lab01Ivanov.Pages;
using TaskManager.DTOModels.Project;
using TaskManager.Services;

namespace Lab01Ivanov.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        private readonly IProjectService _service;

        private List<ProjectListDto> _projects = new();
        public List<ProjectListDto> Projects
        {
            get => _projects;
            set => SetProperty(ref _projects, value);
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); _ = LoadProjectsAsync(); }
        }

        private string? _selectedSort;
        public string? SelectedSort
        {
            get => _selectedSort;
            set { SetProperty(ref _selectedSort, value); _ = LoadProjectsAsync(); }
        }

        public List<string> SortOptions { get; } =
            new() { "Default", "Name ↑", "Name ↓", "Type", "Progress ↑", "Progress ↓" };

        private ProjectListDto? _selectedProject;
        public ProjectListDto? SelectedProject
        {
            get => _selectedProject;
            set
            {
                if (_selectedProject == value) return;
                _selectedProject = value; OnPropertyChanged();
                if (value != null) { NavigateToProject(value); _selectedProject = null; OnPropertyChanged(); }
            }
        }

        public Command AddProjectCommand { get; }
        public Command<ProjectListDto> DeleteProjectCommand { get; }
        public Command RefreshCommand { get; }

        public ProjectsViewModel(IProjectService service)
        {
            _service = service;
            AddProjectCommand    = new Command(async () => await Shell.Current.GoToAsync(nameof(ProjectEditPage)));
            DeleteProjectCommand = new Command<ProjectListDto>(async p => await DeleteProjectAsync(p));
            RefreshCommand       = new Command(async () => await LoadProjectsAsync());
        }

        public async Task LoadProjectsAsync()
        {
            await RunBusyAsync(async () =>
            {
                string? sort = _selectedSort == "Default" ? null : _selectedSort;
                Projects = await _service.GetAllProjectsAsync(
                    string.IsNullOrWhiteSpace(SearchText) ? null : SearchText, sort);
            });
        }

        private async void NavigateToProject(ProjectListDto p)
            => await Shell.Current.GoToAsync($"{nameof(ProjectDetailPage)}?projectId={p.Id}");

        private async Task DeleteProjectAsync(ProjectListDto p)
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Project", $"Delete '{p.Name}' and all its tasks?", "Delete", "Cancel");
            if (!confirm) return;
            await RunBusyAsync(async () =>
            {
                await _service.DeleteProjectAsync(p.Id);
                await LoadProjectsAsync();
            });
        }
    }
}
