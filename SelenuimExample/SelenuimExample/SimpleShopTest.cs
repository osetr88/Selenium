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

        [Test]
        public void CountryTest()
        {
            // login
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            // go to country page
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";

            // get list of countries
            var countries = driver.FindElements(By.XPath(".//tr[@class=\"row\"]/td[5]/a")).ToList();
            var sortedCountries = countries.OrderBy(a => a.Text).ToList();

            Assert.IsTrue(IsSorted(countries, sortedCountries, "text"));

            // get rows of table
            var tableRows = driver.FindElements(By.XPath(".//tr[@class=\"row\"]"));
            List<string> zones = new List<string>();

            foreach (var row in tableRows)
            {
                string timeZoneCount = row.FindElement(By.XPath("./td[6]")).Text;
                if (timeZoneCount != "0")
                {
                    var link = row.FindElement(By.XPath("./td[5]/a"))
                        .GetAttribute("href");
                    zones.Add(link);
                }
            }

            foreach (string zone in zones)
            {
                driver.Url = zone;
                var timeZones = driver.FindElements(By.XPath(".//tr/td[3]/input")).ToList();
                var sortedTimeZones = timeZones.OrderBy(a => a.GetAttribute("textContent")).ToList();

                Assert.IsTrue(IsSorted(timeZones, sortedTimeZones, "textContent"));
            }

        }

        [Test]
        public void GeoZoneTest()
        {
            // login
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            // go to country page
            driver.Url = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";

            var table = driver.FindElement(By.CssSelector("table.dataTable"));
            var countries = GetListOfLinks(table, By.XPath(".//tr[@class=\"row\"]/td[3]/a"));

            foreach (string county in countries)
            {
                driver.Url = county;
                var geoZones = driver.FindElements(By.XPath(".//tr/td[3]/select")).ToList();
                var sortedGeoZones = geoZones.OrderBy(a => a.Text).ToList();

                Assert.IsTrue(IsSorted(geoZones, sortedGeoZones, "text"));
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

        bool IsSorted(List<IWebElement> list, List<IWebElement> sortedList, string attribute)
        {
            bool isSorted = true;
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].GetAttribute(attribute) != sortedList[i].GetAttribute(attribute))
                    isSorted = false;
            }

            return isSorted;
        }
    }
}