namespace csMACnz.SeaOrDew
{
    public class CommandResult<TError>
    {
        public TError Problem { get; }

        public bool IsSuccess => Problem == null;

        public static CommandResult<TError> Success => new CommandResult<TError>();

        private CommandResult() { }

        public CommandResult(TError problem)
        {
            Problem = problem;
        }

        public static implicit operator CommandResult<TError>(TError problem)
        {
            return new CommandResult<TError>(problem);
        }
    }
}
