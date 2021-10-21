using System;
using System.Linq;
using System.Reflection;
using CommandExecutor.Attributes;

namespace CommandExecutor
{
    [Flags]
    internal enum ExecutorOptions
    {
        None = 0,
        IgnoreCase = 1 << 0,
        IgnoreExtraArguments = 1 << 1,
        GetPrivateMethod = 1 << 2
    }
    
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
        
        internal ExecutorOptions ToOptions()
        {
            string[] names = Enum.GetNames<ExecutorOptions>();
            PropertyInfo[] infos = this.GetType()
                                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        .Where(l => l.PropertyType == typeof(bool))
                                        .ToArray();
            
            int options = 0;
            
            for (int i = 0; i < infos.Length; i++)
            {
                PropertyInfo info = infos[i];
                
                if (!((bool?)info.GetValue(this)).Value) continue;
                
                int ind = Array.IndexOf(names, info.Name) - 1;
                
                options |= 1 << ind;
            }
            
            return (ExecutorOptions)options;
        }
    }
}