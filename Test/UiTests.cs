using NUnit.Framework;
using VivatUiTests.Base;
using VivatUiTests.Pages;

namespace VivatUiTests.Tests
{
    public class UiTests : BaseTest
    {
        [Test]
        public void HomePage_ShouldOpenSuccessfully()
        {
            var home = new HomePage(driver);
            home.Open(baseUrl);

            Assert.That(driver.Title, Is.Not.Empty);
        }

        [Test]
        public void SearchField_ShouldBeVisible_OnHomePage()
        {
            var home = new HomePage(driver);
            home.Open(baseUrl);

            Assert.That(home.IsSearchVisible(), Is.True);
        }

        [Test]
        public void Search_ShouldReturnResults()
        {
            var home = new HomePage(driver);
            home.Open(baseUrl);

            home.Search("книга");

            Assert.That(driver.PageSource.ToLower().Contains("книга"));
        }

        [Test]
        public void ProductPage_ShouldOpen()
        {
            var home = new HomePage(driver);
            var product = new ProductPage(driver);

            home.Open(baseUrl);
            product.OpenFirstProduct();

            Assert.That(product.IsProductPageOpened(), Is.True);
        }
    }
}