using System;
using System.Linq;
using System.Reflection;
using CommandExecutor.Attributes;

namespace CommandExecutor
{
    /// <summary>
    /// The class that can configure executor.
    /// </summary>
    public class ExecutorConfiguration
    {
        /// <summary>
        /// If this set to true, ignore case sensitive when find command.
        /// </summary>
        public bool IgnoreCase { get; set; } = false;
        
        /// <summary>
        /// If this set to true, ignore extra arguments when execute command.
        /// </summary>
        public bool IgnoreExtraArguments { get; set; } = false;
        
        /// <summary>
        /// If this set to true, find command method allow to get method with <see cref="CommandAttribute"/>'s <see cref="CommandAttribute.IsPrivate"/> set to true
        /// </summary>
        public bool GetPrivateMethod { get; set; } = false;
        
        /// <summary>
        /// If this set to true, find command method allow to get static method in class.
        /// </summary>
        public bool IncludeStaticMethod { get; set; } = true;
    }
}