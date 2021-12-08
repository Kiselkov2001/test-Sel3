using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace GibrPlan.Test
{
    [TestFixture]
    public class Test1 
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public static string Path = @"C:\ProjectVS\test\Doc\Tutorial\Selenium\training\drivers";

        //[ClassInitialize]
        //public void start(TestContext context)
        //{
        //    driver = new ChromeDriver();
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //}

        //[ClassCleanup]
        //public void stop()
        //{
        //    driver.Quit();
        //    driver = null;
        //}

        [SetUp]
        public void TestInit()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddExcludedArguments("excludeSwitches", "enable-automation" );

            driver = new ChromeDriver(Path, options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void TestClear()
        {
            driver.Quit();
            driver = null;
        }

        [Test]
        [Obsolete]
        public void Test01() 
        {
            driver.Url = "http://www.google.com/"; TimeSpan.FromSeconds(60);
            driver.FindElement(By.Name("q")).SendKeys("webdriver"); TimeSpan.FromSeconds(240);
            driver.FindElements(By.Name("btnK"))[0].Click(); 
            wait.Until(ExpectedConditions.TitleIs("webdriver - Поиск в Google")); 
        }

        ///// <summary>
        /////Получает или устанавливает контекст теста, в котором предоставляются
        /////сведения о текущем тестовом запуске и обеспечивается его функциональность.
        /////</summary>
        //public TestContext TestContext
        //{
        //    get
        //    {
        //        return testContextInstance;
        //    }
        //    set
        //    {
        //        testContextInstance = value;
        //    }
        //}
        //private TestContext testContextInstance;

    }
}
