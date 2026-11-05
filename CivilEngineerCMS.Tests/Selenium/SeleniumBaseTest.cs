using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace CivilEngineerCMS.Tests.Selenium
{
    public abstract class SeleniumBaseTest
    {
        public IWebDriver driver;



        //[OneTimeSetUp]
        [SetUp]
        public void Open()
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:7255";
        }

        //[OneTimeTearDown]
        [TearDown]
        public void Close()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}
