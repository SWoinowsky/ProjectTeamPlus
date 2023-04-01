using System.Text.RegularExpressions;

namespace SteamProject.Helpers;

public class TaskHelperMethods
{
    public static async Task<string[]> HandleFailedTasks(List<Task<string>> tasks, int timeoutMilliseconds = 5000)
    {
        var handledTasks = tasks.Select(task => AddTimeoutAndHandleFailedTask(task, timeoutMilliseconds)).ToList();
        return await Task.WhenAll(handledTasks);
    }

    private static async Task<string> AddTimeoutAndHandleFailedTask(Task<string> task, int timeoutMilliseconds)
    {
        var timeoutTask = Task.Delay(timeoutMilliseconds);
        var completedTask = await Task.WhenAny(task, timeoutTask);

        if (completedTask == timeoutTask)
        {
            return "This article took to long, Check out whats new by clicking the above button"; ;
        }

        if (task.IsFaulted)
        {
            return "There was a problem summarizing news for this game, Check out whats new by clicking the above button"; ;
        }

        return task.Result;
    }
}