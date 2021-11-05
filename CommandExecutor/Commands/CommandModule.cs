using System;
using CommandExecutor.Structures;

namespace CommandExecutor
{
    /// <summary>
    /// The interface to specify that it is a command class
    /// </summary>
    public abstract class CommandModule
    {
        /// <summary>
        /// The CommandCheckAttribute class version.
        /// </summary>
        /// <param name="cmd">The command that will execute</param>
        /// <returns>The boolean value that determines whether the command execute</returns>
        public virtual bool CheckExecute(Command cmd) => true;
        
        /// <summary>
        /// This method execute before the method execute.
        /// </summary>
        public virtual void BeforeExecute() { }
        
        /// <summary>
        /// This method execute after the method execute.
        /// </summary>
        public virtual void AfterExecute() { }
    }
}