using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FakeStorePageObjects
{
    public class CartPage : BasePage
    {
        private string CartUrl => baseUrl + "/koszyk/";

        public IList<IWebElement> CartItems => driver.FindElementsWithWait(By.CssSelector("tr.cart_item"),2);
        public string ItemId => CartItems[0].FindElement(By.CssSelector("a")).GetAttribute("data-product_id");
        public IList<string> ItemIds => CartItems.Select(item => item.FindElement(By.CssSelector("a")).GetAttribute("data-product_id")).ToList();
        public IWebElement CartEmpyMessage => driver.FindElement(By.CssSelector("p[class='cart-empty woocommerce-info']"));
        private IWebElement GoToPaymentsBtn => driver.FindElement(By.CssSelector("a.checkout-button"));
        public IWebElement QuantityInCartFld => CartItems[0].FindElement(By.CssSelector("input[type='number']"));
        public IWebElement UpdateCartBtn => driver.FindElement(By.CssSelector("button[name='update_cart']"));
        private IWebElement CouponCodeFld => driver.FindElement(By.CssSelector("input#coupon_code"));
        private IWebElement ApplyCouponBtn => driver.FindElement(By.CssSelector("button[name=apply_coupon]"));
        public IWebElement CouponAlert => driver.FindElementWithWait(By.CssSelector("div.woocommerce-message"),10);
        public IWebElement CartDiscount => driver.FindElement(By.CssSelector("tr.cart-discount>th"));
        public IWebElement CouponAmountField => driver.FindElement(By.CssSelector("tr.cart-discount>td>span"));
        public IWebElement OrderTotalWithoutTaxElement => driver.FindElement(By.CssSelector(".order-total td strong"));
        public IWebElement ErrorAlert => driver.FindElementWithWait(By.CssSelector("div>ul.woocommerce-error"),10);


        public CartPage(IWebDriver driver) : base(driver) { }

        public CartPage GoToPage()
        {
            driver.Navigate().GoToUrl(CartUrl);
            return this;
        }

        public OrderPage GoToPayments()
        {
            GoToPaymentsBtn.Click();
            WaitForLoadersDisappear();
            return new OrderPage(driver);
        }

        public CartPage UpdateQuantity(int amount)
        {
            QuantityInCartFld.Clear();
            QuantityInCartFld.SendKeys(amount.ToString());
            UpdateCartBtn.Click();
            WaitForLoadersDisappear();
            return this;
        }

        public CartPage AddCoupon(string code)
        {
            CouponCodeFld.SendKeys(code);
            ApplyCouponBtn.Click();
            WaitForLoadersDisappear();
            return this;
        }
    }
}