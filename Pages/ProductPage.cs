using OpenQA.Selenium;

namespace VivatUiTests.Pages
{
    public class ProductPage
    {
        private IWebDriver driver;

        private By productLink = By.CssSelector("a[href*='/product/']");

        public ProductPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void OpenFirstProduct()
        {
            var product = driver.FindElement(productLink);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", product);
        }

        public bool IsProductPageOpened()
        {
            return driver.Url.Contains("/product/");
        }
    }
}