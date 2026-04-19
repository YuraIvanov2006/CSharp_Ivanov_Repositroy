using TaskManager.DTOModels.Project;
using TaskManager.DTOModels.Task;

namespace TaskManager.Services
{
    /// <summary>
    /// Contract for the project service layer.
    /// UI components depend on this interface — never on the concrete class.
    /// </summary>
    public interface IProjectService
    {
        /// <summary>Returns all projects as list DTOs.</summary>
        List<ProjectListDto> GetAllProjects();

        /// <summary>Returns full project details with tasks, or null if not found.</summary>
        ProjectDetailDto? GetProjectDetail(int projectId);

        /// <summary>Returns full task details, or null if not found.</summary>
        TaskDetailDto? GetTaskDetail(int projectId, int taskId);
    }
}
