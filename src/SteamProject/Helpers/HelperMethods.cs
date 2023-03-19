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
        item = Regex.Replace(item, @"&[A-Za-z0-9]+;", "");


        return item;
    }

    public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
    {

        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
}