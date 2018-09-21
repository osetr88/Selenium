using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace SelenuimExample.PageObject
{
    internal class MainPage : Page
    {
        public MainPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        internal void Open()
        {
            driver.Url = "http://localhost/litecart/en/";
        }

        public void OpenProductPage()
        {
            driver.FindElement(By.CssSelector("div#box-most-popular li")).Click();
        }

        public void OpenCartPage()
        {
            driver.FindElement(By.XPath(".//a[.='Checkout »']")).Click();
        }
    }
}
