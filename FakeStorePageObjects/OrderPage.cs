using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace FakeStorePageObjects
{
    public class OrderPage : BasePage
    {
        public OrderPage(IWebDriver driver) : base(driver) { } //dziedziczenie drivera po klasie bazowej

        private IWebElement PlaceOrderBtn => driver.FindElement(By.CssSelector("button#place_order"));
        //Card Fields
        private IWebElement CardNumberIframe => driver.FindElement(By.CssSelector("#stripe-card-element iframe"));
        private IWebElement ExpirationDateIframe => driver.FindElement(By.CssSelector("#stripe-exp-element iframe"));
        private IWebElement CvcCodeIframe => driver.FindElement(By.CssSelector("#stripe-cvc-element iframe"));
        //Customer Fields
        private IWebElement NameFld => driver.FindElement(By.CssSelector("input#billing_first_name"));
        private IWebElement SurnameFld => driver.FindElement(By.CssSelector("input#billing_last_name"));
        private IWebElement StreetFld => driver.FindElement(By.CssSelector("input#billing_address_1"));
        private IWebElement PostCodeFld => driver.FindElement(By.CssSelector("input#billing_postcode"));
        private IWebElement CityFld => driver.FindElement(By.CssSelector("#billing_city"));
        private IWebElement PhoneFld => driver.FindElement(By.CssSelector("#billing_phone"));
        private IWebElement EmailFld => driver.FindElement(By.CssSelector("#billing_email"));
        //Login Form
        private IWebElement ShowLoginLink => driver.FindElementWithWait(By.CssSelector("a.showlogin"),10);
        private IWebElement UserLogin => driver.FindElementWithWait(By.CssSelector("form.login>p.form-row>input#username"),10);
        private IWebElement UserPassword => driver.FindElement(By.CssSelector("form.login>p.form-row>span>input#password"));
        private IWebElement SubmitLoginForm => driver.FindElement(By.CssSelector("button.woocommerce-form-login__submit"));
        //CreditCard Fields
        private IWebElement CardNumberFld => driver.FindElement(By.CssSelector(".CardNumberField-input-wrapper input"));
        private IWebElement ExpirationDateFld => driver.FindElement(By.CssSelector("input[name='exp-date']"));
        private IWebElement CvcCodeFld => driver.FindElement(By.CssSelector("input[name='cvc']"));

        private IWebElement TermsChbox => driver.FindElement(By.CssSelector("input#terms"));
        public IWebElement ErrorsList => driver.FindElement(By.CssSelector("ul.woocommerce-error"));
        public IList<IWebElement> ErrorMessagesElements => driver.FindElements(By.CssSelector("ul.woocommerce-error li"));
        public IList<IWebElement> ProductTotalElements => driver.FindElements(By.CssSelector(".product-total bdi"));

        public IWebElement OrderTotalTax => driver.FindElement(By.CssSelector("tr.order-total>td>small>span.woocommerce-Price-amount"));
        By LoginForm => By.CssSelector(".login[style='']");

        public OrderPage FillCreditCardData(string cartNumber, string expDate, string cvcCode)
        {
            driver.SwitchTo().Frame(CardNumberIframe);
            CardNumberFld.SendKeys(cartNumber);
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(ExpirationDateIframe);
            ExpirationDateFld.SendKeys(expDate);
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(CvcCodeIframe);
            CvcCodeFld.SendKeys(cvcCode);
            driver.SwitchTo().DefaultContent();
            return this;
        }

        public OrderPage FillCustomerData(string name, string surname, string street, string postCode, string city, string phone, string email)
        {
            NameFld.SendKeys(name);
            SurnameFld.SendKeys(surname);
            StreetFld.SendKeys(street);
            PostCodeFld.SendKeys(postCode);
            CityFld.SendKeys(city);
            PhoneFld.SendKeys(phone);
            EmailFld.SendKeys(email);
            WaitForLoadersDisappear();
            return this;
        }

        public OrderPage LoginAsRegistredUserData(string login, string password)
        {
            ShowLoginLink.Click();
            WaitForElementAppear(LoginForm);
            UserLogin.SendKeys(login);
            UserPassword.SendKeys(password);
            SubmitLoginForm.Click();
            WaitForLoadersDisappear();
            return this;
        }

        public T PlaceOrder<T>() //metoda generyczna, zwraca typ T, może zwracać różne strony
        {
            PlaceOrderBtn.Click();
            WaitForLoadersDisappear();
            return (T) Activator.CreateInstance(typeof(T), driver); 
        }

        public OrderPage CheckTerms()
        {
            TermsChbox.Click();
            return this;
        }

    }

}