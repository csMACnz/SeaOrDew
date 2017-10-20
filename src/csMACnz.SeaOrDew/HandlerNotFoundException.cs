using System;

namespace csMACnz.SeaOrDew
{
    public class HandlerNotFoundException : Exception
    {
        public HandlerNotFoundException(Type expectedType, string message)
            : base(message)
        {
            ExpectedType = expectedType;
        }
        public HandlerNotFoundException(Type expectedType, string message, Exception innerException)
            : base(message, innerException)
        {
            ExpectedType = expectedType;
        }

        public Type ExpectedType { get; }
    }
}
