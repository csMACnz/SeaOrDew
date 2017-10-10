using System;

namespace csMACnz.SeaOrDew
{
    public class CommandError
    {
        public static int DefaultErrorCode { get; set; } = 500;

        public CommandError(string message)
            : this(DefaultErrorCode, message, null)
        {
        }

        public CommandError(string message, Exception ex)
            : this(DefaultErrorCode, message, ex)
        {
        }

        public CommandError(int errorCode, string message)
            : this(errorCode, message, null)
        {
        }

        public CommandError(int errorCode, string message, Exception ex)
        {
            ErrorCode = errorCode;
            Message = message;
            Ex = ex;
        }

        public int ErrorCode { get; }
        public string Message { get; }
        public Exception Ex { get; }
    }
}