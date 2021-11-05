using System;
using CommandExecutor.Structures;

namespace CommandExecutor.Exceptions
{
    /// <summary>
    /// Occurred exception when command check failed.
    /// </summary>
    public class CommandCheckFailException : Exception
    {
        /// <summary>
        /// Initialize object with Command class
        /// </summary>
        /// <param name="cmd">The command which raised the exception</param>
        public CommandCheckFailException(Command cmd) : base() { InnerCommand = cmd; }
        
        /// <summary>
        /// Initialize object with command class and exception message
        /// </summary>
        /// <param name="cmd">The command which raised the exception</param>
        /// <param name="message">The message of exception</param>
        public CommandCheckFailException(Command cmd, string message) : base(message) { InnerCommand = cmd; }
        
        /// <summary>
        /// The command which raised the exception
        /// </summary>
        public Command InnerCommand { get; set; }
    }
}