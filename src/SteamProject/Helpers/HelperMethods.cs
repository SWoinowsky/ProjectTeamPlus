using System.Text.RegularExpressions;

namespace SteamProject.Helpers;

public static class HelperMethods
{
    public static string StripJunkFromString(string? str)
    {
        if (str == null)
        {
            return null;
        }

        //Remove Html tags from string ex: <a>
        var item = Regex.Replace(str, @"<[^>]+>*", "");

        //Remove braket tags ex: [a]
        item = Regex.Replace(item, @"\[[^\]]+\]*", "");

        //Remove hyperlinks ex: wwww, https
        item = Regex.Replace(item, @"http[^\s]+", "");
        item = Regex.Replace(item, @"www[^\s]+", "");

        //Remove whatever is left that looks out of place
        item = Regex.Replace(item, @"&#[^\s]+", "");
        item = Regex.Replace(item, @"\{STEAM_CLAN_IMAGE\}/[A-Za-z0-9]+/[A-Za-z0-9]+\.[A-Za-z]+", "");
        item = Regex.Replace(item, @"[A-Za-z]+&[A-Za-z0-9]+;s", "");

        // Updated pattern to remove entities starting with & and ending with ;
        item = Regex.Replace(item, @"&[#]?[A-Za-z0-9]*;", "");

        return item;
    }

    public static string ConvertImageToBase64(byte[] imageBytes)
    {
        return Convert.ToBase64String(imageBytes);
    }



    public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
    {

        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }

    public static string FixEmailUrl(string url)
    {
        return url.Replace("&amp;", "&");
    }

    public static string FixResetUrl(string html)
    {
        string newHtml = html.Replace("Please reset your password by <a href='", "").Replace("'>clicking here</a>.", "");
        return newHtml;
    }
    public static List<List<T>> SplitListIntoChunks<T>(List<T> items, int chunkSize)
    {
        // If items is null or chunkSize is less than or equal to 0, return an empty list
        if (items == null || chunkSize <= 0)
        {
            return new List<List<T>>();
        }

        List<List<T>> list = new List<List<T>>();

        for (int i = 0; i < items.Count; i += chunkSize)
        {
            List<T> chunk = items.Skip(i).Take(chunkSize).ToList();
            list.Add(chunk);
        }

        return list;
    }


}