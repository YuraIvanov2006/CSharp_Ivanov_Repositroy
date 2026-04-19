using TaskManager.DBModels.Enums;
using TaskManager.DTOModels.Task;
using TaskManager.Services;

namespace Lab01Ivanov.ViewModels
{
    /// <summary>
    /// ViewModel for Add/Edit Task page.
    /// taskId == 0 → Add mode; taskId > 0 → Edit mode.
    /// projectId is always required (owner project).
    /// </summary>
    public class TaskEditViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IProjectService _service;
        private int _taskId;
        private int _projectId;

        public bool IsEditMode => _taskId > 0;
        public string PageTitle => IsEditMode ? "Edit Task" : "Add Task";

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private TaskPriority _priority;
        public TaskPriority Priority
        {
            get => _priority;
            set => SetProperty(ref _priority, value);
        }

        private DateTime _dueDate = DateTime.Today.AddDays(7);
        public DateTime DueDate
        {
            get => _dueDate;
            set => SetProperty(ref _dueDate, value);
        }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set => SetProperty(ref _isCompleted, value);
        }

        public List<TaskPriority> PriorityOptions { get; } =
            Enum.GetValues<TaskPriority>().ToList();

        public Command SaveCommand   { get; }
        public Command CancelCommand { get; }

        public TaskEditViewModel(IProjectService service)
        {
            _service      = service;
            SaveCommand   = new Command(async () => await SaveAsync(), () => !IsBusy);
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("projectId", out var pv) && int.TryParse(pv?.ToString(), out int pid))
                _projectId = pid;

            if (query.TryGetValue("taskId", out var tv) && int.TryParse(tv?.ToString(), out int tid) && tid > 0)
                _ = LoadForEditAsync(tid);

            OnPropertyChanged(nameof(IsEditMode));
            OnPropertyChanged(nameof(PageTitle));
        }

        private async Task LoadForEditAsync(int id)
        {
            _taskId = id;
            OnPropertyChanged(nameof(IsEditMode));
            OnPropertyChanged(nameof(PageTitle));
            var dto = await _service.GetTaskForEditAsync(id);
            if (dto == null) return;
            Name        = dto.Name;
            Description = dto.Description;
            Priority    = dto.Priority;
            DueDate     = dto.DueDate;
            IsCompleted = dto.IsCompleted;
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlert("Validation", "Task name is required.", "OK");
                return;
            }
            await RunBusyAsync(async () =>
            {
                if (IsEditMode)
                    await _service.UpdateTaskAsync(new UpdateTaskDto
                    {
                        Id = _taskId, ProjectId = _projectId, Name = Name,
                        Description = Description, Priority = Priority,
                        DueDate = DueDate, IsCompleted = IsCompleted
                    });
                else
                    await _service.AddTaskAsync(new CreateTaskDto
                    {
                        ProjectId = _projectId, Name = Name, Description = Description,
                        Priority = Priority, DueDate = DueDate, IsCompleted = IsCompleted
                    });

                await Shell.Current.GoToAsync("..");
            });
        }
    }
}
