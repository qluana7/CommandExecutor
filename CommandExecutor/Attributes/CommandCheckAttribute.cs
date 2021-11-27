using System;
using CommandExecutor.Structures;

namespace CommandExecutor.Attributes
{
    /// <summary>
    /// Check before method execute.
    /// You can create your own check attribute by inheriting this class
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class CommandCheckAttribute : Attribute
    {
        /// <summary>
        /// Check can method execute.
        /// If return value is true, command will execute; otherwise not execute.
        /// </summary>
        /// <param name="cmd">The class that has information of command which executing</param>
        /// <returns>Boolean value that determines whether command execute</returns>
        public abstract bool Check(Command cmd);
    }
}