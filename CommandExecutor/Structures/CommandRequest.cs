using CommandExecutor.Exceptions;

namespace CommandExecutor.Structures
{
    /// <summary>
    /// The command request object for <see cref="CommandNotFoundException"/>
    /// </summary>
    public class CommandRequest
    {
        /// <summary>
        /// Inner command.
        /// </summary>
        public string Command { get; set; }
        
        /// <summary>
        /// Inner arguments
        /// </summary>
        public string[] Arguments { get; set; }
    }
}