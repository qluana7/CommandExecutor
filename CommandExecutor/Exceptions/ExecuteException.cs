using System;

namespace CommandExecutor
{
    /// <summary>
    /// A logical exception that occurs during command execution.
    /// It occurs trying to run command in preparation
    /// </summary>
    public class ExecuteException : Exception
    {
        /// <summary>
        /// Initialize object with string
        /// </summary>
        /// <param name="message">The exception message</param>
        public ExecuteException(string message) : base (message) { }
    }
}