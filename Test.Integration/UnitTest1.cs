using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace Test.Integration
{
    [TestClass]
    public class UnitTest1
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;



        [TestMethod]
        public void TestMethod1()
        {
            driver = new ChromeDriver();
            baseURL = "http://casb1.cloudapp.net/1028/783ddd8731f9cf945fe1aa1838070723/index.html#categorias";
            verificationErrors = new StringBuilder();

            driver.Navigate().GoToUrl(baseURL);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementExists(By.Id("incluirDocumentoOficial")));

            driver.FindElement(By.Id("incluirDocumentoOficial")).Click();
            driver.FindElement(By.Name("name")).Clear();
            driver.FindElement(By.Name("name")).SendKeys("debentures");
            driver.FindElement(By.Name("abbreviation")).Clear();
            driver.FindElement(By.Name("abbreviation")).SendKeys("cdb");
            driver.FindElement(By.Id("isgovernment")).Click();
            driver.FindElement(By.Id("isguaranteed")).Click();
        }
    }
}





