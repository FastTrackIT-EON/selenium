using OpenQA.Selenium;
using System;
using System.Runtime.ExceptionServices;

namespace SeleniumSample.Infrastructure
{
    public class AutomationEngine
    {
        private readonly Func<IWebDriver> _webDriverFactory;
        private readonly ILogger _logger;

        public AutomationEngine(
            Func<IWebDriver> webDriverFactory,
            ILogger logger)
        { 
            _webDriverFactory = webDriverFactory ?? throw new ArgumentNullException(nameof(webDriverFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        public void Execute(IAutomationScript script)
        {
            ExceptionDispatchInfo edi = null;

            using (IWebDriver driver = _webDriverFactory.Invoke())
            {
                try
                {
                    script.Execute(driver);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex);

                    // capture in order to re-throw later
                    edi = ExceptionDispatchInfo.Capture(ex);
                }
                finally
                {
                    driver.Quit();
                }
            }

            edi?.Throw(); 
        }
    }
}
