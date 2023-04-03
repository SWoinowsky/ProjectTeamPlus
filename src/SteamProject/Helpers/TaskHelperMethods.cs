using System.Text.RegularExpressions;

namespace SteamProject.Helpers;

public class TaskHelperMethods
{
    //This little method right here is meant to help handle Tasks that take to long or fail for whatever reason '
    //as well as having a timeout methods for the ones that take to long
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