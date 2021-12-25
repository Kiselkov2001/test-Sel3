using GibrPlan.Test.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace GibrPlan.Test
{
    [TestFixture]
    public class Test1 
    {
        private IWebDriver driver => Helper.driver;
        private WebDriverWait wait => Helper.wait;

        private ProdPage prodPage = new ProdPage();

        private class Product
        {
            //general
            public string name = "Pyramid{num}";
            public string code = "1{num}";
            public string enabled = "true";
            public string gender = "true";
            public string file = "Pyramid.jpeg";
            public string dat_beg = "01.01.2021";
            public string dat_end = "01.01.2023";

            //prises
            public string purshprice = "10";
            public string currency = "US Dollars";
            public string price = "12";

            public Product()
            {
                name = name.Replace("{num}", DateTime.Now.ToString("mmss"));
                code = code.Replace("{num}", DateTime.Now.ToString("mmss"));
            }
        }

        private Product prod = new Product();

        [SetUp]
        public void TestInit()
        {
        }

        [TearDown]
        public void TestClear()
        {
            if (driver == null) return;
            driver.Quit();
            Helper.driver = null;
        }

        #region Scenario
        public void Scenario()
        {
            IWebElement winElem;

            // 1) открыть главную страницу
            driver.Url = "http://localhost/litecart/"; TimeSpan.FromSeconds(60);
            driver.Manage().Window.Maximize(); Thread.Sleep(1000);

            for (int i = 0; i < 3; i++)
            {

                // 2) открыть первый товар из списка
                // 2) добавить его в корзину(при этом может случайно добавиться товар, который там уже есть, ничего страшного)
                IWebElement Item, CartContent;

                //ExpectedConditions depricated -> https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras
                Item = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//ul/li[starts-with(@class,'product')]")))[0];
                Item.Click(); 

                CartContent = driver.FindElementExt(By.CssSelector("#cart span.quantity"));
                int j = int.Parse(CartContent.GetAttribute("textContent"));

                var btnCart = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[name=add_cart_product]")));

                NeedSelectOption();

                btnCart.Click(); 

                // 3) подождать, пока счётчик товаров в корзине обновится
                wait.Until(ExpectedConditions.TextToBePresentInElement(CartContent, $"{j+1}"));

                // 4) вернуться на главную страницу, повторить предыдущие шаги ещё два раза, чтобы в общей сложности в корзине было 3 единицы товара
                driver.Navigate().Back();
            }

            // 5) открыть корзину(в правом верхнем углу кликнуть по ссылке Checkout)
            winElem = driver.FindElementExt(By.PartialLinkText("Checkout"));
            winElem.Click();

            // 6) удалить все товары из корзины один за другим, после каждого удаления подождать, пока внизу обновится таблица
            var table = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table.dataTable")));

            int k = 0;
            while (k < 10)
            {
                winElem = driver.FindElement(By.CssSelector("button[name=remove_cart_item]")); //if (winElem == null) break;
                winElem.Click();
                wait.Until(ExpectedConditions.StalenessOf(table));
                table = driver.FindElementExt(By.CssSelector("table.dataTable"), 1); if (table == null) break;
                k++;
            };

            Thread.Sleep(1000);
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


        #endregion Scenario

        [Test]
        public void Test01()
        {
            Helper.RunBrowser(xВrowserIdx.Chrome);
            Scenario();
        }


        [Test]
        public void Test02()
        {
            Helper.RunBrowser(xВrowserIdx.IE);
            Scenario();
        }

        [Test]
        public void Test03()
        {
            Helper.RunBrowser(xВrowserIdx.Edge);
            Scenario();
        }

        [Test]
        public void Test04()
        {
            Helper.RunBrowser(xВrowserIdx.Firefox);
            Scenario();
        }

        [Test]
        public void Test05()
        {
            Helper.RunBrowser(xВrowserIdx.Firefox_Nightly);
            Scenario();
        }

    }
}
