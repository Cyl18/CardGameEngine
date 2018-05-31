using System;

namespace CardGameEngine.Exceptions
{
    public class TypeMismatchException : Exception
    {
        public TypeMismatchException(string message) : base(message)
        {
        }
    }
}