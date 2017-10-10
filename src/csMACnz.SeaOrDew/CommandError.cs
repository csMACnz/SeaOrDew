using System;

namespace csMACnz.SeaOrDew
{
    public class CommandError
    {
        public CommandError(string message, Exception ex = null)
        {
            Message = message;
            Ex = ex;
        }

        public string Message { get; }
        public Exception Ex { get; }
    }
}