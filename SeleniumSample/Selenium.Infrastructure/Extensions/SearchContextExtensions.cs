using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenium.Infrastructure.Extensions
{
    public static class SearchContextExtensions
    {
        public static IWebElement SafeFindElement(
            this ISearchContext searchContext,
            By selector)
        {
            try
            {
                IWebElement element = searchContext.FindElement(selector);
                return (element.Displayed && element.Enabled) ? element : null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        public static IReadOnlyList<IWebElement> SafeFindElements(
            this ISearchContext searchContext,
            By selector)
        {
            return searchContext.SafeFindElementsThat(selector, e => true);
        }

        public static IReadOnlyList<IWebElement> SafeFindElementsThat(
            this ISearchContext searchContext,
            By selector,
            Func<IWebElement, bool> predicate)
        {
            try
            {
                IReadOnlyList<IWebElement> elements = searchContext.FindElements(selector);
                return elements.Where(e => e.Displayed && e.Enabled && predicate(e)).ToList();
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }
    }
}
