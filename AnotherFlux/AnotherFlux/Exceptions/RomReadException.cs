using System;
using System.Runtime.Serialization;

namespace AnotherFlux.Exceptions
{
    [Serializable]
    internal class RomReadException : Exception
    {
        public RomReadException()
        {
        }

        public RomReadException(string message) : base(message)
        {
        }

        public RomReadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RomReadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}