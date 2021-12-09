using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
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
            ChromeOptions options = new ChromeOptions();
            options.AddExcludedArguments("excludeSwitches", "enable-automation" );

            //driver = new ChromeDriver(Path, options);
            driver = new ChromeDriver(options);
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
            driver.Url = "http://localhost/litecart/admin/"; TimeSpan.FromSeconds(60);
            driver.FindElement(By.Name("username")).SendKeys("admin"); TimeSpan.FromSeconds(60);
            driver.FindElement(By.Name("password")).SendKeys("admin"); TimeSpan.FromSeconds(60);

            driver.FindElement(By.Name("login")).Click(); Thread.Sleep(5000);
            wait.Until(ExpectedConditions.TitleIs("My Store")); 
        }


    }
}
