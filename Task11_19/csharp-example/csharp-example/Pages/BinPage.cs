using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_example
{
    internal class BinPage : Page
    {
        public BinPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        internal void DeleteProducts()
        {
            var table = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table.dataTable")));

            int k = 0;
            while (k < 10)
            {
                btnDelete.Click();
                wait.Until(ExpectedConditions.StalenessOf(table));
                table = driver.FindElementExt(By.CssSelector("table.dataTable"), 1); if (table == null) break;
                k++;
            };
        }

        [FindsBy(How = How.CssSelector, Using = "button[name=remove_cart_item]")]
        public IWebElement btnDelete;
    }
}
