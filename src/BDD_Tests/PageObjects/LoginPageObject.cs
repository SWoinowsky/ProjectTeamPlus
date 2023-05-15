using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace BDD_Tests.PageObjects
{
    public class LoginPageObject : PageObject
    {
        public LoginPageObject(IWebDriver webDriver) : base(webDriver)
        {
            _pageName = "Login";
        }

        private IWebElement FindElementSafely(By by)
        {
            try
            {
                return _webDriver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        public IWebElement EmailInput => FindElementSafely(By.Id("Input_Email"));
        public IWebElement PasswordInput => FindElementSafely(By.Id("Input_Password"));
        public IWebElement RememberMeCheck => FindElementSafely(By.Id("Input_RememberMe"));
        public IWebElement SubmitButton => FindElementSafely(By.Id("login-submit"));

        public void EnterEmail(string email)
        {
            if (EmailInput != null)
            {
                EmailInput.Clear();
                EmailInput.SendKeys(email);
            }
            else
            {
                throw new NoSuchElementException("Email input field not found");
            }
        }

        public void EnterPassword(string password)
        {
            if (PasswordInput != null)
            {
                PasswordInput.Clear();
                PasswordInput.SendKeys(password);
            }
            else
            {
                throw new NoSuchElementException("Password input field not found");
            }
        }

        public void SetRememberMe(bool rememberMe)
        {
            if (RememberMeCheck != null)
            {
                if (rememberMe)
                {
                    if (!RememberMeCheck.Selected)
                    {
                        RememberMeCheck.Click();
                    }
                }
                else
                {
                    if (RememberMeCheck.Selected)
                    {
                        RememberMeCheck.Click();
                    }
                }
            }
            else
            {
                throw new NoSuchElementException("Remember me check box not found");
            }
        }

        public void Login()
        {
            if (SubmitButton != null)
            {
                SubmitButton.Click();
            }
            else
            {
                throw new NoSuchElementException("Submit button not found");
            }
        }

        public bool HasLoginErrors()
        {
            ReadOnlyCollection<IWebElement> elements = _webDriver.FindElements(By.CssSelector(".validation-summary-errors"));
            return elements.Count > 0;
        }
    }
}
