using OpenQA.Selenium;

namespace SeleniumSample.Infrastructure
{
    public interface IAutomationScript
    {
        void Execute(IWebDriver webDriver);
    }
}
