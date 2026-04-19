using TaskManager.Services;
using TaskManager.UIModels;

namespace Lab01Ivanov.Pages
{
    /// <summary>
    /// Page 2: Shows a project's details and its task list.
    /// Receives projectId via Shell query property, loads data from repository.
    /// </summary>
    [QueryProperty(nameof(ProjectId), "projectId")]
    public partial class ProjectDetailPage : ContentPage
    {
        private readonly ITaskManagerRepository _repository;
        private ProjectUiModel? _project;

        public ProjectDetailPage(ITaskManagerRepository repository)
        {
            InitializeComponent();
            _repository = repository;
        }

        // Shell sets this property when navigating with ?projectId=X
        public int ProjectId
        {
            set => LoadProject(value);
        }

        private void LoadProject(int id)
        {
            _project = _repository.GetProjectById(id);
            if (_project == null) return;

            // Populate project info labels
            ProjectNameLabel.Text     = _project.Name;
            ProjectTypeLabel.Text     = _project.Type.ToString();
            ProjectDescLabel.Text     = _project.Description;
            ProjectProgressLabel.Text = $"Progress: {_project.Progress:F1}%";
            ProjectProgressBar.Progress = _project.ProgressFraction;

            int done    = _project.Tasks.Count(t => t.IsCompleted);
            int overdue = _project.Tasks.Count(t => t.IsOverdue);
            ProjectStatsLabel.Text = $"{_project.Tasks.Count} tasks  •  {done} completed  •  {overdue} overdue";

            // Wrap tasks with display helpers, then bind
            TasksCollection.ItemsSource = _project.Tasks
                .Select(t => new TaskDisplayItem(t))
                .ToList();
        }

        private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is TaskDisplayItem item)
            {
                ((CollectionView)sender).SelectedItem = null;
                await Shell.Current.GoToAsync(
                    $"{nameof(TaskDetailPage)}?projectId={item.Task.ProjectId}&taskId={item.Task.Id}");
            }
        }
    }

    /// <summary>
    /// Thin wrapper adding display-only properties (color, icon) for XAML binding.
    /// Keeps TaskUiModel clean from UI concerns.
    /// </summary>
    public class TaskDisplayItem
    {
        public TaskUiModel Task { get; }

        // Forwarded properties for XAML binding convenience
        public string Name      => Task.Name;
        public string Priority  => Task.Priority.ToString();
        public DateTime DueDate => Task.DueDate;

        /// <summary>Emoji icon reflecting task status.</summary>
        public string StatusIcon =>
            Task.IsCompleted ? "✅" : Task.IsOverdue ? "🔴" : "🔵";

        /// <summary>Background colour of the task card based on status.</summary>
        public Color StatusColor =>
            Task.IsCompleted ? Color.FromArgb("#1A2E1A") :
            Task.IsOverdue   ? Color.FromArgb("#2E1A1A") :
                               Color.FromArgb("#16213E");

        public TaskDisplayItem(TaskUiModel task) => Task = task;
    }
}
