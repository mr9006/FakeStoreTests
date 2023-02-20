using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    public class DriverFactory
    {
            private IWebDriver driver;
            public IWebDriver Create(string browser, bool isRemote, Uri remoteAddress = null)
            {
                if (isRemote)
                {
                    return GetRemoteDriver(browser, remoteAddress);
                }
                else
                {
                    return GetLocalDriver(browser);
                }
            }

            private IWebDriver GetLocalDriver(string browser)
            {
                switch (browser)
                {
                    case "chrome":
                        driver = new ChromeDriver();
                        break;
                    case "firefox":
                        driver = new FirefoxDriver();
                        break;
                    default:
                        throw new ArgumentException("Provided driver:" + browser + " is not supported. Available: chrome, firefox.");
                }
                return driver;
            }

            private IWebDriver GetRemoteDriver(string browser, Uri remoteAddress = null)
            {
                DriverOptions options;
                switch (browser)
                {
                    case "chrome":
                        options = new ChromeOptions();
                        break;
                    case "firefox":
                        options = new FirefoxOptions();
                        break;
                    default:
                        throw new ArgumentException("Provided driver:" + browser + " is not supported. Available: chrome, firefox.");
                }
                if (remoteAddress != null)
                {
                    driver = new RemoteWebDriver(remoteAddress, options);
                }
                else
                {
                    driver = new RemoteWebDriver(options);
                }
                return driver;
            }
    }
}



