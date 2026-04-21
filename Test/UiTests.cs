using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using VivatUiTests.Base;
using System.Linq;

namespace VivatUiTests.Tests
{
    public class UiTests : BaseTest
    {
        [Test]
        public void HomePage_ShouldOpenSuccessfully()
        {
            driver.Navigate().GoToUrl(baseUrl);

            Assert.That(driver.Title, Is.Not.Empty);
            Assert.That(driver.Url, Does.Contain("vivat.com.ua"));
        }

        [Test]
        public void SearchField_ShouldBeVisible_OnHomePage()
        {
            driver.Navigate().GoToUrl(baseUrl);

            var searchInput = wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[type='search'], input[name='q'], input[placeholder*='Пошук'], input[placeholder*='пошук']")));

            Assert.That(searchInput.Displayed, Is.True);
            Assert.That(searchInput.Enabled, Is.True);
        }

        [Test]
        public void Search_ShouldReturnResults_ForValidBookName()
        {
            driver.Navigate().GoToUrl(baseUrl);

            var searchInput = wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[type='search'], input[name='q'], input[placeholder*='Пошук'], input[placeholder*='пошук']")));

            searchInput.Clear();
            searchInput.SendKeys("книга");
            searchInput.SendKeys(Keys.Enter);

            wait.Until(d => d.PageSource.ToLower().Contains("книга") || d.Url.Contains("search"));

            Assert.That(driver.PageSource.ToLower(), Does.Contain("книга"));
        }

        [Test]
        public void ProductPage_ShouldOpen_FromSearchResults()
        {
            driver.Navigate().GoToUrl(baseUrl);

            var searchInput = wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("input[type='search'], input[name='q'], input[placeholder*='Пошук'], input[placeholder*='пошук']")));

            searchInput.Clear();
            searchInput.SendKeys("книга");
            searchInput.SendKeys(Keys.Enter);

            var firstProduct = wait.Until(ExpectedConditions.ElementExists(
                By.CssSelector("a[href*='/product/'], .product-card a, .product-item a")));

            firstProduct.Click();

            wait.Until(d => d.Url.Contains("/product/") || d.PageSource.Length > 0);

            Assert.That(driver.Url, Does.Contain("product").IgnoreCase);
        }

        [Test]
        public void ProductPage_ShouldContain_AddToCartButton()
        {
            driver.Navigate().GoToUrl("https://vivat.com.ua/product/shoper-chytaiu-nochamy-knyzhky-vivat/#characteristics");

            var addToCartButton = wait.Until(d =>
            {
                var elements = d.FindElements(By.XPath(
                    "//button[contains(., 'Купити') or contains(., 'До кошика') or contains(., 'Додати')]" +
                    " | //a[contains(., 'Купити') or contains(., 'До кошика') or contains(., 'Додати')]"));
                return elements.FirstOrDefault(e => e.Displayed);
            });

            Assert.That(addToCartButton, Is.Not.Null);
            Assert.That(addToCartButton.Displayed, Is.True);
            Assert.That(addToCartButton.Enabled, Is.True);
        }
    }
}