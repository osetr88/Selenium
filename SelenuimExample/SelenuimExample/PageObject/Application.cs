using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelenuimExample.PageObject
{
    public class Application
    {
        private IWebDriver driver;

        private MainPage mainPage;
        private ProductPage productPage;
        private CartPage cartPage;

        public Application()
        {
            driver = new ChromeDriver();
            mainPage = new MainPage(driver);
            productPage = new ProductPage(driver);
            cartPage = new CartPage(driver);
        }

        public void AddProductsToCart(int count)
        {
            mainPage.Open();
            for (int i = 0; i < count; i++)
            {
                mainPage.OpenProductPage();
                productPage.AddProductToCart();
                productPage.WaitCartAdd();
                mainPage.Open();
            }
        }

        public void RemoveProductsFromCart()
        {
            mainPage.OpenCartPage();
            var count = cartPage.GetCartTableRowsCount();
            for (int i = 0; i < count; i++)
            {
                cartPage.RemoveProductFromCart();
                cartPage.WaitCartRemove();
            }
        }

        public void Quit()
        {
            driver.Quit();
        }
    }
}
