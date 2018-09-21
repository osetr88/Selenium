using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SelenuimExample.PageObject;

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
            //ChromeOptions options = new ChromeOptions();
            //options.SetLoggingPreference(LogType.Browser, LogLevel.All);
            //driver = new ChromeDriver(options);
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

            var stikers = driver.FindElements(By.XPath("li.product"));

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

        [Test]
        public void UserRegistrationTest()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Url = "http://localhost/litecart/en/";
            driver.FindElement(By.XPath(".//a[.='New customers click here']")).Click();

            // user registration
            driver.FindElement(By.Name("firstname")).SendKeys("Alex");
            driver.FindElement(By.Name("lastname")).SendKeys("Ovechkin");
            driver.FindElement(By.Name("address1")).SendKeys("Elm Street");
            driver.FindElement(By.Name("postcode")).SendKeys("75448");
            driver.FindElement(By.Name("city")).SendKeys("Washington");

            var country = new SelectElement(driver.FindElement(By.Name("country_code")));
            country.SelectByValue("US");

            IWebElement zone = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("select[name=zone_code]")));
            SelectElement selectZone = new SelectElement(zone);
            selectZone.SelectByValue("WA");

            var email = GetMail(7);
            driver.FindElement(By.Name("email")).SendKeys(email);
            driver.FindElement(By.Name("phone")).SendKeys("88005553535");
            driver.FindElement(By.Name("password")).SendKeys("123qwe");
            driver.FindElement(By.Name("confirmed_password")).SendKeys("123qwe");
            driver.FindElement(By.Name("create_account")).Click();

            // user logout
            driver.FindElement(By.XPath(".//a[.='Logout']")).Click();

            // user login
            driver.FindElement(By.Name("email")).SendKeys(email);
            driver.FindElement(By.Name("password")).SendKeys("123qwe");
            driver.FindElement(By.Name("login")).Click();
        }

        [Test]
        public void AddProductTest()
        {
            // login
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            // go to Catalog page
            driver.FindElement(By.XPath(".//span[.='Catalog']")).Click();
            driver.FindElement(By.XPath(".//a[.=' Add New Product']")).Click();

            // General
            driver.FindElement(By.XPath(".//label[.=' Enabled']")).Click();
            driver.FindElement(By.XPath(".//input[@name='name[en]']")).SendKeys("Super Duck");
            driver.FindElement(By.CssSelector("input[name=code]")).SendKeys("RD047");
            driver.FindElement(By.XPath(".//input[@data-name='Rubber Ducks']")).Click();
            driver.FindElement(By.XPath(".//input[@data-name='Subcategory']")).Click();

            var defaultCategory = new SelectElement(driver.FindElement(By.Name("default_category_id")));
            defaultCategory.SelectByText("Rubber Ducks");

            driver.FindElement(By.XPath(".//input[@name='product_groups[]' and @value='1-3']")).Click();
            driver.FindElement(By.Name("quantity")).SendKeys("20");

            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var path = Path.GetDirectoryName(location);
            string filePath = path.Replace("bin\\Debug","images\\SuperDuck.jpg");
            driver.FindElement(By.XPath(".//input[@name='new_images[]']")).SendKeys(filePath);

            driver.FindElement(By.Name("date_valid_from")).SendKeys("19.09.2018");
            driver.FindElement(By.Name("date_valid_to")).SendKeys("19.09.2019");

            // Information
            driver.FindElement(By.XPath(".//a[.='Information']")).Click(); 

            var manufacturer = new SelectElement(driver.FindElement(By.Name("manufacturer_id")));
            manufacturer.SelectByValue("1");

            driver.FindElement(By.Name("keywords")).SendKeys("Duck");
            driver.FindElement(By.Name("short_description[en]")).SendKeys("Super Duck");
            driver.FindElement(By.Name("description[en]")).SendKeys("Really Super Duck");
            driver.FindElement(By.Name("head_title[en]")).SendKeys("Super Duck");
            driver.FindElement(By.Name("meta_description[en]")).SendKeys("Meta description");

            // Prices
            driver.FindElement(By.XPath(".//a[.='Prices']")).Click();

            driver.FindElement(By.Name("purchase_price")).SendKeys("40");

            var currency = new SelectElement(driver.FindElement(By.Name("purchase_price_currency_code")));
            currency.SelectByValue("USD");

            driver.FindElement(By.Name("prices[USD]")).SendKeys("40");
            driver.FindElement(By.Name("prices[EUR]")).SendKeys("35");

            // Save new product
            driver.FindElement(By.Name("save")).Click();

            // Check
            driver.Url = "http://localhost/litecart/admin/?app=catalog&doc=catalog";
            driver.FindElement(By.XPath(".//a[.='Rubber Ducks']")).Click();

            Assert.IsTrue(IsElementExist(By.XPath(".//a[.='Super Duck']")));
        }

        [Test]
        public void CartTest()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // login
            driver.Url = "http://localhost/litecart/en/";

            // add products to cart
            for (int i = 0; i < 3; i++)
            {
                driver.FindElement(By.CssSelector("div#box-most-popular li")).Click();
                if (IsElementExist(By.XPath(".//select[@name='options[Size]']")))
                {
                    var el = driver.FindElement(By.XPath(".//select[@name='options[Size]']"));
                    var select = new SelectElement(el);

                    select.SelectByValue("Small");
                }

                var cartCount = driver.FindElement(By.CssSelector("div#cart span.quantity"));
                int count = Convert.ToInt32(cartCount.Text);

                driver.FindElement(By.Name("add_cart_product")).Click();
                count++;

                wait.Until(ExpectedConditions.TextToBePresentInElement(cartCount, count.ToString()));
                driver.Url = "http://localhost/litecart/en/";
            }

            driver.FindElement(By.XPath(".//a[.='Checkout »']")).Click();
            var rows = driver.FindElements(By.CssSelector(".dataTable td.item"));

            for (int i = 0; i < rows.Count(); i++)
            {
                if (IsElementExist(By.CssSelector("ul.shortcuts li")))
                    driver.FindElement(By.CssSelector("ul.shortcuts li")).Click();

                driver.FindElement(By.Name("remove_cart_item")).Click();

                var deleteRow = driver.FindElements(By.CssSelector(".dataTable td.item"));
                wait.Until(ExpectedConditions.StalenessOf(deleteRow[deleteRow.Count - 1]));
            }

        }

        [Test]
        public void PageObjectTest()
        {
            Application app = new Application();
            app.AddProductsToCart(3);
            app.RemoveProductsFromCart();
            app.Quit();
        }

        [Test]
        public void WindowsHandleTest()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // login
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            // go to countries page
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";

            // add new country
            driver.FindElement(By.XPath(".//a[.=' Add New Country']")).Click();

            string originalWindow = driver.CurrentWindowHandle;
            var oldWindows = driver.WindowHandles.ToList();

            var links = driver.FindElements(By.CssSelector("i.fa-external-link"));

            foreach (var link in links)
            {
                link.Click();

                var newWindows = driver.WindowHandles.ToList();
                string window = newWindows.Except(oldWindows).First();

                driver.SwitchTo().Window(window);
                driver.Close();
                driver.SwitchTo().Window(originalWindow);
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("i.fa-external-link")));
            }
        }

        [Test]
        public void LogTest()
        {
            // login
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            // go to products page
            driver.Url = "http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1";

            var links = GetListOfLinks(By.XPath(".//a[contains(text(), 'Duck')]"));

            foreach (string link in links)
            {
                driver.Navigate().GoToUrl(link);
                foreach (LogEntry l in driver.Manage().Logs.GetLog("browser"))
                {
                    Console.WriteLine(l);
                }
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

        List<string> GetListOfLinks(By locator)
        {
            List<string> result = new List<string>();
            var links = driver.FindElements(locator);

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

        string GetMail(int length)
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder(length - 1);
            for (int i = 0; i < length; i++)
            {
                var letter = (char) random.Next(97, 122);
                sb.Append(letter);
            }
            return sb + "@mail.ru";
        }

    }
}
