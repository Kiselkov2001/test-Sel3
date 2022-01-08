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

namespace csharp_example
{
    [TestFixture]
    public class Test1 
    {
        public Application app;

        [SetUp]
        public void TestInit()
        {
        }

        [TearDown]
        public void TestClear()
        {
            app.Quit();
            app = null;
        }

        [Test]
        public void Test01()
        {
            app = new Application(xВrowserIdx.Chrome);
            app.Scenario();
        }

        [Test]
        public void Test02()
        {
            app = new Application(xВrowserIdx.IE); 
            app.Scenario();
        }

        [Test]
        public void Test03()
        {
            app = new Application(xВrowserIdx.Edge);
            app.Scenario();
        }

        [Test]
        public void Test04()
        {
            app = new Application(xВrowserIdx.Firefox);
            app.Scenario();
        }

        [Test]
        public void Test05()
        {
            app = new Application(xВrowserIdx.Firefox_Nightly);
            app.Scenario();
        }

    }
}
