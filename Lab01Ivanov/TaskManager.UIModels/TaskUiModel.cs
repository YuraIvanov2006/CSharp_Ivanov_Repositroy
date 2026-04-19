using TaskManager.DBModels;
using TaskManager.DBModels.Enums;

namespace TaskManager.UIModels
{
    /// <summary>
    /// UI/display model for the Task entity.
    /// Responsibility: wrapping TaskDbModel data and adding computed properties + display methods.
    /// This class is used for showing, creating and editing tasks in the application.
    /// </summary>
    public class TaskUiModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Computed: true when the task is not completed AND its due date is in the past.
        /// Recalculated every time it is accessed (no setter needed).
        /// </summary>
        public bool IsOverdue => !IsCompleted && DueDate < DateTime.Today;

        /// <summary>
        /// Creates a TaskUiModel by copying data from a storage model (TaskDbModel).
        /// </summary>
        public TaskUiModel(TaskDbModel dbModel)
        {
            Id          = dbModel.Id;
            ProjectId   = dbModel.ProjectId;
            Name        = dbModel.Name;
            Description = dbModel.Description;
            Priority    = dbModel.Priority;
            DueDate     = dbModel.DueDate;
            IsCompleted = dbModel.IsCompleted;
        }

        /// <summary>
        /// Displays a short one-line summary of the task (used in list views).
        /// Status legend: [✓] = completed, [!] = overdue, [ ] = pending.
        /// </summary>
        public void DisplaySummary(int index)
        {
            string status = IsCompleted ? "[✓]" : IsOverdue ? "[!]" : "[ ]";
            string priorityTag = Priority == TaskPriority.Critical ? " ⚠" : "";
            Console.WriteLine($"  {index,2}. {status} {Name}{priorityTag}");
            Console.WriteLine($"       Priority: {Priority,-8}  Due: {DueDate:yyyy-MM-dd}");
        }

        /// <summary>
        /// Displays full details of this task (used in detail view).
        /// </summary>
        public void DisplayDetails()
        {
            Console.WriteLine($"  Task #{Id}: {Name}");
            Console.WriteLine($"  Description : {Description}");
            Console.WriteLine($"  Priority    : {Priority}");
            Console.WriteLine($"  Due Date    : {DueDate:yyyy-MM-dd}");
            Console.WriteLine($"  Completed   : {(IsCompleted ? "Yes ✓" : "No")}");
            Console.WriteLine($"  Overdue     : {(IsOverdue ? "Yes !" : "No")}");
        }
    }
}
