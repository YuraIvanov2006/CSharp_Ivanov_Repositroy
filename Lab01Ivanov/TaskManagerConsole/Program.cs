using TaskManager.Services;
using TaskManager.UIModels;

/*
 * TaskManager Console Application
 * ─────────────────────────────────────────────────────────────────────────────
 * Entry point for the Task Manager lab application.
 *
 * Navigation flow:
 *   1. App starts → loads all projects from TaskManagerRepository.
 *   2. User sees a numbered list of projects.
 *   3. User picks a project → full project details + its tasks are displayed.
 *   4. User can pick a specific task to see full task details.
 *   5. User can go back to the project list at any time.
 *   6. User can quit with 'q'.
 * ─────────────────────────────────────────────────────────────────────────────
 */

// ── Bootstrap ────────────────────────────────────────────────────────────────
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Title = "Task Manager";

var repository = new TaskManagerRepository();
List<ProjectUiModel> projects = new List<ProjectUiModel>();

// ── Main loop ────────────────────────────────────────────────────────────────
bool running = true;
while (running)
{
    // Step 1: Load projects (or reload if returning from a project view)
    if (projects.Count == 0)
    {
        projects = repository.GetAllProjects();
    }

    ShowProjectList(projects);

    Console.Write("\n  Enter project number to open, or [q] to quit: ");
    string? input = Console.ReadLine()?.Trim().ToLower();

    if (input == "q")
    {
        running = false;
        continue;
    }

    // Validate selection
    if (!int.TryParse(input, out int projectChoice) ||
        projectChoice < 1 || projectChoice > projects.Count)
    {
        PrintWarning("  Invalid selection. Please try again.");
        Pause();
        continue;
    }

    // Step 2: Show selected project
    ProjectUiModel selectedProject = projects[projectChoice - 1];
    ShowProjectDetail(selectedProject, repository);

    // Step 3: Task / navigation sub-loop
    bool inProject = true;
    while (inProject)
    {
        Console.Write("\n  [number] View task detail  |  [b] Back to list  |  [r] Reload projects  |  [q] Quit: ");
        string? sub = Console.ReadLine()?.Trim().ToLower();

        switch (sub)
        {
            case "q":
                running   = false;
                inProject = false;
                break;

            case "b":
                inProject = false;
                break;

            case "r":
                projects  = new List<ProjectUiModel>(); // force reload on next iteration
                inProject = false;
                break;

            default:
                if (int.TryParse(sub, out int taskChoice) &&
                    taskChoice >= 1 && taskChoice <= selectedProject.Tasks.Count)
                {
                    ShowTaskDetail(selectedProject.Tasks[taskChoice - 1]);
                }
                else
                {
                    PrintWarning("  Invalid input.");
                }
                break;
        }
    }
}

// ── Farewell ─────────────────────────────────────────────────────────────────
Console.Clear();
PrintColor("\n  Goodbye! Task Manager closed.\n", ConsoleColor.Cyan);

// ═════════════════════════════════════════════════════════════════════════════
// Helper methods
// ═════════════════════════════════════════════════════════════════════════════

/// <summary>Prints the full project list to the console.</summary>
void ShowProjectList(List<ProjectUiModel> projectList)
{
    Console.Clear();
    PrintHeader("═══ TASK MANAGER — PROJECTS ═══");

    for (int i = 0; i < projectList.Count; i++)
    {
        projectList[i].DisplaySummary(i + 1);
    }
}

/// <summary>
/// Prints project details and loads + displays its tasks.
/// Tasks are loaded lazily (only on first access for each project).
/// </summary>
void ShowProjectDetail(ProjectUiModel project, TaskManagerRepository repo)
{
    Console.Clear();
    PrintHeader("═══ PROJECT DETAILS ═══");
    Console.WriteLine();

    // Load tasks if not already loaded
    if (!project.TasksLoaded)
    {
        repo.LoadTasksForProject(project);
    }

    project.DisplayDetails();

    Console.WriteLine();
    PrintColor("  ── Tasks ───────────────────────────────────", ConsoleColor.DarkGray);

    if (project.Tasks.Count == 0)
    {
        Console.WriteLine("  (no tasks in this project)");
        return;
    }

    for (int i = 0; i < project.Tasks.Count; i++)
    {
        // Colour overdue tasks red, completed tasks green
        if (project.Tasks[i].IsCompleted)
            Console.ForegroundColor = ConsoleColor.Green;
        else if (project.Tasks[i].IsOverdue)
            Console.ForegroundColor = ConsoleColor.Red;

        project.Tasks[i].DisplaySummary(i + 1);
        Console.ResetColor();
    }

    Console.WriteLine();
    PrintColor("  Legend: [✓] Completed   [!] Overdue   [ ] Pending", ConsoleColor.DarkGray);
}

/// <summary>Prints full details for a single task.</summary>
void ShowTaskDetail(TaskUiModel task)
{
    Console.WriteLine();
    PrintColor("  ── Task Details ─────────────────────────────", ConsoleColor.DarkGray);
    task.DisplayDetails();
    Pause();
}

/// <summary>Prints a highlighted header line.</summary>
void PrintHeader(string text)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"\n  {text}\n");
    Console.ResetColor();
}

/// <summary>Prints text in the specified colour then resets.</summary>
void PrintColor(string text, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.WriteLine(text);
    Console.ResetColor();
}

/// <summary>Prints a yellow warning message.</summary>
void PrintWarning(string text)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(text);
    Console.ResetColor();
}

/// <summary>Waits for the user to press Enter before continuing.</summary>
void Pause()
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  Press Enter to continue...");
    Console.ResetColor();
    Console.ReadLine();
}
