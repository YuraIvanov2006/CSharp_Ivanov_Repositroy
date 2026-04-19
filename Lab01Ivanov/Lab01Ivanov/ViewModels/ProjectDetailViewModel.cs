using Lab01Ivanov.Pages;
using TaskManager.DTOModels.Project;
using TaskManager.DTOModels.Task;
using TaskManager.Services;

namespace Lab01Ivanov.ViewModels
{
    public class ProjectDetailViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IProjectService _service;
        private int _projectId;

        private ProjectDetailDto? _project;
        public ProjectDetailDto? Project
        {
            get => _project;
            set => SetProperty(ref _project, value);
        }

        private string _taskSearch = string.Empty;
        public string TaskSearch
        {
            get => _taskSearch;
            set { SetProperty(ref _taskSearch, value); _ = ReloadAsync(); }
        }

        private string? _taskSort;
        public string? TaskSort
        {
            get => _taskSort;
            set { SetProperty(ref _taskSort, value); _ = ReloadAsync(); }
        }

        public List<string> TaskSortOptions { get; } =
            new() { "Default", "Name ↑", "Name ↓", "Priority ↑", "Priority ↓", "Due Date ↑", "Due Date ↓" };

        private TaskListDto? _selectedTask;
        public TaskListDto? SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (_selectedTask == value) return;
                _selectedTask = value; OnPropertyChanged();
                if (value != null) { NavigateToTask(value); _selectedTask = null; OnPropertyChanged(); }
            }
        }

        public Command EditProjectCommand   { get; }
        public Command AddTaskCommand       { get; }
        public Command<TaskListDto> DeleteTaskCommand { get; }

        public ProjectDetailViewModel(IProjectService service)
        {
            _service = service;
            EditProjectCommand = new Command(async () =>
                await Shell.Current.GoToAsync($"{nameof(ProjectEditPage)}?projectId={_projectId}"));
            AddTaskCommand = new Command(async () =>
                await Shell.Current.GoToAsync($"{nameof(TaskEditPage)}?projectId={_projectId}"));
            DeleteTaskCommand = new Command<TaskListDto>(async t => await DeleteTaskAsync(t));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("projectId", out var v) && int.TryParse(v?.ToString(), out int id))
            { _projectId = id; _ = ReloadAsync(); }
        }

        public async Task ReloadAsync() => await RunBusyAsync(async () =>
        {
            string? sort   = _taskSort == "Default" ? null : _taskSort;
            string? search = string.IsNullOrWhiteSpace(TaskSearch) ? null : TaskSearch;
            Project = await _service.GetProjectDetailAsync(_projectId, search, sort);
        });

        private async void NavigateToTask(TaskListDto t)
            => await Shell.Current.GoToAsync($"{nameof(TaskDetailPage)}?taskId={t.Id}");

        private async Task DeleteTaskAsync(TaskListDto t)
        {
            bool ok = await Shell.Current.DisplayAlert("Delete Task", $"Delete '{t.Name}'?", "Delete", "Cancel");
            if (!ok) return;
            await RunBusyAsync(async () => { await _service.DeleteTaskAsync(t.Id); await ReloadAsync(); });
        }
    }
}
