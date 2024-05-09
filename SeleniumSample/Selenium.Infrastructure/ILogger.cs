using System;

namespace Selenium.Infrastructure
{
    public interface ILogger
    {
        void LogError(Exception exception);
    }
}
