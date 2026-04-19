using TaskManager.Services;
using TaskManager.UIModels;

namespace Lab01Ivanov.Pages
{
    /// <summary>
    /// Page 3: Shows full details of a single task.
    /// Receives projectId + taskId via Shell query properties.
    /// </summary>
    [QueryProperty(nameof(ProjectId), "projectId")]
    [QueryProperty(nameof(TaskId),    "taskId")]
    public partial class TaskDetailPage : ContentPage
    {
        private readonly ITaskManagerRepository _repository;
        private int _projectId;
        private int _taskId;

        public TaskDetailPage(ITaskManagerRepository repository)
        {
            InitializeComponent();
            _repository = repository;
        }

        public int ProjectId { set { _projectId = value; TryLoad(); } }
        public int TaskId    { set { _taskId    = value; TryLoad(); } }

        // Shell sets both properties independently; load only when both are received
        private void TryLoad()
        {
            if (_projectId == 0 || _taskId == 0) return;

            TaskUiModel? task = _repository.GetTaskById(_projectId, _taskId);
            if (task == null) return;

            BindTask(task);
        }

        private void BindTask(TaskUiModel task)
        {
            // Status banner
            if (task.IsCompleted)
            {
                StatusBanner.BackgroundColor = Color.FromArgb("#1A2E1A");
                StatusLabel.Text      = "✅  Completed";
                StatusLabel.TextColor = Color.FromArgb("#66BB6A");
            }
            else if (task.IsOverdue)
            {
                StatusBanner.BackgroundColor = Color.FromArgb("#2E1A1A");
                StatusLabel.Text      = "🔴  Overdue";
                StatusLabel.TextColor = Color.FromArgb("#EF5350");
            }
            else
            {
                StatusBanner.BackgroundColor = Color.FromArgb("#16213E");
                StatusLabel.Text      = "🔵  In Progress";
                StatusLabel.TextColor = Color.FromArgb("#4FC3F7");
            }

            // Field values
            TaskNameLabel.Text  = task.Name;
            TaskDescLabel.Text  = task.Description;
            PriorityLabel.Text  = task.Priority.ToString();
            DueDateLabel.Text   = task.DueDate.ToString("yyyy-MM-dd");

            CompletedLabel.Text      = task.IsCompleted ? "Yes ✓" : "No";
            CompletedLabel.TextColor = task.IsCompleted
                ? Color.FromArgb("#66BB6A")
                : Color.FromArgb("#8080A0");

            OverdueLabel.Text      = task.IsOverdue ? "Yes !" : "No";
            OverdueLabel.TextColor = task.IsOverdue
                ? Color.FromArgb("#EF5350")
                : Color.FromArgb("#8080A0");
        }
    }
}
