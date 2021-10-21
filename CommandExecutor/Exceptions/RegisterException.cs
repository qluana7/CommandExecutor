using System;

namespace CommandExecutor.Exceptions
{
    /// <summary>
    /// occurred exception when register commands executed.
    /// </summary>
    public class RegisterException : Exception
    {
        /// <summary>
        /// Initialize object with inner class type
        /// </summary>
        /// <param name="innerclass">The class that raises exception</param>
        public RegisterException(Type innerclass): base()
        { this.InnerClass = innerclass; }
        
        /// <summary>
        /// Initialize object with inner class type and message
        /// </summary>
        /// <param name="innerclass">The class that raises exception</param>
        /// <param name="message">The message for exception</param>
        public RegisterException(Type innerclass, string message): base(message)
        { this.InnerClass = innerclass; }
        
        /// <summary>
        /// The inner class that raises exception.
        /// </summary>
        public Type InnerClass { get; }
    }
}