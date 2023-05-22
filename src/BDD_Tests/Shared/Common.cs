using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Tests.Shared
{
    // Sitewide definitions and useful methods
    public class Common
    {
        public const string BaseUrl = "http://localhost:5105";     // copied from launchSettings.json


        // File to store browser cookies in
        public const string CookieFile = "..\\..\\..\\..\\..\\..\\..\\StandupsCookies.txt";

        // Page names that everyone should use
        // A handy way to look these up
        public static readonly Dictionary<string, string> Paths = new()
        {
            { "Home" , "/" },
            { "Login", "/Identity/Account/Login" },
            { "Compete", "/Compete" },
            { "Friends", "/Home/Friends" },
            { "Dashboard", "/Dashboard"},
            { "Library","/Library/Index" },
            { "External Logins", "/Identity/Account/Manage/ExternalLogins" },
            { "Profile", "/Identity/Account/Manage" },
            { "Register", "/Identity/Account/Register" },
            { "Speedrun", "/Compete/Speedrun" },
            { "Create Speedrun", "/Compete/CreateSpeedrun" },
            { "Create Competition", "/Compete/CreateCompetition" },
            { "Competition", "/Compete/Competition" },
            { "CompetitionDetails", "/Compete/Details/{0}" }
        };

        public static string PathFor(string pathName) => Paths[pathName];

        public static string UrlFor(string pathName, params object[] args)
        {
            if (!Paths.ContainsKey(pathName))
            {
                throw new KeyNotFoundException($"The given key '{pathName}' was not present in the dictionary.");
            }

            return BaseUrl + string.Format(Paths[pathName], args);
        }
    }
}