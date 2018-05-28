using System;
using System.Runtime.Serialization;

namespace CardGameEngine.Games
{
    public abstract class CardGameException : Exception
    {
        protected CardGameException()
        {
        }

        protected CardGameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected CardGameException(string message) : base(message)
        {
        }

        protected CardGameException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}