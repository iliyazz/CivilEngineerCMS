using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace CivilEngineerCMS.Tests.Utilities
{
    public class BrowserUtility
    {
        public IWebDriver Init(IWebDriver driver)
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:7255";
            return driver;
        }
    }
}
