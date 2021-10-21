using System;
using CommandExecutor.Structures;

namespace CommandExecutor.Exceptions
{
    public class CommandNotFoundException : Exception
    {
        public CommandNotFoundException(CommandRequest request): base(GetExceptionMessage(request))
        {
            this.Request = request;
        }
        
        public CommandRequest Request { get; set; }
        
        private static string GetExceptionMessage(CommandRequest req)
            => $"Cannot found method name : {req.Command}. Check options if some options set false.";
    }
}