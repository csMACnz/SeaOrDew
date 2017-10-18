namespace csMACnz.SeaOrDew
{
    public static class Handler
    {
        public static SuccessHandlerResult Success { get; } = new SuccessHandlerResult();

        public class SuccessHandlerResult
        {
            internal SuccessHandlerResult() { }
        }
    }
}
