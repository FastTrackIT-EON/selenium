using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenium.Infrastructure.Extensions
{
    public static class WebDriverWaitExtensions
    {
        public static IWebElement ForElement(
            this IWait<IWebDriver> webDriverWait,
            By selector)
        {
            try
            {
                return webDriverWait.Until(wd => wd.SafeFindElement(selector));
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }

        public static IWebElement ForChildElement(
            this IWait<IWebDriver> webDriverWait,
            ISearchContext parentElement,
            By selector)
        {
            try
            {
                // attepmt to wait for the child element to appear
                return webDriverWait.Until(wd => parentElement.SafeFindElement(selector));
            }
            catch (WebDriverTimeoutException)
            {
                // child element didn't appeared in the specified timeout
                return null;
            }
        }

        public static IReadOnlyList<IWebElement> ForElementsThat(
            this IWait<IWebDriver> webDriverWait,
            By selector,
            Func<IWebElement, bool> predicate)
        {
            try
            {
                return webDriverWait.Until(
                    wd =>
                    {
                        IReadOnlyList<IWebElement> elements = wd.SafeFindElementsThat(selector, predicate);

                        return elements.Any() ? elements : null;
                    });
            }
            catch (WebDriverTimeoutException)
            {
                // elements didn't appeared in the specified timeout
                return new List<IWebElement>();
            }
        }
    }
}
