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
        /// Initialize object with <see cref="CommandRequest"/>
        /// </summary>
        /// <param name="request"></param>
        public CommandNotFoundException(CommandRequest request): base(GetExceptionMessage(request))
        {
            this.Request = request;
        }
        
        /// <summary>
        /// The request object when exception raised.
        /// </summary>
        public CommandRequest Request { get; set; }
        
        private static string GetExceptionMessage(CommandRequest req)
            => $"Cannot found method name : {req.Command}. Check options if some options set false.";
    }
}