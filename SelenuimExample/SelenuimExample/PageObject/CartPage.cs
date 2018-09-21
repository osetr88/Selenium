using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace SelenuimExample.PageObject
{
    internal class CartPage : Page
    {
        public CartPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        public int GetCartTableRowsCount()
        {
            var rows = driver.FindElements(By.CssSelector(".dataTable td.item"));
            return rows.Count();
        }

        public void RemoveProductFromCart()
        {
            if (IsElementExist(By.CssSelector("ul.shortcuts li")))
                driver.FindElement(By.CssSelector("ul.shortcuts li")).Click();

            driver.FindElement(By.Name("remove_cart_item")).Click();
        }

        public void WaitCartRemove()
        {
            var deleteRow = driver.FindElements(By.CssSelector(".dataTable td.item"));
            wait.Until(ExpectedConditions.StalenessOf(deleteRow[deleteRow.Count - 1]));
        }
    }
}
