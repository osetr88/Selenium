using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace SelenuimExample.PageObject
{
    internal class ProductPage : Page
    {
        private int count = 0;
        //private IWebElement cartCount;

        public ProductPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        public void AddProductToCart()
        {
            if (IsElementExist(By.XPath(".//select[@name='options[Size]']")))
            {
                var el = driver.FindElement(By.XPath(".//select[@name='options[Size]']"));
                var select = new SelectElement(el);

                select.SelectByValue("Small");
            }

            var cartCount = driver.FindElement(By.CssSelector("div#cart span.quantity"));
            count = Convert.ToInt32(cartCount.Text);

            driver.FindElement(By.Name("add_cart_product")).Click();
            count++;
        }

        public void WaitCartAdd()
        {
            var cartCount = driver.FindElement(By.CssSelector("div#cart span.quantity"));
            wait.Until(ExpectedConditions.TextToBePresentInElement(cartCount, count.ToString()));
        }

        public void OpenCartPage()
        {
            driver.FindElement(By.XPath(".//a[.='Checkout »']")).Click();
        }
    }
}
