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
        
        public Type ExpectedType { get; }
    }
}
