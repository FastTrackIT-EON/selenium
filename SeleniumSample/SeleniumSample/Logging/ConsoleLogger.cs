using SeleniumSample.Infrastructure;
using System;
using System.Text;

namespace SeleniumSample.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void LogError(Exception exception)
        {
            StringBuilder errorBuilder = new StringBuilder();
            BuildLogErrorInternal(exception, errorBuilder);

            Console.WriteLine(errorBuilder.ToString());
        }
        private void BuildLogErrorInternal(Exception exception, StringBuilder errorBuilder)
        {   
            errorBuilder.AppendLine($"Error: {exception.GetType()}");
            errorBuilder.AppendLine($"Message: {exception.Message}");
            errorBuilder.AppendLine("StackTrace:");
            errorBuilder.AppendLine(exception.StackTrace);

            if (exception.InnerException != null)
            {
                errorBuilder.AppendLine("Inner Exception:");
                BuildLogErrorInternal(exception.InnerException, errorBuilder);
            }
        }
    }
}
