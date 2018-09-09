using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SelenuimExample
{
    [TestFixture]
    class DifferentDrivers
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Start()
        {
            
        }

        [Test]
        public void FireFoxLogin()
        {
            driver = new FirefoxDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
        }

        [Test]
        public void ESRLogin()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.UseLegacyImplementation = true;
            driver = new FirefoxDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
        }

        [Test]
        public void NightlyLogin()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.BrowserExecutableLocation = @"C:\Program Files\Firefox Nightly\firefox.exe";
            driver = new FirefoxDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
        }

        [Test]
        public void IELogin()
        {
            driver = new InternetExplorerDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
        }

        [TearDown]
        public void Finish()
        {
            driver.Quit();
            driver = null;
        }
    }
}
