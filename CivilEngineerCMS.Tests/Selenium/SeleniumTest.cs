using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace CivilEngineerCMS.Tests.Selenium
{
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium;

    [TestFixture]
    public class SeleniumTest : SeleniumBaseTest
    {
        [Test, Category("Login testing1")]
        public void TestMethodSelenium1()
        {
            IWebElement buttonLogin = driver.FindElement(By.LinkText("Login"));
            buttonLogin.Click();
            IWebElement emailTextElement = driver.FindElement(By.Id("Email"));
            emailTextElement.SendKeys("client1@abv.bg");
            IWebElement passwordTextElement = driver.FindElement(By.Id("Password"));
            passwordTextElement.SendKeys("123456User-+");
            IWebElement rememberMeCheckBox = driver.FindElement(By.Id("RememberMe"));
            rememberMeCheckBox.Click();
            IWebElement submitLoginButton = driver.FindElement(By.Id("login-submit"));
            submitLoginButton.Click();
            //driver.Quit();
        }

        [Test, Category("Login testing2")]
        public void TestMethodSelenium2()
        {
            IWebElement buttonLogin = driver.FindElement(By.LinkText("Login"));
            buttonLogin.Click();
            IWebElement emailTextElement = driver.FindElement(By.Id("Email"));
            emailTextElement.SendKeys("client2@abv.bg");
            IWebElement passwordTextElement = driver.FindElement(By.Id("Password"));
            passwordTextElement.SendKeys("123456User-+");
            IWebElement rememberMeCheckBox = driver.FindElement(By.Id("RememberMe"));
            rememberMeCheckBox.Click();
            IWebElement submitLoginButton = driver.FindElement(By.Id("login-submit"));
            submitLoginButton.Click();
            //driver.Quit();
        }
        [Test, Category("Drop down list")]
        public void TestMethodSelenium3()
        {
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
            //dropDownList.Click();
            IWebElement dropDownListElement = driver.FindElement(By.XPath(".//*[@id=\"Status\"]/option[3]"));
            dropDownListElement.Click();
            IWebElement dropDownListElement2 = driver.FindElement(By.XPath(".//*[@id=\"ManagerId\"]/option[2]"));
            dropDownListElement2.Click();
            IWebElement buttonSave = driver.FindElement(By.XPath("//input[@type='submit']"));
            js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'})", buttonSave);
            driver.ExecuteJavaScript("arguments[0].click();", buttonSave);
        }
    }
}
