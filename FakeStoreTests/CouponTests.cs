using FakeStorePageObjects;
using NUnit.Framework;

namespace FakeStoreTests
{
    public class CouponTests : BaseTest
    {
        int firstProductPrice => testData.Products[0].Price;
        int secondProductPrice => testData.Products[1].Price;
        int thirdProductPrice => testData.Products[2].Price;

        string firstProductPriceText => testData.Products[0].PriceText;
        string secondProductPriceText => testData.Products[1].PriceText;
        string thirdProductPriceText => testData.Products[2].PriceText;

        string firstProductPage => testData.Products[0].Url;
        string secondProductPage => testData.Products[1].Url;
        string thirdProductPage => testData.Products[2].Url;

        string coupon300 = "kwotowy300";
        int coupon300Value = 300;

        string coupon10procent = "10procent";
        float coupon10procentValue = 0.1f;

        string categoryCoupon = "windsurfing350";
        int categoryCouponValue = 350;

        [Test]
        public void MinimalValueCouponTest()
        {
            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage
                .GoToPage(firstProductPage)
                .AddToCart()
                .GoToCart()
                .AddCoupon(coupon300);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("Kupon został pomyślnie użyty.", cartPage.CouponAlert.Text, "Alert is not present");
                Assert.AreEqual("Kupon: " + coupon300, cartPage.CartDiscount.Text, "Coupon name is not correct");
                Assert.AreEqual("300,00 zł", cartPage.CouponAmountField.Text, "Coupon amount is not correct.");
                Assert.AreEqual(FormatNumber(firstProductPrice - coupon300Value), cartPage.OrderTotalWithoutTaxElement.Text, "Total is not correct. Was the coup applied?");
            });
        }

        [Test]
        public void TooSmallSumForMinimalValueCouponTest()
        {
            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage
                .GoToPage(secondProductPage)
                .AddToCart()
                .GoToCart()
                .AddCoupon(coupon300);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("Minimalna wartość zamówienia dla tego kuponu to 3 000,00 zł.", cartPage.ErrorAlert.Text, "Alert is not present");
                Assert.AreEqual(FormatNumber(secondProductPrice), cartPage.OrderTotalWithoutTaxElement.Text, "Total is not correct. Was the coup applied?");
            });
        }

        [Test]
        public void PercentCouponTest()
        {
            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage
                .GoToPage(firstProductPage)
                .AddToCart()
                .GoToCart()
                .AddCoupon(coupon10procent);

            float couponValue = firstProductPrice * coupon10procentValue;
            Assert.Multiple(() =>
            {
                Assert.AreEqual("Kupon został pomyślnie użyty.", cartPage.CouponAlert.Text, "Alert is not present");
                Assert.AreEqual("Kupon: " + coupon10procent, cartPage.CartDiscount.Text, "Coupon name is not correct");
                Assert.AreEqual(FormatNumber(couponValue), cartPage.CouponAmountField.Text, "Coupon amount is not correct.");
                Assert.AreEqual(FormatNumber(firstProductPrice - couponValue), cartPage.OrderTotalWithoutTaxElement.Text, "Total is not correct. Was the coup applied?");
            });
        }

        [Test]
        public void CategoryCouponTest()
        {
            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage
                .GoToPage(thirdProductPage)
                .AddToCart()
                .GoToCart()
                .AddCoupon(categoryCoupon);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("Kupon został pomyślnie użyty.", cartPage.CouponAlert.Text, "Alert is not present");
                Assert.AreEqual("Kupon: " + categoryCoupon, cartPage.CartDiscount.Text, "Coupon name is not correct");
                Assert.AreEqual(FormatNumber(categoryCouponValue), cartPage.CouponAmountField.Text, "Coupon amount is not correct.");
                Assert.AreEqual(FormatNumber(thirdProductPrice - categoryCouponValue), cartPage.OrderTotalWithoutTaxElement.Text, "Total is not correct. Was the coup applied?");
            });
        }

        [Test]
        public void CategoryCouponTest_CouponFromOutsideCategory()
        {
            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = productPage
                .GoToPage(secondProductPage)
                .AddToCart()
                .GoToCart()
                .AddCoupon(categoryCoupon);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("Przepraszamy, tego kuponu nie można zastosować do wybranych produktów.", cartPage.ErrorAlert.Text, "Alert is not present");
                Assert.AreEqual(secondProductPriceText, cartPage.OrderTotalWithoutTaxElement.Text, "Total is not correct. Was the coup applied?");
 
            });
        }

        private string FormatNumber(float number)
        {
            if (number < 1000)
            {
                return string.Format("{0:###.00}", number) + " zł";
            }
            else return string.Format("{0:### ###.00}", number) + " zł";
        }
    }
}
