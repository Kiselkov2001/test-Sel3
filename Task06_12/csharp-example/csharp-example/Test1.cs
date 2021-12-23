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
            public string name = "Pyramid";
            public string code = "12345";
            public string enabled = "true";
            public string gender = "true";
            public string file = "Pyramid.jpeg";
            public string dat_beg = "01.01.2021";
            public string dat_end = "01.01.2023";

            //prises
            public string purshprice = "10";
            public string currency = "US Dollars";
            public string price = "12";
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

            // Сделайте сценарий для добавления нового товара (продукта) в учебном приложении litecart (в админке).
            driver.Url = "http://localhost/litecart/admin/"; TimeSpan.FromSeconds(60);
            driver.FindElement(By.Name("username")).SendKeys("admin"); TimeSpan.FromSeconds(60);
            driver.FindElement(By.Name("password")).SendKeys("admin"); TimeSpan.FromSeconds(60);

            driver.FindElement(By.Name("login")).Click(); Thread.Sleep(5000);
            driver.Manage().Window.Maximize(); Thread.Sleep(1000);

            //Для добавления товара нужно открыть меню Catalog, в правом верхнем углу нажать кнопку "Add New Product", заполнить поля с информацией о товаре и сохранить.
            winElem = driver.FindElementExt(By.XPath("//*[@id='box-apps-menu']//span[text()='Catalog']"));
            winElem.Click(); Thread.Sleep(1000);

            winElem = driver.FindElementExt(By.XPath("//*[@id='content']//*[contains(text(),'Add New Product')]"));
            winElem.Click(); Thread.Sleep(1000);

            //Достаточно заполнить только информацию на вкладках General, Information и Prices. Скидки (Campains) на вкладке Prices можно не добавлять.
            //Переключение между вкладками происходит не мгновенно, поэтому после переключения можно сделать небольшую паузу (о том, как делать более правильные ожидания, будет рассказано в следующих занятиях).
            //Картинку с изображением товара нужно уложить в репозиторий вместе с кодом.При этом указывать в коде полный абсолютный путь к файлу плохо, на другой машине работать не будет. Надо средствами языка программирования преобразовать относительный путь в абсолютный.
            fillProduct(prodPage, prod);
            if (prodPage.btnSave != null) prodPage.btnSave.Click(); Thread.Sleep(1000);

            //После сохранения товара нужно убедиться, что он появился в каталоге (в админке). Клиентскую часть магазина можно не проверять.
            winElem = driver.FindElementExt(By.XPath($"//table[@class='dataTable']//a[text()='{prod.name}']"));
            Assert.NotNull(winElem, $"в каталоге нет товара '{prod.name}'");

            Thread.Sleep(3000);
        }

        private void fillProduct(ProdPage prodPage, Product prod)
        {
            if (prodPage.tabGeneral != null) { prodPage.tabGeneral.Click(); Thread.Sleep(1000); }

            inner(prodPage.enabled, prod.enabled, "CheckBox");
            inner(prodPage.name, prod.name);
            inner(prodPage.code, prod.code);
            inner(prodPage.gender, prod.gender, "CheckBox");
            inner(prodPage.file, AppNames.GetTestDataPath() + $"\\Files\\{prod.file}", "InputFile"); 
            inner(prodPage.dat_beg, prod.dat_beg);
            inner(prodPage.dat_end, prod.dat_end);

            if (prodPage.tabInfo != null) { prodPage.tabInfo.Click(); Thread.Sleep(1000); }
            //TODO

            if (prodPage.tabPrices != null) { prodPage.tabPrices.Click(); Thread.Sleep(1000); }
            inner(prodPage.purshprice, prod.purshprice);
            inner(prodPage.currency, prod.currency, "PopupList");
            inner(prodPage.price, prod.price);

        }

        public void inner(IWebElement winElem, string txt, string type = "TextBox")
        {
            if (winElem == null) return;
            switch (type)
            {
                case "TextBox":
                    winElem.Click(); Thread.Sleep(100); winElem.Clear(); winElem.SendKeys(Keys.Home + txt);
                    break;

                case "InputFile":
                    winElem.SendKeys(txt);
                    break;

                case "CheckBox":
                    if (txt == "true" && !winElem.Selected) winElem.Click();
                    if (txt == "false" && winElem.Selected) winElem.Click();
                    break;

                case "PopupList":
                    SelectElement sel = new SelectElement(winElem);
                    sel.SelectByText(txt);

                    //Helper.SelectByText(winElem, txt);
                    break;
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
