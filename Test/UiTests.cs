using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using VivatUiTests.Base;
using System;
using System.Linq;

namespace VivatUiTests.Tests
{
    public class UiTests : BaseTest
    {
        private void ClosePopupsIfPresent()
        {
            try
            {
                var popupButtons = driver.FindElements(By.XPath(
                    "//button[contains(., 'Прийняти') or contains(., 'Добре') or contains(., 'Зрозуміло') or contains(., 'Close')]" +
                    " | //a[contains(., 'Прийняти') or contains(., 'Добре') or contains(., 'Зрозуміло')]"));

                var visibleButton = popupButtons.FirstOrDefault(e => e.Displayed && e.Enabled);
                if (visibleButton != null)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", visibleButton);
                }
            }
            catch
            {
                //нічого не робимо
            }
        }

        [Test]
        public void HomePage_ShouldOpenSuccessfully()
        {
            driver.Navigate().GoToUrl(baseUrl);
            ClosePopupsIfPresent();

            Assert.That(driver.Title, Is.Not.Empty);
            Assert.That(driver.Url, Does.Contain("vivat.com.ua"));
        }

        [Test]
        public void SearchField_ShouldBeVisible_OnHomePage()
        {
            driver.Navigate().GoToUrl(baseUrl);
            ClosePopupsIfPresent();

            var searchInput = wait.Until(d =>
            {
                return d.FindElements(By.CssSelector(
                    "input[type='search'], input[name='q'], input[placeholder*='Пошук'], input[placeholder*='пошук']"))
                    .FirstOrDefault(e => e.Displayed && e.Enabled);
            });

            Assert.That(searchInput, Is.Not.Null);
            Assert.That(searchInput.Displayed, Is.True);
            Assert.That(searchInput.Enabled, Is.True);
        }

        [Test]
        public void Search_ShouldReturnResults_ForValidBookName()
        {
            driver.Navigate().GoToUrl(baseUrl);
            ClosePopupsIfPresent();

            var searchInput = wait.Until(d =>
            {
                return d.FindElements(By.CssSelector(
                    "input[type='search'], input[name='q'], input[placeholder*='Пошук'], input[placeholder*='пошук']"))
                    .FirstOrDefault(e => e.Displayed && e.Enabled);
            });

            Assert.That(searchInput, Is.Not.Null);

            searchInput.Clear();
            searchInput.SendKeys("книга");
            searchInput.SendKeys(Keys.Enter);

            wait.Until(d => d.Url.Contains("search") || d.PageSource.ToLower().Contains("книга"));

            Assert.That(driver.Url.Contains("search") || driver.PageSource.ToLower().Contains("книга"), Is.True);
        }

        [Test]
        public void ProductPage_ShouldOpen_FromHomePage()
        {
            driver.Navigate().GoToUrl(baseUrl);
            ClosePopupsIfPresent();

            var firstProduct = wait.Until(d =>
            {
                return d.FindElements(By.CssSelector("a[href*='/product/']"))
                    .FirstOrDefault(e => e.Displayed);
            });

            Assert.That(firstProduct, Is.Not.Null);

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", firstProduct);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", firstProduct);

            wait.Until(d => d.Url.Contains("/product/"));

            Assert.That(driver.Url, Does.Contain("/product/"));
            Assert.That(driver.Title, Is.Not.Empty);
        }

        [Test]
        public void ProductPage_ShouldContain_MainContent()
        {
            driver.Navigate().GoToUrl("https://vivat.com.ua/product/shoper-chytaiu-nochamy-knyzhky-vivat/");
            ClosePopupsIfPresent();

            wait.Until(d => d.Url.Contains("/product/"));

            var bodyText = wait.Until(d =>
            {
                var body = d.FindElement(By.TagName("body"));
                return !string.IsNullOrWhiteSpace(body.Text) ? body.Text : null;
            });

            Assert.That(driver.Title, Does.Contain("Vivat").IgnoreCase);
            Assert.That(driver.Url, Does.Contain("/product/"));
            Assert.That(bodyText, Is.Not.Null);
            Assert.That(bodyText.Length, Is.GreaterThan(50));
        }
    }
}