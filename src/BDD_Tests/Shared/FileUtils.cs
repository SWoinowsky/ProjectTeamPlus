using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Standups_BDD_Tests.Shared
{
    public class FileUtils
    {
        // If filePath doesn't exist it will be created.  If it exists it will be overwritten
        public static bool SerializeCookiesToFile(string filePath, ReadOnlyCollection<Cookie> cookies)
        {
            // Simple storage strategy here. Store cookie name and value on consecutive lines of the file
            // That way we don't have to worry about encoding into an established format such as JSON
            // Cookies are plain text
            List<string> vals = new List<string>();
            foreach (Cookie cookie in cookies)
            {
                vals.Add(cookie.Name);
                vals.Add(cookie.Value);
            }
            try
            {
                File.WriteAllLines(filePath, vals);
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public static List<Cookie> DeserializeCookiesFromFile(string filePath)
        {
            List<Cookie> cookies = new List<Cookie>();
            string[] lines = File.ReadAllLines(filePath);       // throws an exception if it fails, can test for that
            int numCookies = lines.Length / 2;
            for(int i = 0; i < numCookies; ++i)
            {
                string name = lines[2 * i];
                string value = lines[2 * i + 1];
                cookies.Add(new Cookie(name, value));
            }
            return cookies;
        }

    }
}
