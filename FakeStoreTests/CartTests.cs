using FakeStorePageObjects;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace FakeStoreTests
{
    public class CartTests : BaseTest
    {
        IList<string> ProductsIDs => new List<string>() { testData.Products[0].Id, testData.Products[1].Id };

        string firstProductPage => testData.Products[0].Url;
        string secondProductPage => testData.Products[1].Url;

        [Test]
        public void ProductAddedToCart_CartIsNotEmpty()
        {
            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage
                .GoToPage(firstProductPage)
                .AddToCart()
                .GoToCart();

            Assert.Multiple(() => {
                Assert.AreEqual(1, cartPage.CartItems.Count, "Number of product in cart is not 1");
                Assert.AreEqual(ProductsIDs[0], cartPage.ItemId,
                    "Product's in cart id is not " + ProductsIDs[0]);
            });
        }

        [Test]
        public void TwoProductAddedToCart_TwoProductsAreInCart()
        {
            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage
                .GoToPage(firstProductPage)
                .AddToCart(2)
                .GoToCart();

            Assert.Multiple(() => {
                Assert.AreEqual(1, cartPage.CartItems.Count, "Number of product in cart is not 1");
                Assert.AreEqual(ProductsIDs[0], cartPage.ItemId,
                    "Product's in cart id is not " + ProductsIDs[0]);
                Assert.AreEqual("2", cartPage.CartItems[0].FindElement(By.CssSelector("input[type='number']")).GetAttribute("value"),
                    "Amount of product is not " + 2);
            });
        }

        [Test]
        public void TwoDifferentProductAddedToCart_TwoDifferentProductsAreInCart()
        {
            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage
                .GoToPage(firstProductPage)
                .AddToCart()
                .GoToPage(secondProductPage)
                .AddToCart()
                .GoToCart();

            Assert.Multiple(() => {
                Assert.AreEqual(2, cartPage.CartItems.Count, "Number of product in cart is not 2");
                Assert.AreEqual(ProductsIDs[0], cartPage.ItemIds[0],
                    "Product's in cart id is not " + ProductsIDs[0]);
                Assert.AreEqual(ProductsIDs[1], cartPage.ItemIds[1],
                    "Product's in cart id is not " + ProductsIDs[1]);
            });
        }

        [Test]
        public void GoToCart_CartIsEmpty()
        {
            CartPage cartPage = new CartPage(driver);
            cartPage.GoToPage();

            Assert.DoesNotThrow(() => _ = cartPage.CartEmpyMessage.GetDomProperty("hidden"), "Cart is not empty");
        }

        [Test]
        public void AddZeroItem_ValidatiomMsgShouldBeVisible()
        {
            ProductPage productPage = new ProductPage(driver);
            productPage.GoToPage(firstProductPage).AddToCart(0);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(productPage.IsQuantityFieldRangeUnderflowPresent(), "Test was probably able to add 0 items to cart. Range Underflow validation didn't return true.");
            });
        }

        [Test]
        public void UpdateProductQuantityInCart_QuantityShouldBeUpdated()
        {
            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage
                .GoToPage(firstProductPage)
                .AddToCart()
                .GoToCart()
                .UpdateQuantity(2);

            Assert.Multiple(() => {
                Assert.AreEqual(1, cartPage.CartItems.Count, "Number of product in cart is not 1");
                Assert.AreEqual(ProductsIDs[0], cartPage.CartItems[0].FindElement(By.CssSelector("a")).GetAttribute("data-product_id"),
                    "Product's in cart id is not " + ProductsIDs[0]);
                Assert.AreEqual("2", (cartPage.CartItems[0].FindElement(By.CssSelector("input[type='number']"))).GetAttribute("value"),
                    "Amount of product is not " + 2);
                Assert.AreEqual("true", cartPage.UpdateCartBtn.GetAttribute("aria-disabled"), "Update button is active");
            }); 
        }

        [Test]
        public void UpdateProductQuantityOverStock()
        {
            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage
                .GoToPage(firstProductPage)
                .AddToCart()
                .GoToCart();

            int stockMax = int.Parse((cartPage.CartItems[0].FindElement(By.CssSelector("input[type='number']")).GetAttribute("max")))+1;

            cartPage.UpdateQuantity(stockMax);

            bool isMoreThanStock = (bool)js.ExecuteScript("return arguments[0].validity.rangeOverflow", cartPage.QuantityInCartFld);

            Assert.IsTrue(isMoreThanStock, "Test was probably able to add more items than available in stock. Range Overflow validation didn't return ");
        }

    }
}