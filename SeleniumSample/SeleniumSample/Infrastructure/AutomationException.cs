using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumSample.Infrastructure
{
    public class AutomationException : Exception
    {
        public AutomationException(string message) 
            : base(message)
        {
        }

        public AutomationException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }


    }
}
