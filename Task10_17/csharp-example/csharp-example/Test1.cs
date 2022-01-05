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
            // 1) зайти в админку
            driver.Url = "http://localhost/litecart/admin/"; TimeSpan.FromSeconds(60);
            driver.FindElement(By.Name("username")).SendKeys("admin"); TimeSpan.FromSeconds(60);
            driver.FindElement(By.Name("password")).SendKeys("admin"); TimeSpan.FromSeconds(60);

            driver.FindElement(By.Name("login")).Click(); Thread.Sleep(1000);
            driver.Manage().Window.Maximize(); Thread.Sleep(100);

            //2) открыть каталог, категорию, которая содержит товары(страница http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1)
            //winElem = driver.FindElementExt(By.XPath("//*[@id='app-']//span[text()='Catalog']"));
            //if (winElem != null) winElem.Click(); Thread.Sleep(1000);

            driver.Navigate().GoToUrl("http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1)"); TimeSpan.FromSeconds(60);

            //3) последовательно открывать страницы товаров и проверять, не появляются ли в логе браузера сообщения(любого уровня)
            int j = 0;
            ReadOnlyCollection<IWebElement> winColl = driver.FindElements(By.XPath("//table[@class='dataTable']//a[@title='Edit'][contains(@href,'product_id')]"));

            ReadOnlyCollection<string> arrType = driver.Manage().Logs.AvailableLogTypes; //WebDriver 3.141.0.0 System.NullReferenceException : Ссылка на объект не указывает на экземпляр объекта.

            for (int i = 0; i < winColl.Count; i++)
            {
                winColl[i].Click(); Thread.Sleep(100);

                if (j != driver.Manage().Logs.GetLog("browser").Count) { Console.WriteLine($"Product_id={i+1} : New messages have appeared in browser log"); }
                j = driver.Manage().Logs.GetLog("browser").Count;

                //alter method for WebDriver 3.141.0.0
                //IEnumerable<IDictionary<string, object>> lstLogs = driver.GetBrowserLogs();
                //List<IDictionary<string, object>> lst = new List<IDictionary<string, object>>(); lst.AddRange(lstLogs);

                //if (j != lst.Count) { Console.WriteLine($"Product_id={i} : New messages have appeared in browser log"); }
                //j = lst.Count;

                driver.Navigate().Back(); Thread.Sleep(100);
                winColl = driver.FindElements(By.XPath("//table[@class='dataTable']//a[@title='Edit'][contains(@href,'product_id')]"));
            }

            Thread.Sleep(1000);
        }

        public class CustomExpectedConditions
        {
            public static Func<IWebDriver, string> ThereIsWindowOtherThan(ICollection<string> oldWindows)
            {
                return (driver) =>
                {
                    List<string> handles = new List<string>(); handles.AddRange(driver.WindowHandles);
                    foreach (string s in oldWindows) handles.Remove(s);
                    return handles.Count > 0 ? handles[0] : null;
                };
            }

            public static Func<IWebDriver, IWebElement> ElementExistsIsVisibleIsEnabledNoAttribute(By locator) 
            {
                return (driver) =>
                {
                    IWebElement element = driver.FindElement(locator);
                    if (element.Displayed
                    && element.Enabled
                    && element.GetAttribute("aria-disabled").Equals(null))
                    {
                        return element;
                    }

                    return null;
                };
            }
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
