using System;

namespace Core.Domain
{
    public class ReThrownException : Exception
    {
        public ReThrownException()
        { }

        public ReThrownException(string message) : base(message)
        {
        }

        public ReThrownException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}