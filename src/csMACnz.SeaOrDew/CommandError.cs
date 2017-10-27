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
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException($"`{nameof(message)}` is required. It must not be null or whitespace.", nameof(message));
            }

            ErrorCode = errorCode;
            Message = message;
            Ex = ex;
        }

        public int ErrorCode { get; }
        public string Message { get; }
        public Exception Ex { get; }
    }
}