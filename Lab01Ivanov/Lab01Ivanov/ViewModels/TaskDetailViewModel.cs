using Lab01Ivanov.Pages;
using TaskManager.DTOModels.Task;
using TaskManager.Services;

namespace Lab01Ivanov.ViewModels
{
    public class TaskDetailViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IProjectService _service;

        private TaskDetailDto? _task;
        public TaskDetailDto? Task
        {
            get => _task;
            set => SetProperty(ref _task, value);
        }

        public Command EditCommand { get; }

        public TaskDetailViewModel(IProjectService service)
        {
            _service    = service;
            EditCommand = new Command(async () =>
                await Shell.Current.GoToAsync($"{nameof(TaskEditPage)}?taskId={Task?.Id}&projectId={Task?.ProjectId}"));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("taskId", out var v) && int.TryParse(v?.ToString(), out int id))
                _ = LoadAsync(id);
        }

        private async Task LoadAsync(int taskId) => await RunBusyAsync(async () =>
        {
            Task = await _service.GetTaskDetailAsync(taskId);
        });
    }
}
