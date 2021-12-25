using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GibrPlan.Test.Pages
{
    public class ProdPage
    {
        IWebDriver driver => Helper.driver;

        public IWebElement tabGeneral => driver.FindElementExt(By.XPath("//div[@class='tabs']//li/a[text()='General']"));
        public IWebElement tabInfo => driver.FindElementExt(By.XPath("//div[@class='tabs']//li/a[text()='Information']"));
        public IWebElement tabPrices => driver.FindElementExt(By.XPath("//div[@class='tabs']//li/a[text()='Prices']"));

        //General
        public IWebElement enabled => driver.FindElementExt(By.XPath("//label[contains(text(),'Enabled')]/input[@name='status']"));
        public IWebElement name => driver.FindElementExt(By.XPath("//input[@name='name[en]']"));
        public IWebElement code => driver.FindElementExt(By.XPath("//input[@name='code']"));
        public IWebElement gender => driver.FindElementExt(By.XPath("(//*[@id='tab-general']//input[contains(@name,'product_groups')])[3]")); //Unisex
        public IWebElement file => driver.FindElementExt(By.XPath("//*[@id='tab-general']//input[contains(@name,'new_images')]"));
        public IWebElement dat_beg => driver.FindElementExt(By.XPath("//*[@id='tab-general']//input[@name='date_valid_from']"));
        public IWebElement dat_end => driver.FindElementExt(By.XPath("//*[@id='tab-general']//input[@name='date_valid_to']"));


        //Information


        //Prices //currency
        public IWebElement purshprice => driver.FindElementExt(By.XPath("//*[@id='tab-prices']//input[@name='purchase_price']"));
        public IWebElement currency => driver.FindElementExt(By.XPath("//*[@id='tab-prices']//select[@name='purchase_price_currency_code']"));
        public IWebElement price => driver.FindElementExt(By.XPath("//*[@id='tab-prices']//input[@name='prices[USD]']"));


        //Buttons
        public IWebElement btnSave => driver.FindElementExt(By.XPath("//button[@name='save']"));
        public IWebElement btnDelete => driver.FindElementExt(By.XPath("//button[@name='delete']"));
        public IWebElement btnCancel => driver.FindElementExt(By.XPath("//button[@name='cancel']"));

    }
}
