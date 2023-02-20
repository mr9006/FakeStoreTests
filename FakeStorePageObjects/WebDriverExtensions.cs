using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FakeStorePageObjects
{
    public static class WebDriverExtensions //klasa rozszeżająca szukanie elementów o waita 
    {
        public static IWebElement FindElementWithWait(this ISearchContext context, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>((IWebDriver)context);
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
                return wait.Until(c => c.FindElement(by));
            }
            return context.FindElement(by);

        }

        public static ReadOnlyCollection<IWebElement> FindElementsWithWait(this ISearchContext context, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>((IWebDriver)context);
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
                return wait.Until(c => (c.FindElements(by).Count > 0) ? c.FindElements(by) : null);
            }
            return context.FindElements(by);
        }
    }
}

