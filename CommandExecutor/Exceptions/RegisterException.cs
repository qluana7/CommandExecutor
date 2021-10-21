using System;

namespace CommandExecutor.Exceptions
{
    public class RegisterException : Exception
    {
        public RegisterException(Type innerclass): base()
        { this.InnerClass = innerclass; }
        
        public RegisterException(Type innerclass, string msg): base(msg)
        { this.InnerClass = innerclass; }
        
        public Type InnerClass { get; }
    }
}