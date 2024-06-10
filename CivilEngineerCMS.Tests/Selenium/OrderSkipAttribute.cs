using CivilEngineerCMS.Tests.Utilities;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;




namespace CivilEngineerCMS.Tests.Selenium
{


    [TestFixture]
    public class OrderSkipAttribute
    {

        [Test, Order(3), Category("OrderSkipAttribute")]
        public void TestMethod1()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Url = "https://localhost:7255";
            driver.Manage().Window.Maximize();

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
            IWebElement buttonMyManagersProjects = driver.FindElement(By.LinkText("My manager's projects"));
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
        [Test,Order(0), Category("OrderSkipAttribute")]
        public void TestMethod2()
        {
            IWebDriver driver = new FirefoxDriver();
            driver.Url = "https://localhost:7255";
            driver.Manage().Window.Maximize();

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
            IWebElement buttonMyManagersProjects = driver.FindElement(By.LinkText("My manager's projects"));
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

            IWebElement projectTitle = driver.FindElement(By.Id("Description"));
            projectTitle.Clear();
            projectTitle.SendKeys("New Test Project description 1");

            IWebElement dropDownListElement = driver.FindElement(By.XPath(".//*[@id=\"Status\"]/option[3]"));
            dropDownListElement.Click();
            IWebElement dropDownListElement2 = driver.FindElement(By.XPath(".//*[@id=\"ManagerId\"]/option[2]"));
            dropDownListElement2.Click();

            IWebElement buttonCancel = driver.FindElement(By.LinkText("Cancel"));
            js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'})", buttonCancel);
            driver.ExecuteJavaScript("arguments[0].click();", buttonCancel);

            driver.Close();
        }
        [Test, Order(1), Category("OrderSkipAttribute")]
        public void TestMethod3()
        {
            IWebDriver driver = new EdgeDriver();
            driver.Url = "https://localhost:7255";
            driver.Manage().Window.Maximize();

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
            IWebElement buttonMyManagersProjects = driver.FindElement(By.LinkText("My manager's projects"));
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

            IWebElement projectTitle = driver.FindElement(By.Id("Description"));
            projectTitle.Clear();
            projectTitle.SendKeys("New Test Project description 1");

            IWebElement dropDownListElement = driver.FindElement(By.XPath(".//*[@id=\"Status\"]/option[3]"));
            dropDownListElement.Click();
            IWebElement dropDownListElement2 = driver.FindElement(By.XPath(".//*[@id=\"ManagerId\"]/option[2]"));
            dropDownListElement2.Click();

            IWebElement buttonCancel = driver.FindElement(By.LinkText("Cancel"));
            js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'})", buttonCancel);
            driver.ExecuteJavaScript("arguments[0].click();", buttonCancel);

            driver.Close();
        }
        //[Test, Category("OrderSkipAttribute")]
        //public void TestMethod4()
        //{
        //    IWebDriver driver = new InternetExplorerDriver();
        //    driver.Url = "https://localhost:7255";
        //    driver.Manage().Window.Maximize();

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
        //    IWebElement buttonMyManagersProjects = driver.FindElement(By.LinkText("My manager's projects"));
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
