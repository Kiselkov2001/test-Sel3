using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GibrPlan.Test.Pages
{
    public class RegPage
    {
        IWebDriver driver => Helper.driver;

        public IWebElement firstname => driver.FindElementExt(By.XPath("//*[@id='create-account']//table//input[@name='firstname']"));
        public IWebElement lastname => driver.FindElementExt(By.XPath("//*[@id='create-account']//table//input[@name='lastname']"));
        public IWebElement city => driver.FindElementExt(By.XPath("//*[@id='create-account']//table//input[@name='city']"));
        public IWebElement address1 => driver.FindElementExt(By.XPath("//*[@id='create-account']//table//input[@name='address1']"));
        public IWebElement postcode => driver.FindElementExt(By.XPath("//*[@id='create-account']//table//input[@name='postcode']"));
        public IWebElement country => driver.FindElementExt(By.XPath("//*[@class='select2-selection__rendered']"));
        public IWebElement countrySelect => driver.FindElementExt(By.CssSelector("select[name=country_code]"));
        public IWebElement countrySearch => driver.FindElementExt(By.XPath("//*[@class='select2-search__field']"));


        public IWebElement email => driver.FindElementExt(By.XPath("//*[@id='create-account']//table//input[@name='email']"));
        public IWebElement phone => driver.FindElementExt(By.XPath("//*[@id='create-account']//table//input[@name='phone']"));
        public IWebElement password => driver.FindElementExt(By.XPath("//*[@id='create-account']//table//input[@name='password']"));
        public IWebElement confirm => driver.FindElementExt(By.XPath("//*[@id='create-account']//table//input[@name='confirmed_password']"));

        public IWebElement btnCreate => driver.FindElementExt(By.XPath("//*[@id='create-account']//table//button[@name='create_account']"));

    }
}
