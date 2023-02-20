using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;

namespace FakeStorePageObjects
{
    public class SuccessfulOrderPage : BasePage
    {

        public SuccessfulOrderPage(IWebDriver driver) : base(driver) { }

        public IWebElement EntryHeader => driver.FindElement(By.CssSelector(".entry-header"));
    }
}
