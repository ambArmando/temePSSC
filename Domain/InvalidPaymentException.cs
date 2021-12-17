using System;
using System.Runtime.Serialization;

namespace lab1PSSC.Domain
{
    [Serializable]
    internal class InvalidPaymentException : Exception
    {
        public InvalidPaymentException()
        {
        }

        public InvalidPaymentException(string message) : base(message)
        {
        }

        public InvalidPaymentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidPaymentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}