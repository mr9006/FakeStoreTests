using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace FakeStorePageObjects
{
    public class MainPage : BasePage
    {
        public MainPage(IWebDriver driver) : base(driver) { }

        public MainPage GoTo()
        {
            driver.Navigate().GoToUrl(baseUrl);
            return this;
        }
    }
}