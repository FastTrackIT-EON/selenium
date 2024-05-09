using OpenQA.Selenium.Chrome;
using SeleniumSample.Infrastructure;
using SeleniumSample.Logging;
using System;

namespace SeleniumSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AutomationEngine engine = new AutomationEngine(
                () => new ChromeDriver(),
                new ConsoleLogger());

            engine.Execute(new AccuweatherAutomationScript("Cluj-Napoca"));

            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }
}
