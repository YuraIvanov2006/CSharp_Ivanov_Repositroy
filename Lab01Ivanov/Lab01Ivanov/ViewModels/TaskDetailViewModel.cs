using TaskManager.DTOModels.Task;
using TaskManager.Services;

namespace Lab01Ivanov.ViewModels
{
    /// <summary>
    /// ViewModel for the Task detail page.
    /// Implements IQueryAttributable to receive projectId + taskId from Shell navigation.
    /// </summary>
    public class TaskDetailViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IProjectService _projectService;

        private TaskDetailDto? _task;
        public TaskDetailDto? Task
        {
            get => _task;
            set => SetProperty(ref _task, value);
        }

        public TaskDetailViewModel(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            bool hasProject = query.TryGetValue("projectId", out object? pVal)
                              && int.TryParse(pVal?.ToString(), out int projectId);
            bool hasTask    = query.TryGetValue("taskId",    out object? tVal)
                              && int.TryParse(tVal?.ToString(), out int taskId);

            if (hasProject && hasTask)
            {
                // Need to re-parse — C# requires declared vars inside if
                int.TryParse(pVal?.ToString(), out int pid);
                int.TryParse(tVal?.ToString(), out int tid);
                Task = _projectService.GetTaskDetail(pid, tid);
            }
        }
    }
}
