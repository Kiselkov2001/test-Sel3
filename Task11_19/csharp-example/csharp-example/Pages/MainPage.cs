using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace csharp_example
{
    internal class MainPage : Page
    {
        public MainPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        internal void Open()
        {
            driver.Url = "http://localhost/litecart";
            driver.Manage().Window.Maximize(); Thread.Sleep(1000);
        }

        internal void SelectProducts()
        {
            ProdPage prodPage = new ProdPage(driver);

            for (int i = 0; i < 3; i++)
            {
                var Item = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//ul/li[starts-with(@class,'product')]")))[0];
                
                OpenProdPage(Item);
                prodPage.SelectProduct();

                driver.Navigate().Back();
            }
        }

        internal void OpenProdPage(IWebElement item)
        {
            item.Click();
        }

        internal void OpenBinPage()
        {
            // 5) открыть корзину(в правом верхнем углу кликнуть по ссылке Checkout)
            var winElem = driver.FindElementExt(By.PartialLinkText("Checkout"));
            winElem.Click();
        }
    }
}
