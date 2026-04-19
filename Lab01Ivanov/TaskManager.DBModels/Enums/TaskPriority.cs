namespace TaskManager.DBModels.Enums
{
    /// <summary>
    /// Defines the priority level of a task.
    /// </summary>
    public enum TaskPriority
    {
        /// <summary>Must be done immediately; blocks other work.</summary>
        Critical,

        /// <summary>Important, should be done soon.</summary>
        High,

        /// <summary>Normal priority.</summary>
        Medium,

        /// <summary>Nice to have, no rush.</summary>
        Low,

        /// <summary>Optional — done only if time permits.</summary>
        Optional
    }
}
