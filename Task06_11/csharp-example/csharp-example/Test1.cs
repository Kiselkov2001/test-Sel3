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

        private RegPage regPage = new RegPage();

        private class User
        {
            public string firstname = "Alexey";
            public string lastname = "Kiselkov";
            public string address1 = "Buffallo";
            public string city = "Buffallo";
            public string postcode = "54321";
            public string phone = "8008005050";
            public string email = "Kiselkov{num}@yandex.ru";
            public string country = "United States";
            public string password = "alexus";
            public string confirm = "alexus";

            public User()
            {
                email = email.Replace("{num}", DateTime.Now.ToString("mmss"));
            }
        }

        private User user = new User();

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

            // Открыть главную страницу, выбрать первый товар в блоке Campaigns
            driver.Url = "http://localhost/litecart/"; TimeSpan.FromSeconds(60);
            driver.Manage().Window.Maximize(); Thread.Sleep(1000);

            // 1) регистрация новой учётной записи с достаточно уникальным адресом электронной почты (чтобы не конфликтовало с ранее созданными пользователями, в том числе при предыдущих запусках того же самого сценария),
            winElem = driver.FindElement(By.LinkText("New customers click here"));
            winElem.Click(); Thread.Sleep(1000);

            fillReg(regPage, user);
            if (regPage.btnCreate != null) regPage.btnCreate.Click(); Thread.Sleep(1000);

            //2) выход (logout), потому что после успешной регистрации автоматически происходит вход,
            winElem = driver.FindElementExt(By.LinkText("Logout"));
            winElem.Click(); Thread.Sleep(1000);

            //3) повторный вход в только что созданную учётную запись,
            winElem = driver.FindElementExt(By.XPath("//*[@id='box-account-login']//input[@name='email']"));
            inner(winElem, user.email);
            winElem = driver.FindElementExt(By.XPath("//*[@id='box-account-login']//input[@name='password']"));
            inner(winElem, user.password);
            winElem = driver.FindElementExt(By.XPath("//button[@name='login']"));
            winElem.Click(); Thread.Sleep(3000);

            //4) и ещё раз выход.
            winElem = driver.FindElementExt(By.LinkText("Logout"));
            winElem.Click(); Thread.Sleep(1000);

            Thread.Sleep(3000);
        }

        private void fillReg(RegPage regPage, User user)
        {
            inner(regPage.firstname, user.firstname);
            inner(regPage.lastname, user.lastname);
            inner(regPage.address1, user.address1);
            inner(regPage.city, user.city);
            inner(regPage.postcode, user.postcode);
            inner(regPage.phone, user.phone);
            inner(regPage.email, user.email);
            inner(regPage.country, user.country, "PopupList" ); 
            inner(regPage.password, user.password);
            inner(regPage.confirm, user.confirm);
        }

        public void inner(IWebElement winElem, string txt, string type = "TextBox")
        {
            if (winElem == null) return;
            switch (type)
            {
                case "TextBox":
                    winElem.Click(); Thread.Sleep(100); winElem.Clear(); winElem.SendKeys(Keys.Home + txt);
                    break;
                case "PopupList":
                    //SelectElement sel = new SelectElement(winElem);
                    //sel.SelectByText(txt);

                    //Helper.SelectByText(winElem, txt);

                    winElem.Click(); Thread.Sleep(100); //regPage.countryButton
                    winElem = regPage.countrySearch;
                    winElem.Clear(); Thread.Sleep(100);
                    winElem.SendKeys(txt + Keys.Return); Thread.Sleep(100);

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
