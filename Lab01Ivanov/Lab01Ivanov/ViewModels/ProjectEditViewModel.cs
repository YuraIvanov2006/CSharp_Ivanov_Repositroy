using TaskManager.DBModels.Enums;
using TaskManager.DTOModels.Project;
using TaskManager.Services;

namespace Lab01Ivanov.ViewModels
{
    /// <summary>
    /// ViewModel for Add/Edit Project page.
    /// Mode is determined by projectId query param:
    ///   projectId == 0 (or absent) → Add mode
    ///   projectId > 0              → Edit mode
    /// </summary>
    public class ProjectEditViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IProjectService _service;
        private int _projectId;

        public bool IsEditMode => _projectId > 0;
        public string PageTitle => IsEditMode ? "Edit Project" : "Add Project";

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

        private ProjectType _type;
        public ProjectType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public List<ProjectType> TypeOptions { get; } =
            Enum.GetValues<ProjectType>().ToList();

        public Command SaveCommand   { get; }
        public Command CancelCommand { get; }

        public ProjectEditViewModel(IProjectService service)
        {
            _service      = service;
            SaveCommand   = new Command(async () => await SaveAsync(), () => !IsBusy);
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("projectId", out var v) &&
                int.TryParse(v?.ToString(), out int id) && id > 0)
                _ = LoadForEditAsync(id);
            OnPropertyChanged(nameof(IsEditMode));
            OnPropertyChanged(nameof(PageTitle));
        }

        private async Task LoadForEditAsync(int id)
        {
            _projectId = id;
            OnPropertyChanged(nameof(IsEditMode));
            OnPropertyChanged(nameof(PageTitle));
            var dto = await _service.GetProjectForEditAsync(id);
            if (dto == null) return;
            Name        = dto.Name;
            Description = dto.Description;
            Type        = dto.Type;
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlert("Validation", "Project name is required.", "OK");
                return;
            }
            await RunBusyAsync(async () =>
            {
                if (IsEditMode)
                    await _service.UpdateProjectAsync(new UpdateProjectDto
                        { Id = _projectId, Name = Name, Description = Description, Type = Type });
                else
                    await _service.AddProjectAsync(new CreateProjectDto
                        { Name = Name, Description = Description, Type = Type });

                await Shell.Current.GoToAsync("..");
            });
        }
    }
}
