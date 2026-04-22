using OpenQA.Selenium;

namespace VivatUiTests.Pages
{
    public class HomePage
    {
        private IWebDriver driver;

        private By searchInput = By.CssSelector("input[type='search']");

        public HomePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void Open(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public bool IsSearchVisible()
        {
            return driver.FindElement(searchInput).Displayed;
        }

        public void Search(string text)
        {
            var input = driver.FindElement(searchInput);
            input.Clear();
            input.SendKeys(text + Keys.Enter);
        }
    }
}