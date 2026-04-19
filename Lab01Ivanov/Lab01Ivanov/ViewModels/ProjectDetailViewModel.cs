using Lab01Ivanov.Pages;
using TaskManager.DTOModels.Project;
using TaskManager.DTOModels.Task;
using TaskManager.Services;

namespace Lab01Ivanov.ViewModels
{
    /// <summary>
    /// ViewModel for the Project detail page.
    /// Implements IQueryAttributable so Shell can pass projectId without any code in .xaml.cs.
    /// </summary>
    public class ProjectDetailViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IProjectService _projectService;

        private ProjectDetailDto? _project;
        public ProjectDetailDto? Project
        {
            get => _project;
            set => SetProperty(ref _project, value);
        }

        private TaskListDto? _selectedTask;
        /// <summary>Binding target for task CollectionView.SelectedItem.</summary>
        public TaskListDto? SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (_selectedTask == value) return;
                _selectedTask = value;
                OnPropertyChanged();
                if (value != null)
                {
                    NavigateToTask(value);
                    _selectedTask = null;
                    OnPropertyChanged();
                }
            }
        }

        public ProjectDetailViewModel(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Shell calls this automatically with the query parameters from the navigation URL.
        /// Replaces [QueryProperty] attribute — keeps .xaml.cs clean.
        /// </summary>
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("projectId", out object? val)
                && int.TryParse(val?.ToString(), out int id))
            {
                Project = _projectService.GetProjectDetail(id);
            }
        }

        private async void NavigateToTask(TaskListDto task)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(TaskDetailPage)}?projectId={task.ProjectId}&taskId={task.Id}");
        }
    }
}
