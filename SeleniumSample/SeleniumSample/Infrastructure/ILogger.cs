using System;

namespace SeleniumSample.Infrastructure
{
    public interface ILogger
    {
        void LogError(Exception exception);
    }
}
