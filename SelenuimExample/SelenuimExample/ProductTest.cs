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
    class ProductTest
    {
        private IWebDriver driver;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void PriceTest()
        {         
            driver.Url = "http://localhost/litecart/";

            var product = driver.FindElement(By.CssSelector("div#box-campaigns"));
            Product pageProduct = new Product(product, By.CssSelector("div.name"));

            Assert.IsTrue(IsGrey(pageProduct.RegularPriceColor));
            Assert.IsTrue(IsLineThought(pageProduct.RegularPriceDecor));
            Assert.IsTrue(IsRed(pageProduct.CampaiganPriceColor));
            Assert.IsTrue(IsBold(pageProduct.CampaiganPriceDecor));
            Assert.IsTrue(IsBiggerFont(pageProduct.RegularPriceFont, pageProduct.CampaiganPriceFont));

            product.FindElement(By.CssSelector("div.name")).Click();

            product = driver.FindElement(By.CssSelector("div#box-product"));
            Product campaiganProduct = new Product(product, By.CssSelector("h1.title"));

            Assert.IsTrue(IsGrey(campaiganProduct.RegularPriceColor));
            Assert.IsTrue(IsLineThought(campaiganProduct.RegularPriceDecor));
            Assert.IsTrue(IsRed(campaiganProduct.CampaiganPriceColor));
            Assert.IsTrue(IsBold(campaiganProduct.CampaiganPriceDecor));
            Assert.IsTrue(IsBiggerFont(campaiganProduct.RegularPriceFont, campaiganProduct.CampaiganPriceFont));

            Assert.AreEqual(pageProduct.Name, campaiganProduct.Name);
            Assert.AreEqual(pageProduct.RegularPrice, campaiganProduct.RegularPrice);
            Assert.AreEqual(pageProduct.CampaiganPrice, campaiganProduct.CampaiganPrice);
        }

        [TearDown]
        public void Finish()
        {
            driver.Quit();
            driver = null;
        }

        bool IsGrey(string rgb)
        {
            rgb = rgb.Replace("rgba(", "");
            rgb = rgb.Replace(")", "");
            rgb = rgb.Replace(" ", "");
            string[] digital = rgb.Split(',');
            if (Convert.ToInt32(digital[0]) == Convert.ToInt32(digital[1]) &&
                Convert.ToInt32(digital[0]) == Convert.ToInt32(digital[2]))
                return true;
            else
                return false;
        }

        bool IsRed(string rgb)
        {
            rgb = rgb.Replace("rgba(", "");
            rgb = rgb.Replace(")", "");
            rgb = rgb.Replace(" ", "");
            string[] digital = rgb.Split(',');
            if (Convert.ToInt32(digital[1]) == 0 &&
                Convert.ToInt32(digital[2]) == 0)
                return true;
            else
                return false;
        }

        bool IsLineThought(string decor)
        {
            if (decor == "line-through")
                return true;
            else
                return false;
        }

        bool IsBold(string decor)
        {
            if (Convert.ToInt32(decor) > 600)
                return true;
            else
                return false;
        }

        bool IsBiggerFont(string regularFont, string campaiganFont)
        {
            regularFont = regularFont.Replace("px", "");
            regularFont = regularFont.Replace(".", ",");

            campaiganFont = campaiganFont.Replace("px", "");
            campaiganFont = campaiganFont.Replace(".", ",");

            if (Convert.ToDouble(campaiganFont) > Convert.ToDouble(regularFont))
                return true;
            else
                return false;
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public string RegularPrice { get; set; }
        public string RegularPriceColor { get; set; }
        public string RegularPriceFont { get; set; }
        public string RegularPriceDecor { get; set; }
        public string CampaiganPrice { get; set; }
        public string CampaiganPriceColor { get; set; }
        public string CampaiganPriceFont { get; set; }
        public string CampaiganPriceDecor { get; set; }

        public Product(IWebElement product, By locator)
        {
            Name = product.FindElement(locator).Text;
            RegularPrice = product.FindElement(By.CssSelector("s.regular-price")).Text;
            RegularPriceColor = product.FindElement(By.CssSelector("s.regular-price")).GetCssValue("color");
            RegularPriceFont = product.FindElement(By.CssSelector("s.regular-price")).GetCssValue("font-size");
            RegularPriceDecor = product.FindElement(By.CssSelector("s.regular-price")).GetCssValue("text-decoration-line");
            CampaiganPrice = product.FindElement(By.CssSelector("strong.campaign-price")).Text;
            CampaiganPriceColor = product.FindElement(By.CssSelector("strong.campaign-price")).GetCssValue("color");
            CampaiganPriceFont = product.FindElement(By.CssSelector("strong.campaign-price")).GetCssValue("font-size");
            CampaiganPriceDecor = product.FindElement(By.CssSelector("strong.campaign-price")).GetCssValue("font-weight");
        }
    }
}
