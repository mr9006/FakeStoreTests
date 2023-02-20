using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace FakeStorePageObjects
{
    public abstract class BasePage //abstract jest po to żeby nie można było stworzyć obiektu tej kalsy w innych klasach POM
    {
        protected IWebDriver driver;
        protected readonly string baseUrl = "https://fakestore.testelka.pl";
        private IWebElement DismissNoticeLink => driver.FindElement(By.CssSelector(".woocommerce-store-notice__dismiss-link"));

        protected BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        protected void WaitForLoadersDisappear()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                wait.Until(d => driver.FindElements(By.CssSelector(".blockUI")).Count == 0);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Loaders didn't disappear in 10 seconds.");
                throw;
            }
        }

        protected void WaitForElementAppear(By by)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            try
            {
                wait.Until(d => driver.FindElements(by).Count == 1);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Elements located by " + by + " didn't disappear in 5 seconds.");
                throw;
            }
        }

        public void DismissNotice()
        {
            DismissNoticeLink.Click();
        }
    }
}
