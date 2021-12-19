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
        private IWebDriver driver;
        private WebDriverWait wait;

        public static string Path = "";//@"C:\ProjectsVS\test\Doc\Tutorial\Selenium\training\drivers";

        public class duck
        {
            public string name;
            public string priceG;
            public string priceR;
            public int sizeG;
            public int sizeR;
            public string clrG;
            public string clrR;
            public string styleG;
            public string styleR;
            public string weightG;
            public string weightR;
            public string decorG;
            public string decorR;
            public bool sizeComp => sizeR > sizeG;
            public bool clrsComp => isRed(clrR) && isGray(clrG);

            public bool isGray(string clr) 
            {
                Regex rgx = new Regex(@"(\d+)");
                MatchCollection m = rgx.Matches(clr);
                return m[0].Value == m[1].Value && m[1].Value == m[2].Value; 
            }
            public bool isRed(string clr)
            {
                Regex rgx = new Regex(@"(\d+)");
                MatchCollection m = rgx.Matches(clr);
                return m[0].Value != "0" && m[1].Value == "0" && m[2].Value == "0";
            }

        }

        duck duck1 = new duck();
        duck duck2 = new duck();

        public static string clrGray = "rgba(119, 119, 119, 1)";
        public static string clrRed = "rgba(204, 0, 0, 1)";

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
            // Открыть главную страницу, выбрать первый товар в блоке Campaigns
            driver.Url = "http://localhost/litecart/"; TimeSpan.FromSeconds(60);
            driver.Manage().Window.Maximize(); Thread.Sleep(1000);

            IWebElement part = driver.FindElement(By.CssSelector("#box-campaigns")); Scroll(0, part.Location.Y);
            IWebElement item = part.FindElement(By.XPath(".//ul/li"));

            fillduck(duck1, item, 1);

            item.Click(); Thread.Sleep(1000);

            item = driver.FindElement(By.CssSelector("#box-product"));

            fillduck(duck2, item, 2);

            //а) на главной странице и на странице товара совпадает текст названия товара
            Assert.IsTrue(duck1.name == duck2.name, $"Не совпадают названия '{duck1.name}' и '{duck2.name}'");

            //б) на главной странице и на странице товара совпадают цены(обычная и акционная)
            Assert.IsTrue(duck1.priceG == duck2.priceG, $"Не совпадают цены (обычн.) '{duck1.priceG}' и '{duck2.priceG}'");
            Assert.IsTrue(duck1.priceR == duck2.priceR, $"Не совпадают цены (акцион.) '{duck1.priceR}' и '{duck2.priceR}'");

            //в) обычная цена зачёркнутая и серая (можно считать, что "серый" цвет это такой, у которого в RGBa представлении одинаковые значения для каналов R, G и B)
            Assert.IsTrue(duck1.decorG.Contains("line-through"), $"Не совпадает декор цены (обычн.) '{duck1.decorG}'");
            Assert.IsTrue(duck2.decorG.Contains("line-through"), $"Не совпадает декор цены (обычн.) '{duck2.decorG}'"); //line-through solid rgb(119, 119, 119)

            Assert.IsTrue(duck1.clrsComp, $"Не совпадают цвета цен (обычн.) '{duck1.clrG}' и '{clrGray}' / '{duck1.clrR}' и '{clrRed}'");
            Assert.IsTrue(duck2.clrsComp, $"Не совпадают цвета цен (акцион.) '{duck2.clrG}' и '{clrGray}' / '{duck2.clrR}' и '{clrRed}'");

            //г) акционная жирная и красная (можно считать, что "красный" цвет это такой, у которого в RGBa представлении каналы G и B имеют нулевые значения)
            //(цвета надо проверить на каждой странице независимо, при этом цвета на разных страницах могут не совпадать)
            Assert.IsTrue(int.Parse(duck1.weightR) > 400, $"Не совпадает стиль цены (акцион.) '{duck1.weightR}' и '700/900'");
            Assert.IsTrue(int.Parse(duck2.weightR) > 400, $"Не совпадает стиль цены (акцион.) '{duck2.weightR}' и '700/900'");

            //д) акционная цена крупнее, чем обычная(это тоже надо проверить на каждой странице независимо)
            Assert.IsTrue(duck1.sizeComp, $"Не соразмерны цены (акцион.) '{duck1.sizeR}' и (обычн.) '{duck1.sizeG}'");
            Assert.IsTrue(duck2.sizeComp, $"Не соразмерны цены (акцион.) '{duck2.sizeR}' и (обычн.) '{duck2.sizeG}'");

            Thread.Sleep(3000);

        }

        public void fillduck(duck d, IWebElement item, int opt = 1)
        {
            IWebElement winElem;

            d.name = item.FindElement(By.CssSelector(opt == 1 ? ".name" :".title")).GetAttribute("textContent");

            winElem = item.FindElement(By.XPath(".//*[@class='regular-price']"));
            d.priceG = winElem.GetAttribute("textContent");
            d.clrG = winElem.GetCssValue("color");
            d.sizeG = winElem.Size.Height;
            d.styleG = winElem.GetCssValue("font-style");
            d.weightG = winElem.GetCssValue("font-weight"); //400
            d.decorG = winElem.GetCssValue("text-decoration");

            winElem = item.FindElement(By.XPath(".//*[@class='campaign-price']"));
            d.priceR = winElem.GetAttribute("textContent");
            d.clrR = winElem.GetCssValue("color");
            d.sizeR = winElem.Size.Height;
            d.styleR = winElem.GetCssValue("font-style");
            d.weightR = winElem.GetCssValue("font-weight"); //700 //900
            d.decorR = winElem.GetCssValue("text-decoration");
        }

        public void fillduck1(duck d, IWebElement item)
        {
            IWebElement winElem;

            d.name = item.FindElement(By.XPath(".//div[@class='name']")).GetAttribute("textContent");
            d.name = item.FindElement(By.CssSelector(".name")).GetAttribute("textContent");

            winElem = item.FindElement(By.XPath(".//*[@class='regular-price']"));
            d.priceG = winElem.GetAttribute("textContent");
            d.clrG = winElem.GetCssValue("color");
            d.sizeG = winElem.Size.Height;
            d.styleG = winElem.GetCssValue("font-style");
            d.decorG = winElem.GetCssValue("text-decoration");

            winElem = item.FindElement(By.XPath(".//*[@class='campaign-price']"));
            d.priceR = winElem.GetAttribute("textContent");
            d.clrR = winElem.GetCssValue("color");
            d.sizeR = winElem.Size.Height;
            d.styleR = winElem.GetCssValue("font-style");
            d.decorR = winElem.GetCssValue("text-decoration");
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

        public void ClickShot(IWebElement winElem)
        {
            Actions action = new Actions(driver);
            action.Click(winElem);
            //action.Build();
            //action.Perform(); // OpenQA.Selenium.WebDriverTimeoutException : timeout: Timed out receiving message from renderer: 5.000
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
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(25);

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            }
        }

        #endregion Browser
    }
}
