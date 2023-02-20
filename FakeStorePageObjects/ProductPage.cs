using OpenQA.Selenium;
using Helpers;

namespace FakeStorePageObjects
{
    public class ProductPage : BasePage
    {
        private string ProductUrl => baseUrl + "/product";

        private IWebElement AddToCartBtn => driver.FindElementWithWait(By.CssSelector("button.single_add_to_cart_button"),2);
        private IWebElement GoToCartOnAlertBtn => driver.FindElementWithWait(By.CssSelector("div[class='woocommerce-message'] .wc-forward"),2);
        private IWebElement QuantityFld => driver.FindElementWithWait(By.CssSelector("input[type ='number']"),2);

        public ProductPage(IWebDriver driver) : base(driver){ }

        public ProductPage GoToPage(string productSlug)
        {
            driver.Navigate().GoToUrl(ProductUrl + productSlug);
            return this;
        }

        public ProductPage AddToCart(int quantity = 1)
        {
            if (quantity <= 0 )
            {
                QuantityFld.Clear();
                QuantityFld.SendKeys(quantity.ToString());
                AddToCartBtn.Click();
            }
            else if (quantity != 1)
            {
                QuantityFld.Clear();
                QuantityFld.SendKeys(quantity.ToString());
                AddToCartBtn.Click();
            }
            else
            {
                AddToCartBtn.Click();
            }
            return this;
        }

        public CartPage GoToCart()
        {
            GoToCartOnAlertBtn.Click();
            return new CartPage(driver);
        }

        public bool IsQuantityFieldRangeUnderflowPresent()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return (bool)js.ExecuteScript("return arguments[0].validity.rangeUnderflow", QuantityFld);
        }
    }
}