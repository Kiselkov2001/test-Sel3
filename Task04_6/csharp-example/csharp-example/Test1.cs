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
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;

namespace GibrPlan.Test
{
    [TestFixture]
    public class Test1 
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public static string Path = "";//@"C:\ProjectsVS\test\Doc\Tutorial\Selenium\training\drivers";


        [SetUp]
        public void TestInit()
        {
        }

        [TearDown]
        public void TestClear()
        {
            if (driver == null) return;
            driver.Quit();
            driver = null;
        }

        #region Scenario
        public void Scenario()
        {
            // 1) входит в панель администратора http://localhost/litecart/admin
            driver.Url = "http://localhost/litecart/admin/"; TimeSpan.FromSeconds(60);
            driver.FindElement(By.Name("username")).SendKeys("admin"); TimeSpan.FromSeconds(60);
            driver.FindElement(By.Name("password")).SendKeys("admin"); TimeSpan.FromSeconds(60);

            driver.FindElement(By.Name("login")).Click(); Thread.Sleep(5000);
            wait.Until(ExpectedConditions.TitleIs("My Store"));

            //example using css
            //winTree = driver.FindElement(By.CssSelector("ul#box-apps-menu"));
            //winItems = driver.FindElements(By.CssSelector("ul#box-apps-menu>li"));
            //for (int i = 0; i < winItems.Count; i++)
            //{
            //    MoveTo(i);
            //    if (winItems[i].Displayed)
            //    {
            //        winItems[i].Click(); Thread.Sleep(100);
            //        winItems = driver.FindElements(By.CssSelector("ul#box-apps-menu>li"));
            //    }
            //}

            // 2) прокликивает последовательно все пункты меню слева, включая вложенные пункты
            IWebElement winElem;
            ReadOnlyCollection<IWebElement> winItems;

            winItems = driver.FindElements(By.XPath("//ul[@id='box-apps-menu']/li"));
            int cntMenu = winItems.Count;

            for (int i = 0; i < cntMenu; i++)
            {
                winElem = driver.FindElement(By.XPath($"//ul[@id='box-apps-menu']/li[{i}+1]"));
                //MoveTo(winElem);
                if (winElem.Displayed)
                {
                    winElem.Click(); Thread.Sleep(100);
                    // 3) для каждой страницы проверяет наличие заголовка(то есть элемента с тегом h1)
                    winElem = driver.FindElement(By.TagName("h1"));

                    winItems = driver.FindElements(By.XPath($"//ul[@id='box-apps-menu']/li[{i}+1]/ul/li"));
                    int cntMenuSub = winItems.Count;

                    for (int j = 0; j < cntMenuSub; j++)
                    {
                        //MoveTo(winElem);
                        winElem = driver.FindElement(By.XPath($"//ul[@id='box-apps-menu']/li[{i}+1]/ul/li[{j}+1]"));
                        winElem.Click(); Thread.Sleep(100);
                        // 3) для каждой страницы проверяет наличие заголовка(то есть элемента с тегом h1)
                        winElem = driver.FindElement(By.TagName("h1"));
                    }
                }
            }

            // прокликивает последовательно все пункты меню слева, не включая вложенные пункты, передвижением курсора //Не работает!

            //for (int i = 0; i < driver.FindElements(By.XPath("//ul[@id='box-apps-menu']/li")).Count; i++)
            //{
            //    winElem = driver.FindElement(By.XPath($"//ul[@id='box-apps-menu']/li[{i}+1]"));
            //    MoveTo(winElem);
            //}
        }

        public void MoveTo(IWebElement winElem)
        {
            Actions action = new Actions(driver);
            action.MoveToElement(winElem).Build().Perform();
        }

        #endregion Scenario

        [Test]
        [Obsolete]
        public void Test01()
        {
            RunBrowser(xВrowserIdx.Chrome);
            Scenario();
        }


        [Test]
        [Obsolete]
        public void Test02()
        {
            RunBrowser(xВrowserIdx.IE);
            Scenario();
        }

