using FakeStorePageObjects;
using FakeStoreTests.Config;
using FakeStoreTests.TestData;
using Helpers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace FakeStoreTests
{
    public class BaseTest
    {
        protected IWebDriver driver;
        protected Configuration config;
        protected Data testData;


        protected DriverOptions options;
        protected WebDriverWait wait;
        protected IJavaScriptExecutor js;

        [OneTimeSetUp]
        public void SetUpConfig()
        {
            ConfigSetup();
            TestDataSetup();
        }

        [SetUp]
        public void DriverSetup()
        {
            driver = new DriverFactory().Create(config.Browser, config.IsRemote, config.RemoteAddress);

            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            MainPage mainPage = new MainPage(driver);
            mainPage.GoTo().DismissNotice();
        }

        [TearDown]
        public void QuitDriver()
        {
            driver.Close();
            driver.Quit();
        }

        private void ConfigSetup()
        {
            config = new Configuration();
            IConfiguration configurationFile = new ConfigurationBuilder().AddJsonFile(@"Config\configuration.json").Build();
            configurationFile.Bind(config);
        }

        private void TestDataSetup()
        {
            testData = new Data();
            IConfiguration testDataFile = new ConfigurationBuilder().AddJsonFile(@"TestData\testData.json").Build();
            testDataFile.Bind(testData);
        }

    }
}
