using System.Collections;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace CivilEngineerCMS.Tests.Selenium
{
    [TestFixture]
    public class SeleniumCSharp
    {
        [Test]
        [Author("IZZ", "iliyaz.softuni@gmail.com")]
        public void Test1()
        {
            IWebDriver driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:7255";

            IWebElement buttonLogin = driver.FindElement(By.LinkText("Login"));
            buttonLogin.Click();
            IWebElement emailTextElement = driver.FindElement(By.Id("Email"));
            emailTextElement.SendKeys("employee2@abv.bg");
            IWebElement passwordTextElement = driver.FindElement(By.Id("Password"));
            passwordTextElement.SendKeys("123456User-+");
            IWebElement rememberMeCheckBox = driver.FindElement(By.Id("RememberMe"));
            rememberMeCheckBox.Click();
            IWebElement submitLoginButton = driver.FindElement(By.Id("login-submit"));
            submitLoginButton.Click();
            IWebElement buttonMyManagersProjects = driver.FindElement(By.LinkText("Manager projects"));
            buttonMyManagersProjects.Click();
            IWebElement buttonDetails = driver.FindElement(By.LinkText("Details"));
            buttonDetails.Click();
            IWebElement buttonEdit = driver.FindElement(By.LinkText("Edit"));
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'})", buttonEdit);
            driver.ExecuteJavaScript("arguments[0].click();", buttonEdit);
            Thread.Sleep(1000);
            IWebElement dropDownList = driver.FindElement(By.Id("Status"));
            SelectElement selectElement = new SelectElement(dropDownList);
            selectElement.SelectByIndex(3);
            selectElement.SelectByValue("4");
            selectElement.SelectByText("Finished");

            IWebElement projectTitle = driver.FindElement(By.Id("Name"));
            projectTitle.Clear();
            projectTitle.SendKeys("New Test Project 1");

            IWebElement dropDownListElement = driver.FindElement(By.XPath(".//*[@id=\"Status\"]/option[3]"));
            dropDownListElement.Click();
            IWebElement dropDownListElement2 = driver.FindElement(By.XPath(".//*[@id=\"ManagerId\"]/option[2]"));
            dropDownListElement2.Click();

            IWebElement buttonCancel = driver.FindElement(By.LinkText("Cancel"));
            js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'})", buttonCancel);
            driver.ExecuteJavaScript("arguments[0].click();", buttonCancel);

            driver.Close();
        }

        static IList DataDrivenTesting()
        {
            ArrayList list = new ArrayList();
            list.Add(new string[] { "" });
            return list;
        }

        //[Test]
        //[Author("IZZ", "iliyaz.softuni@gmail.com")]
        //[Description("This is a temporary selenium test for the Edit button in the My manager's projects page.")]
        //public void Test2()
        //{
        //    IWebDriver driver = new FirefoxDriver();
        //    driver.Manage().Window.Maximize();
        //    driver.Url = "https://localhost:7255";

        //    IWebElement buttonLogin = driver.FindElement(By.LinkText("Login"));
        //    buttonLogin.Click();
        //    IWebElement emailTextElement = driver.FindElement(By.Id("Email"));
        //    emailTextElement.SendKeys("employee2@abv.bg");
        //    IWebElement passwordTextElement = driver.FindElement(By.Id("Password"));
        //    passwordTextElement.SendKeys("123456User-+");
        //    IWebElement rememberMeCheckBox = driver.FindElement(By.Id("RememberMe"));
        //    rememberMeCheckBox.Click();
        //    IWebElement submitLoginButton = driver.FindElement(By.Id("login-submit"));
        //    submitLoginButton.Click();
        //    IWebElement buttonMyManagersProjects = driver.FindElement(By.LinkText("Manager projects"));
        //    buttonMyManagersProjects.Click();
        //    IWebElement buttonDetails = driver.FindElement(By.LinkText("Details"));
        //    buttonDetails.Click();
        //    IWebElement buttonEdit = driver.FindElement(By.LinkText("Edit"));
        //    var js = (IJavaScriptExecutor)driver;
        //    js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'})", buttonEdit);
        //    driver.ExecuteJavaScript("arguments[0].click();", buttonEdit);
        //    Thread.Sleep(1000);
        //    IWebElement dropDownList = driver.FindElement(By.Id("Status"));
        //    SelectElement selectElement = new SelectElement(dropDownList);
        //    selectElement.SelectByIndex(3);
        //    selectElement.SelectByValue("4");
        //    selectElement.SelectByText("Finished");

        //    IWebElement projectTitle = driver.FindElement(By.Id("Description"));
        //    projectTitle.Clear();
        //    projectTitle.SendKeys("New Test Project description 1");

        //    IWebElement dropDownListElement = driver.FindElement(By.XPath(".//*[@id=\"Status\"]/option[3]"));
        //    dropDownListElement.Click();
        //    IWebElement dropDownListElement2 = driver.FindElement(By.XPath(".//*[@id=\"ManagerId\"]/option[2]"));
        //    dropDownListElement2.Click();

        //    IWebElement buttonCancel = driver.FindElement(By.LinkText("Cancel"));
        //    js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'})", buttonCancel);
        //    driver.ExecuteJavaScript("arguments[0].click();", buttonCancel);

        //    driver.Quit();
        //}

        //[Test]
        //public void Test3()
        //{
        //    IWebDriver driver = new FirefoxDriver();
        //    driver.Manage().Window.Maximize();
        //    driver.Url = "https://localhost:7255";

        //    IWebElement buttonLogin = driver.FindElement(By.LinkText("Login"));
        //    buttonLogin.Click();
        //    IWebElement emailTextElement = driver.FindElement(By.Id("Email"));
        //    emailTextElement.SendKeys("employee2@abv.bg");
        //    IWebElement passwordTextElement = driver.FindElement(By.Id("Password"));
        //    passwordTextElement.SendKeys("123456User-+");
        //    IWebElement rememberMeCheckBox = driver.FindElement(By.Id("RememberMe"));
        //    rememberMeCheckBox.Click();
        //    IWebElement submitLoginButton = driver.FindElement(By.Id("login-submit"));
        //    submitLoginButton.Click();
        //    IWebElement buttonMyManagersProjects = driver.FindElement(By.LinkText("Manager projects"));
        //    buttonMyManagersProjects.Click();
        //    IWebElement buttonDetails = driver.FindElement(By.LinkText("Details"));
        //    buttonDetails.Click();
        //    IWebElement buttonEdit = driver.FindElement(By.LinkText("Edit"));
        //    var js = (IJavaScriptExecutor)driver;
        //    js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'})", buttonEdit);
        //    driver.ExecuteJavaScript("arguments[0].click();", buttonEdit);
        //    Thread.Sleep(1000);
        //    IWebElement dropDownList = driver.FindElement(By.Id("Status"));
        //    SelectElement selectElement = new SelectElement(dropDownList);
        //    selectElement.SelectByIndex(3);
        //    selectElement.SelectByValue("4");
        //    selectElement.SelectByText("Finished");

        //    IWebElement projectTitle = driver.FindElement(By.Id("Description"));
        //    projectTitle.Clear();
        //    projectTitle.SendKeys("New Test Project description 1");

        //    IWebElement dropDownListElement = driver.FindElement(By.XPath(".//*[@id=\"Status\"]/option[3]"));
        //    dropDownListElement.Click();
        //    IWebElement dropDownListElement2 = driver.FindElement(By.XPath(".//*[@id=\"ManagerId\"]/option[2]"));
        //    dropDownListElement2.Click();

        //    IWebElement buttonCancel = driver.FindElement(By.LinkText("Cancel"));
        //    js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'})", buttonCancel);
        //    driver.ExecuteJavaScript("arguments[0].click();", buttonCancel);

        //    driver.Close();
        //}

    }
}