        [Test]
        [Obsolete]
        public void Test03()
        {
            RunBrowser(xВrowserIdx.Edge);
            Scenario();
        }

        [Test]
        [Obsolete]
        public void Test04()
        {
            RunBrowser(xВrowserIdx.Firefox);
            Scenario();
        }

        [Test]
        [Obsolete]
        public void Test05()
        {
            RunBrowser(xВrowserIdx.Firefox_Nightly);
            Scenario();
        }

        #region Browser
        public enum xВrowserIdx
        {
            None = 0,
            Chrome = 1,
            IE = 2,
            Edge = 3,
            Firefox = 4,
            Opera = 5,
            Firefox_Nightly = 6
        }

        public void RunBrowser(xВrowserIdx Index)
        {
            switch (Index)
            {
                case xВrowserIdx.Chrome:
                    {
                        ChromeOptions opt = new ChromeOptions();
                        //opt.AddExcludedArguments("excludeSwitches", "enable-automation");
                        opt.UnhandledPromptBehavior = UnhandledPromptBehavior.Dismiss;

                        driver = string.IsNullOrEmpty(Path) ? new ChromeDriver(opt) : new ChromeDriver(Path, opt);
                    }
                    break;

                case xВrowserIdx.IE:
                    {
                        InternetExplorerOptions opt = new InternetExplorerOptions();
                        //opt.IgnoreZoomLevel = true;
                        opt.UnhandledPromptBehavior = UnhandledPromptBehavior.Dismiss;
                        //opt.AddAdditionalCapability("unexpectedAlertBehaviour", "dismiss"); //except. ignore
                        opt.AddAdditionalCapability("IGNORE_ZOOM_SETTING", "false");
                        opt.AddAdditionalCapability("INTRODUCE_FLAKENESS_BY_IGNORING_SECURITY_DOMAINS", "false");

                        driver = string.IsNullOrEmpty(Path) ? new InternetExplorerDriver(opt) : new InternetExplorerDriver(Path, opt);
                    }
                    break;

                case xВrowserIdx.Edge:
                    {
                        EdgeOptions opt = new EdgeOptions();
                        opt.AddAdditionalCapability("useAutomationExtension", false);
                        var srv = EdgeDriverService.CreateDefaultService(Path, "msedgedriver.exe");
                        driver = string.IsNullOrEmpty(Path) ? new EdgeDriver(opt) : new EdgeDriver(srv, opt);

                        driver.Manage().Window.Maximize(); Thread.Sleep(100); //Driver.Manage().Window.FullScreen();
                    }
                    break;

                case xВrowserIdx.Firefox:
                    {
                        FirefoxOptions opt = new FirefoxOptions();
                        //opt.AddAdditionalCapability("MARIONETTE", false) //old method

                        driver = string.IsNullOrEmpty(Path) ? new FirefoxDriver(opt) : new FirefoxDriver(Path, opt);
                    }
                    break;

                case xВrowserIdx.Opera:
                    driver = new OperaDriver(Path);
                    break;

                case xВrowserIdx.Firefox_Nightly:
                    {
                        string bin1 = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                        string bin2 = @"C:\Program Files\Firefox Nightly\firefox.exe";

                        FirefoxOptions opt = new FirefoxOptions();
                        opt.BrowserExecutableLocation = bin2;

                        //FirefoxDriverService srv = FirefoxDriverService.CreateDefaultService(Path);
                        //driver = new FirefoxDriver(srv, opt);

                        driver = string.IsNullOrEmpty(Path) ? new FirefoxDriver(opt) : new FirefoxDriver(Path, opt);
                        break;
                    }
            }

            if (driver != null)
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); // Set implicit wait timeouts to 5 secs
                driver.Manage().Timeouts().AsynchronousJavaScript = new TimeSpan(0, 0, 0, 5);  // Set script timeouts to 5 secs
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            }
        }

        #endregion Browser
    }
}
