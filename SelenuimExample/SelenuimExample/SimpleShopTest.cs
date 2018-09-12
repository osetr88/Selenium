using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SelenuimExample
{
    [TestFixture]
    class SimpleShopTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        [Test]
        public void LeftMenuTest()
        {
            // login
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            // get list of links
            IWebElement list = driver.FindElement(By.CssSelector("ul#box-apps-menu"));
            List<string> listOfLinks = GetListOfLinks(list, By.CssSelector("li#app->a"));

            // "click" left menu
            foreach (string link in listOfLinks)
            {
                driver.Url = link;
                driver.FindElement(By.CssSelector("h1"));

                // "click" sub left menu if it need
                if (IsElementExist(By.CssSelector("ul.docs")))
                {
                    var subList = driver.FindElement(By.CssSelector("ul.docs"));
                    var subListOfLinks = GetListOfLinks(subList, By.XPath(".//li[contains(@id, 'doc-')]/a"));

                    foreach (string subLink in subListOfLinks)
                    {
                        driver.Url = subLink;
                        driver.FindElement(By.CssSelector("h1"));
                    }
                }
            }
        }

        [Test]
        public void StikersTest()
        {
            driver.Url = "http://localhost/litecart/";

            var stikers = driver.FindElements(By.XPath(".//div[img[contains(@alt, 'Duck')]]"));

            foreach (var stiker in stikers)
            {
                Assert.IsTrue(IsElementSingle(stiker, By.CssSelector("div.sticker")));
            }
        }

        [TearDown]
        public void Finish()
        {
            driver.Quit();
            driver = null;
        }

        List<string> GetListOfLinks(IWebElement list, By locator)
        {
            List<string> result = new List<string>();
            var links = list.FindElements(locator);            

            foreach (var link in links)
            {
                result.Add(link.GetAttribute("href"));
            }

            return result;
        }

        bool IsElementExist(By locator)
        {
            try
            {
                driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException ex)
            {
                return false;
            }
        }

        bool IsElementSingle(IWebElement element, By locator)
        {
            return element.FindElements(locator).Count() == 1;
        }
    }
}