using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Standups_BDD_Tests.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standups_BDD_Tests.PageObjects
{
    /// <summary>
    /// Put things here that are shared or needed by all PageObjects
    /// </summary>
    public abstract class PageObject
    {
        protected readonly IWebDriver _webDriver;

        // set this in derived classes
        protected string _pageName;

        //The default wait time in seconds for WaitUntil
        public int DefaultWaitInSeconds { get; set; } = 5;
        public PageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        // Go to the previously set page name (convenience method for using derived classes)
        public void GoTo()
        {
            if(_pageName != null)
            {
                GoTo(_pageName);
            }
        }

        /// <summary>
        /// Go to a named page.  Set these in Shared/Common.cs
        /// i.e. GoTo("Home") or GoTo("Login")
        /// </summary>
        /// <param name="pageName"></param>
        public void GoTo(string pageName)
        {
            _webDriver.Navigate().GoToUrl(Common.UrlFor(pageName));
        }

        public string GetTitle() => _webDriver.Title;
        public string GetURL() => _webDriver.Url;

        public bool SaveAllCookies()
        {
            ReadOnlyCollection<Cookie> cookies = _webDriver.Manage().Cookies.AllCookies;
            bool success = FileUtils.SerializeCookiesToFile(Common.CookieFile, cookies);
            return success;
        }

        public bool LoadAllCookies()
        {
            List<Cookie> cookies;
            try
            {
                cookies = FileUtils.DeserializeCookiesFromFile(Common.CookieFile);
            }
            catch(Exception ex)
            {
                return false;
            }
            foreach (Cookie cookie in cookies)
            {
                _webDriver.Manage().Cookies.AddCookie(cookie);
            }
            _webDriver.Navigate().Refresh();
            return true;
        }

        public void DeleteCookies()
        {
            _webDriver.Manage().Cookies.DeleteAllCookies();
        }

        /// <summary>
        /// Helper method to wait until the expected result is available on the UI
        /// </summary>
        /// <typeparam name="T">The type of result to retrieve</typeparam>
        /// <param name="getResult">The function to poll the result from the UI</param>
        /// <param name="isResultAccepted">The function to decide if the polled result is accepted</param>
        /// <returns>An accepted result returned from the UI. If the UI does not return an accepted result within the timeout an exception is thrown.</returns>
        private T WaitUntil<T>(Func<T> getResult, Func<T, bool> isResultAccepted) where T : class
        {
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(DefaultWaitInSeconds));
            return wait.Until(driver =>
            {
                var result = getResult();
                if (!isResultAccepted(result))
                    return default;

                return result;
            });
        }
    }
}
