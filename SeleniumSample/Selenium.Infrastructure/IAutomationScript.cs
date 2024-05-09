using OpenQA.Selenium;

namespace Selenium.Infrastructure
{
    public interface IAutomationScript
    {
        void Execute(IWebDriver webDriver);
    }
}
