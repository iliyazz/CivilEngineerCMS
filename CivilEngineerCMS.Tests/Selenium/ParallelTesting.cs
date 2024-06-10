using CivilEngineerCMS.Tests.Utilities;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

//[assembly: LevelOfParallelism(3)]
//[assembly: Parallelizable(ParallelScope.Children)]
////[assembly: Parallelizable(ParallelScope.Fixtures)]


namespace CivilEngineerCMS.Tests.Selenium
{


    [TestFixture]
    public class ParallelTesting
    {
        IWebDriver driverFirefox;


        [Test, Category("UAT Testing"), Category("Module")]
        public void ParallelTestSelenium1()
        {
            var driver = new BrowserUtility().Init(driverFirefox);

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
        [Test, Category("UAT Testing"), Category("Module")]
        public void ParallelTestSelenium2()
        {
            var driver = new BrowserUtility().Init(driverFirefox);

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
        [Test, Category("UAT Testing"), Category("Module")]
        public void ParallelTestSelenium3()
        {
            var driver = new BrowserUtility().Init(driverFirefox);

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
        [Test, Category("UAT Testing"), Category("Module")]
        public void ParallelTestSelenium4()
        {
            var driver = new BrowserUtility().Init(driverFirefox);

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
        [Test, Category("UAT Testing"), Category("Module")]
        public void ParallelTestSelenium5()
        {
            var driver = new BrowserUtility().Init(driverFirefox);

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
        [Test, Category("UAT Testing"), Category("Module")]
        public void ParallelTestSelenium6()
        {
            var driver = new BrowserUtility().Init(driverFirefox);

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
        [Test, Category("UAT Testing"), Category("Module")]
        public void ParallelTestSelenium7()
        {
            var driver = new BrowserUtility().Init(driverFirefox);

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
        [Test, Category("UAT Testing"), Category("Module")]
        public void ParallelTestSelenium8()
        {
            var driver = new BrowserUtility().Init(driverFirefox);

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
        [Test, Category("UAT Testing"), Category("Module")]
        public void ParallelTestSelenium9()
        {
            var driver = new BrowserUtility().Init(driverFirefox);

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
        [Test, Category("UAT Testing"), Category("Module")]
        public void ParallelTestSelenium10()
        {
            var driver = new BrowserUtility().Init(driverFirefox);

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
    }
}
