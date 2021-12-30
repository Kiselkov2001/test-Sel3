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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GibrPlan.Test
{
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

    public static class Helper
    {
        public static IWebDriver driver;
        public static WebDriverWait wait;

        public static string path = "";//@"C:\ProjectsVS\test\Doc\Tutorial\Selenium\training\drivers";

        public static void Scroll(int OffsetX, int OffsetY)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript($"window.scrollBy({OffsetX},{OffsetY})", ""); Thread.Sleep(100);
        }

        public static void mouseOver(IWebElement element)
        {
            string code = "var fireOnThis = arguments[0];"
                        + "var evObj = document.createEvent('MouseEvents');"
                        + "evObj.initEvent( 'mouseover', true, true );"
                        + "fireOnThis.dispatchEvent(evObj);";
            ((IJavaScriptExecutor)driver).ExecuteScript(code, element);
        }

        public static void SelectByIndex(IWebElement element, int idx)
        {
            string code = $"arguments[0].selectedIndex = {idx};"
                        + "arguments[0].dispatchEvent(new Event('change'));";
            ((IJavaScriptExecutor)driver).ExecuteScript(code, element);
        }

        public static void SelectByText(IWebElement element, string txt)
        {
            string code = "var i = 0;" 
                + "for (i = 0; i < arguments[0].options.length; i++) {{ if (arguments[0].options[i].textContent == '"+ txt +"') break; }}"
                + "arguments[0].selectedIndex = i;"
                + "arguments[0].dispatchEvent(new Event('change'));";
            ((IJavaScriptExecutor)driver).ExecuteScript(code, element);
        }

        public static void MoveTo(IWebElement winElem)
        {
            Actions action = new Actions(driver);
            action.MoveToElement(winElem).Build().Perform();
        }

        public static void MoveTo(IWebElement winElem, int OffsetX = 10, int OffsetY = 10)
        {
            Actions action = new Actions(driver);
            action.MoveToElement(winElem, OffsetX, OffsetY, MoveToElementOffsetOrigin.TopLeft).Build().Perform();
        }

        public static void MoveByOffset(int OffsetX, int OffsetY)
        {
            Actions action = new Actions(driver);
            action.MoveByOffset(OffsetX, OffsetY).Build().Perform();
        }

        public static void Click(IWebElement winElem)
        {
            Actions action = new Actions(driver);
            action.Click(winElem).Build().Perform();
        }

        public static void Click()
        {
            Actions action = new Actions(driver);
            action.Click().Build().Perform();
        }

        public static void inner(this IWebElement winElem, string txt, string type = "TextBox")
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

        public static void RunBrowser(xВrowserIdx Index)
        {
            switch (Index)
            {
                case xВrowserIdx.Chrome:
                    {
                        ChromeOptions opt = new ChromeOptions();
                        //opt.AddExcludedArguments("excludeSwitches", "enable-automation");
                        opt.UnhandledPromptBehavior = UnhandledPromptBehavior.Dismiss;

                        driver = string.IsNullOrEmpty(path) ? new ChromeDriver(opt) : new ChromeDriver(path, opt);
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

                        driver = string.IsNullOrEmpty(path) ? new InternetExplorerDriver(opt) : new InternetExplorerDriver(path, opt);
                    }
                    break;

                case xВrowserIdx.Edge:
                    {
                        EdgeOptions opt = new EdgeOptions();
                        opt.AddAdditionalCapability("useAutomationExtension", false);
                        var srv = EdgeDriverService.CreateDefaultService(path, "msedgedriver.exe");
                        driver = string.IsNullOrEmpty(path) ? new EdgeDriver(opt) : new EdgeDriver(srv, opt);

                        driver.Manage().Window.Maximize(); Thread.Sleep(100); //Driver.Manage().Window.FullScreen();
                    }
                    break;

                case xВrowserIdx.Firefox:
                    {
                        FirefoxOptions opt = new FirefoxOptions();
                        //opt.AddAdditionalCapability("MARIONETTE", false) //old method

                        driver = string.IsNullOrEmpty(path) ? new FirefoxDriver(opt) : new FirefoxDriver(path, opt);
                    }
                    break;

                case xВrowserIdx.Opera:
                    driver = new OperaDriver(path);
                    break;

                case xВrowserIdx.Firefox_Nightly:
                    {
                        string bin1 = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                        string bin2 = @"C:\Program Files\Firefox Nightly\firefox.exe";

                        FirefoxOptions opt = new FirefoxOptions();
                        opt.BrowserExecutableLocation = bin2;

                        //FirefoxDriverService srv = FirefoxDriverService.CreateDefaultService(path);
                        //driver = new FirefoxDriver(srv, opt);

                        driver = string.IsNullOrEmpty(path) ? new FirefoxDriver(opt) : new FirefoxDriver(path, opt);
                        break;
                    }
            }

            if (driver != null)
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); // Set implicit wait timeouts to 5 secs
                driver.Manage().Timeouts().AsynchronousJavaScript = new TimeSpan(0, 0, 0, 5);  // Set script timeouts to 5 secs
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(25);

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            }
        }

        public static IWebElement FindElementExt(this IWebDriver driver, By elemName, int nTryCount = 3)
        {
            IWebElement uiTarget = null;
            while (nTryCount-- > 0)
            {
                try
                {
                    uiTarget = driver.FindElement(elemName);
                }
                catch
                {
                }
                if (uiTarget != null)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            return uiTarget;
        }

        public static IWebElement FindElementExt(this IWebElement winElem, By elemName, int nTryCount = 3)
        {
            IWebElement uiTarget = null;
            while (nTryCount-- > 0)
            {
                try
                {
                    uiTarget = winElem.FindElement(elemName);
                }
                catch
                {
                }
                if (uiTarget != null)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            return uiTarget;
        }
    }
}
