using System;
using CommandExecutor.Structures;

namespace CommandExecutor.EventArgs
{
    /// <summary>
    /// The <see cref="System.EventArgs"/> contains <see cref="Structures.Command"/>
    /// </summary>
    public class CommandEventArgs : System.EventArgs
    {
        internal CommandEventArgs(Command cmd) : base()
        {
            Command = cmd;
            Client = cmd.Client;
        }
        
        /// <summary>
        /// The main client
        /// </summary>
        /// <value></value>
        public Executor Client { get; }
        
        /// <summary>
        /// The command raised event.
        /// </summary>
        public Command Command { get; }
    }
}