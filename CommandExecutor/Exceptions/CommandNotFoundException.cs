using System;
using CommandExecutor.Structures;

namespace CommandExecutor.Exceptions
{
    /// <summary>
    /// Occurred exception when cannot find command.
    /// </summary>
    public class CommandNotFoundException : Exception
    {
        /// <summary>
        /// Initialize object with string
        /// </summary>
        /// <param name="request">The name of command.</param>
        public CommandNotFoundException(string request): base(GetExceptionMessage(request))
        {
            this.CommandName = request;
        }
        
        /// <summary>
        /// The request object when exception raised.
        /// </summary>
        public string CommandName { get; set; }
        
        private static string GetExceptionMessage(string req)
            => $"Cannot found method name : {req}. Check options if some options set false.";
    }
}