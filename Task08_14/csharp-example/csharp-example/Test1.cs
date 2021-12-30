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

            // 1) зайти в админку
            driver.Url = "http://localhost/litecart/admin/"; TimeSpan.FromSeconds(60);
            driver.FindElement(By.Name("username")).SendKeys("admin"); TimeSpan.FromSeconds(60);
            driver.FindElement(By.Name("password")).SendKeys("admin"); TimeSpan.FromSeconds(60);

            driver.FindElement(By.Name("login")).Click(); Thread.Sleep(5000);
            driver.Manage().Window.Maximize(); Thread.Sleep(1000);

            //2) открыть пункт меню Countries(или страницу http://localhost/litecart/admin/?app=countries&doc=countries)
            winElem = driver.FindElementExt(By.XPath("//*[@id='app-']//span[text()='Countries']"));
            if (winElem != null) winElem.Click(); Thread.Sleep(1000);

            //driver.Navigate().GoToUrl("http://localhost/litecart/admin/?app=countries&doc=countries)"); TimeSpan.FromSeconds(60);

            //3) открыть на редактирование какую-нибудь страну или начать создание новой
            winElem = driver.FindElement(By.XPath("//*[@id='content']//table//td/a[text()='Afghanistan']"));
            winElem.Click(); Thread.Sleep(1000);

            //4) возле некоторых полей есть ссылки с иконкой в виде квадратика со стрелкой --они ведут на внешние страницы и открываются в новом окне, именно это и нужно проверить.
            ReadOnlyCollection<IWebElement> winColl = driver.FindElements(By.CssSelector("a[target=_blank]"));

            for (int i = 0; i < winColl.Count; i++)
            {
                string mainWindow = driver.CurrentWindowHandle;
                ICollection<string> oldWindows = driver.WindowHandles;

                winColl[i].Click(); Thread.Sleep(5000);

                string newWindow = wait.Until(CustomExpectedConditions.ThereIsWindowOtherThan(oldWindows));
                driver.SwitchTo().Window(newWindow); Thread.Sleep(1000);
                driver.Close();

                driver.SwitchTo().Window(mainWindow);
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
