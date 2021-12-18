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
using System.Threading;

namespace GibrPlan.Test
{
    [TestFixture]
    public class Test1 
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public static string Path = @"C:\ProjectsVS\test\Doc\Tutorial\Selenium\training\drivers";


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
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries"; TimeSpan.FromSeconds(60);
            driver.FindElement(By.Name("username")).SendKeys("admin"); TimeSpan.FromSeconds(60);
            driver.FindElement(By.Name("password")).SendKeys("admin"); TimeSpan.FromSeconds(60);

            driver.FindElement(By.Name("login")).Click(); Thread.Sleep(5000);
            driver.Manage().Window.Maximize(); Thread.Sleep(100);

            IWebElement winElem;

            int colName = 5;
            int colZone = 6;
            ReadOnlyCollection<IWebElement> arrName;
            ReadOnlyCollection<IWebElement> arrZone;
            ReadOnlyCollection<IWebElement> arrZoneCountry;

            arrName = driver.FindElements(By.XPath($"//form[@name='countries_form']/table//tr[@class='row']/td[{colName}]/a"));
            arrZone = driver.FindElements(By.XPath($"//form[@name='countries_form']/table//tr[@class='row']/td[{colZone}]"));

            //а) проверяет, что страны расположены в алфавитном порядке
            for (int i = 0; i < arrName.Count - 1; i++)
            {
                Scroll(0, 30);
                string txt1 = arrName[i + 0].GetAttribute("textContent");
                string txt2 = arrName[i + 1].GetAttribute("textContent");
                Assert.IsTrue(txt1.CompareTo(txt2) <= 0, $"Нарушен алфавитный порядок '{txt1}' > '{txt2}'");
            }

            //б) для тех стран, у которых количество зон отлично от нуля -- открывает страницу этой страны и там проверяет, что геозоны расположены в алфавитном порядке
            for (int i = 0; i < arrName.Count; i++)
            {
                if (arrZone[i].GetAttribute("textContent") != "0")
                {
                    Scroll(0, arrZone[i].Location.Y); Thread.Sleep(1000);
                    arrName[i].Click(); Thread.Sleep(1000);

                    winElem = driver.FindElement(By.XPath($"//table[@id='table-zones']")); Scroll(0, winElem.Location.Y); Thread.Sleep(1000);
                    arrZoneCountry = driver.FindElements(By.XPath($"//table[@id='table-zones']//tr[not(@class)]/td[3]"));

                    for (int j = 0; j < arrZoneCountry.Count - 1; j++)
                    {
                        Scroll(0, 20);
                        string txt1 = arrZoneCountry[j + 0].GetAttribute("textContent");
                        string txt2 = arrZoneCountry[j + 1].GetAttribute("textContent"); if (string.IsNullOrEmpty(txt2)) continue;
                        Assert.IsTrue(txt1.CompareTo(txt2) <= 0, $"Нарушен алфавитный порядок '{txt1}' > '{txt2}'");
                    }

                    driver.Navigate().Back(); Thread.Sleep(1000);
                    arrName = driver.FindElements(By.XPath($"//form[@name='countries_form']/table//tr[@class='row']/td[{colName}]/a"));
                    arrZone = driver.FindElements(By.XPath($"//form[@name='countries_form']/table//tr[@class='row']/td[{colZone}]"));
                }
            }


            Thread.Sleep(3000);

        }

        public void Scroll(int OffsetX, int OffsetY)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript($"window.scrollBy({OffsetX},{OffsetY})", ""); Thread.Sleep(100);
        }

        public void mouseOver(IWebElement element)
        {
            String code = "var fireOnThis = arguments[0];"
                        + "var evObj = document.createEvent('MouseEvents');"
                        + "evObj.initEvent( 'mouseover', true, true );"
                        + "fireOnThis.dispatchEvent(evObj);";
            ((IJavaScriptExecutor)driver).ExecuteScript(code, element);
        }

        public void MoveTo(IWebElement winElem)
        {
            Actions action = new Actions(driver);
            //action.MoveToElement(winElem).Build().Perform();

            action.MoveByOffset(winElem.Location.X, winElem.Location.Y).Perform();
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
