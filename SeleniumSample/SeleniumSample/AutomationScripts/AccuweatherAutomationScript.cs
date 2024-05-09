using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using Selenium.Infrastructure;
using Selenium.Infrastructure.Extensions;
using System;
using System.IO;
using System.Linq;

namespace SeleniumSample
{
    public class AccuweatherAutomationScript : IAutomationScript
    {
        public AccuweatherAutomationScript(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
            {
                throw new ArgumentNullException(nameof(cityName));
            }

            CityName = cityName;
        }

        public string CityName { get; }

        public void Execute(IWebDriver webDriver)
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(15));

            webDriver
                .Navigate()
                .GoToUrl("https://www.accuweather.com/");

            // is GDPR consent appearing?
            IWebElement cookieConsent = wait.ForElementsThat(
                By.ClassName("fc-dialog"),
                elem =>
                {
                    string ariaLabel = elem.GetAttribute("aria-label");
                    return !string.IsNullOrEmpty(ariaLabel) &&
                           ariaLabel.StartsWith(
                               "AccuWeather needs your consent to use your personal data",
                               StringComparison.OrdinalIgnoreCase);
                })
                .FirstOrDefault();

            if (cookieConsent != null)
            {
                // accept GDPR policy
                IWebElement consentButton = wait.ForChildElement(cookieConsent, By.CssSelector("button.fc-cta-consent"));
                consentButton?.Click();
            }

            IWebElement searchForm = wait.ForElement(By.ClassName("search-form"));
            if (searchForm is null)
            {
                throw new AutomationException("Unable to locate Accuweather's search form");
            }

            IWebElement searchTextBox = wait.ForChildElement(
                    searchForm,
                    By.CssSelector("input[type=text].search-input"));

            if (searchTextBox is null)
            {
                throw new AutomationException("Unable to locate Accuweather's text search");
            }

            searchTextBox.SendKeys(CityName);

            searchForm.Submit();

            // after form submission we navigate to a new url
            // wait for the new page to load
            wait.ForElement(By.CssSelector("body.search-locations"));

            IWebElement locationsListContainer = wait.ForElement(By.CssSelector("div.locations-list"));
            if (locationsListContainer == null)
            {
                throw new AutomationException("Unable to locate search results list");
            }

            IWebElement cityLink = locationsListContainer.SafeFindElementsThat(
                By.TagName("a"),
                e =>
                {
                    string href = e.GetAttribute("href");
                    return !string.IsNullOrEmpty(href) &&
                           href.IndexOf($"city={CityName}", StringComparison.OrdinalIgnoreCase) >= 0;
                })
                .FirstOrDefault();

            if (cityLink is null)
            {
                throw new AutomationException($"Unable to locate link for {CityName}");
            }

            cityLink.Click();

            // after click on the link we navigate to a new url
            // wait for the new page to load
            IWebElement contentCityWeather = wait.ForElement(By.CssSelector("div.page-content"));
            if (contentCityWeather is null)
            {
                throw new AutomationException($"Unable to load content page for {CityName}");
            }
            else
            {
                // TODO: remove this after fixing the issue with ad overlay!!!
                File.WriteAllText("Page.html", webDriver.PageSource);
            }

            // issue here due ad overlay!!!
            //CloseAdFrameOnCityPage(webDriver);

            IWebElement wheaterTempContainer = wait.ForElement(By.CssSelector("div.temp"));
            if (wheaterTempContainer != null)
            {
                Console.Write($"Wheather in {CityName} is {wheaterTempContainer.Text}");
            }

            webDriver.TakeScreenshot().SaveAsFile("Screenshoot.png");
        }

        private void CloseAdFrameOnCityPage(IWebDriver webDriver)
        {
            bool wasFrameSwitch = false;
            try
            {
                webDriver.SwitchTo().Frame("ad_iframe");
                wasFrameSwitch = true;

                Console.WriteLine("Found Ad Frame");

                IWebElement closeTextContainer = webDriver.SafeFindElement(By.CssSelector("div.close-text"));
                closeTextContainer?.Click();
            }
            catch (NoSuchFrameException) 
            {
                Console.WriteLine("Can't find Ad Frame");
            }
            finally
            {
                if (wasFrameSwitch)
                {
                    webDriver.SwitchTo().DefaultContent();
                }
            }
        }
    }
}
