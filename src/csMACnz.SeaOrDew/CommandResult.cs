﻿namespace csMACnz.SeaOrDew
{
    public class CommandResult<TError>
    {
        public TError Problem { get; }

        public bool IsSuccess { get; }

        public static CommandResult<TError> Success => new CommandResult<TError>();

        private CommandResult() { IsSuccess = true; }

        public CommandResult(TError problem)
        {
            IsSuccess = false;
            Problem = problem;
        }

        public static implicit operator CommandResult<TError>(TError problem)
        {
            return new CommandResult<TError>(problem);
        }
    }
}
