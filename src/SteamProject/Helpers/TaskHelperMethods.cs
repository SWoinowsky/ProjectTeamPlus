using System.Text.RegularExpressions;

namespace SteamProject.Helpers;

public class TaskHelperMethods
{
    public static async Task<string[]> HandleFailedTasks(List<Task<string>> tasks)
    {
        var handledTasks = tasks.Select(task => task.ContinueWith(t =>
        {
            if (t.IsCanceled || t.IsFaulted)
            {
                return "Error: Task failed to complete";
            }
            else
            {
                return t.Result;
            }
        })).ToList();

        return await Task.WhenAll(handledTasks);
    }
}