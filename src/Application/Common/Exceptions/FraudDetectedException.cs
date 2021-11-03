using System;

namespace Application.Common.Exceptions
{
    public class FraudDetectedException : Exception
    {
        public FraudDetectedException()
        {
        }

        public FraudDetectedException(string message) : base(message)
        {
        }
    }
}