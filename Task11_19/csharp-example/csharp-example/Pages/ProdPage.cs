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
    internal class ProdPage : Page
    {
        public ProdPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        internal void SelectProduct()
        {
            int j = int.Parse(CartContent.GetAttribute("textContent"));

            NeedSelectOption();

            btnCart.Click();

            wait.Until(ExpectedConditions.TextToBePresentInElement(CartContent, $"{j + 1}"));
        }

        private bool NeedSelectOption()
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(100);
            var select = driver.FindElementExt(By.XPath("//select[contains(@name,'options')]"), 1);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(5000);

            if (!(select != null && select.Displayed)) return false;

            SelectElement sel = new SelectElement(select);
            sel.SelectByIndex(1);
            return true;
        }

        [FindsBy(How = How.CssSelector, Using = "button[name=add_cart_product]")]
        internal IWebElement btnCart;

        [FindsBy(How = How.CssSelector, Using = "#cart span.quantity")]
        internal IWebElement CartContent;

    }
}
