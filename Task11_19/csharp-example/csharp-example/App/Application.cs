using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace csharp_example
{
    public class Application
    {
        private IWebDriver driver;

        private MainPage mainPage;
        private BinPage binPage;

        public Application(xВrowserIdx idx)
        {
            Helper.RunBrowser(idx); driver = Helper.driver;
            mainPage = new MainPage(driver);
            binPage = new BinPage(driver);
        }

        public void Quit()
        {
            if (driver == null) return;
            driver.Quit();
            Helper.driver = null;
        }

        internal void Scenario()
        {
            mainPage.Open();
            mainPage.SelectProducts();

            mainPage.OpenBinPage();
            binPage.DeleteProducts();
        }
    }
}