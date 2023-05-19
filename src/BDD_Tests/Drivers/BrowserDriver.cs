using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Tests.Drivers
{
    public class BrowserDriver : IDisposable
    {
        private readonly Lazy<IWebDriver> _currentWebDriverLazy;
        private bool _isDisposed = false;

        public BrowserDriver()
        {
            _currentWebDriverLazy = new Lazy<IWebDriver>(CreateWebDriver);
        }

        public IWebDriver Current => _currentWebDriverLazy.Value;

        // Expose WebDriver instance as a public property
        public IWebDriver WebDriver => Current;

        private IWebDriver CreateWebDriver()
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            firefoxOptions.AcceptInsecureCertificates = true;
            firefoxOptions.BrowserExecutableLocation = "C:\\Program Files\\Firefox Developer Edition\\firefox.exe";
            FirefoxDriver driver = new FirefoxDriver(firefoxOptions);

            return driver;
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            if (_currentWebDriverLazy.IsValueCreated)
            {
                WebDriver.Quit();
            }

            _isDisposed = true;
        }

    }
}