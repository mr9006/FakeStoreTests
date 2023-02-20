using FakeStorePageObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FakeStoreTests
{
    public class OrderTests : BaseTest
    { 

        string firstProductPage => testData.Products[0].Url;
        string secondProductPage => testData.Products[1].Url;

        int firstProductPrice => testData.Products[0].Price;
        int secondProducPrice => testData.Products[1].Price;

        string firstProductPriceText => testData.Products[0].PriceText;
        string secondProducPriceText => testData.Products[1].PriceText;

        [Test]
        public void ConfirmOrderWithEmptyCustomerData_CustomerFieldValidationMessageShouldBeVisible()
        {
            ProductPage productPage = new ProductPage(driver);
            OrderPage orderPage = productPage
                .GoToPage(firstProductPage)
                .AddToCart()
                .GoToCart()
                .GoToPayments()
                .FillCreditCardData(testData.Card.Number, testData.Card.ExpirationDate, testData.Card.Cvc)
                .PlaceOrder<OrderPage>();

            IList<string> errorMessages = orderPage.ErrorMessagesElements.Select(element => element.Text).ToList();
            IList<string> expectedErrorMessages = new List<string> {
                testData.ValidationMessages.Name, 
                testData.ValidationMessages.LastName,
                testData.ValidationMessages.Address,
                testData.ValidationMessages.Postcode,
                testData.ValidationMessages.City,
                testData.ValidationMessages.Phone,
                testData.ValidationMessages.Email,
                testData.ValidationMessages.Terms
            };

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() => _ = orderPage.ErrorsList, "Error list was not found. There was no validation error.");
                Assert.AreEqual(expectedErrorMessages.OrderBy(message => message), errorMessages.OrderBy(message => message));
            });
        }

        [Test]
        public void OrderProduct_PricesAreCorrect()
        {
            ProductPage productPage = new ProductPage(driver);
            OrderPage orderPage = productPage
                .GoToPage(firstProductPage)
                .AddToCart()
                .GoToCart()
                .GoToPayments();

            float tax = CalculateTax(firstProductPrice);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(firstProductPriceText, orderPage.ProductTotalElements[0].Text, "Price is not correct");
                Assert.AreEqual(tax, float.Parse((orderPage.OrderTotalTax.Text).Remove(orderPage.OrderTotalTax.Text.Length - 3)), "Tax is not correct");
            });
        }

        [Test]
        public void OrderTwoProduct_PricesAreCorrect()
        {
            ProductPage productPage = new ProductPage(driver);
            OrderPage orderPage = productPage
                .GoToPage(firstProductPage)
                .AddToCart(2)
                .GoToPage(secondProductPage)
                .AddToCart(3)
                .GoToCart()
                .GoToPayments();

            float tax = CalculateTax((firstProductPrice * 2) + (secondProducPrice * 3));
            Assert.Multiple(() =>
            {
                Assert.AreEqual(FormatNumber(firstProductPrice * 2), orderPage.ProductTotalElements[0].Text, "Price is not correct");
                Assert.AreEqual(FormatNumber(secondProducPrice * 3), orderPage.ProductTotalElements[1].Text, "Price is not correct");
                Assert.AreEqual(tax, float.Parse((orderPage.OrderTotalTax.Text).Remove(orderPage.OrderTotalTax.Text.Length - 3)), "Tax is not correct");
            });
        }

        [Test]
        public void QuantityUpdatedInCart_PricesAreCorrect()
        {
            ProductPage productPage = new ProductPage(driver);
            OrderPage orderPage = productPage
                .GoToPage(firstProductPage)
                .AddToCart()
                .GoToCart()
                .UpdateQuantity(2)
                .GoToPayments();

            float tax = CalculateTax(firstProductPrice*2);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(FormatNumber(firstProductPrice * 2), orderPage.ProductTotalElements[0].Text, "Price is not correct");
                Assert.AreEqual(tax, float.Parse((orderPage.OrderTotalTax.Text).Remove(orderPage.OrderTotalTax.Text.Length - 3)), "Tax is not correct");
            });
        }


        [Test]
        public void BuyAsANewUser()
        {
            TestData.Customer customer = testData.Customer;
            TestData.Card card = testData.Card;

            ProductPage productPage = new ProductPage(driver);
            SuccessfulOrderPage successfulOrderPage = productPage.GoToPage(firstProductPage).AddToCart().GoToCart().GoToPayments()
                        .FillCreditCardData(card.Number, card.ExpirationDate, card.Cvc)
                        .FillCustomerData(customer.Name, customer.LastName, customer.Address, testData.Customer.Postcode, testData.Customer.City, testData.Customer.Phone, testData.Customer.Email)
                        .CheckTerms()
                        .PlaceOrder<SuccessfulOrderPage>();

            Assert.AreEqual("Zamówienie otrzymane", successfulOrderPage.EntryHeader.Text, "Page header is not what expected. Order was not sucessful.");
        }

        [Test]
        public void BuyAsARegistredUser()
        {
            TestData.User user = testData.User;
            TestData.Card card = testData.Card;

            ProductPage productPage = new ProductPage(driver);
            SuccessfulOrderPage successfulOrderPage = productPage.GoToPage(firstProductPage).AddToCart().GoToCart().GoToPayments()
                        .LoginAsRegistredUserData(user.Email, user.Password)
                        .FillCreditCardData(card.Number, card.ExpirationDate, card.Cvc)
                        .CheckTerms()
                        .PlaceOrder<SuccessfulOrderPage>();

            Assert.AreEqual("Zamówienie otrzymane", successfulOrderPage.EntryHeader.Text, "Page header is not what expected. Order was not sucessful.");
        }

        private float CalculateTax(float total)
        {
            return (float)Math.Round(total - (total / 1.23), 2);
        }

        private string FormatNumber(float number)
        {
            return string.Format("{0:### ###.00}", number) + " zł";
        }
    }
}
