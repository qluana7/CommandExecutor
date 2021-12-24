using System;
using CommandExecutor.Structures;

namespace CommandExecutor.EventArgs
{
    /// <summary>
    /// The <see cref="System.EventArgs"/> that for <see cref="Executor.CommandErrored"/>
    /// </summary>
    public class CommandExceptionEventArgs : System.EventArgs
    {
        internal CommandExceptionEventArgs(Command inner, Exception e) : base()
        {
            InnerCommand = inner;
            InnerException = e.InnerException ?? e;
            Client = inner.Client;
        }
        
        /// <summary>
        /// The main client
        /// </summary>
        public Executor Client { get; }
        
        /// <summary>
        /// The exception that be raised event.
        /// </summary>
        public Exception InnerException { get; }
        
        /// <summary>
        /// The command that be raised event
        /// </summary>
        /// <value></value>
        public Command InnerCommand { get; }
    }
}