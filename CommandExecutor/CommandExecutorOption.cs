using System;
using System.Reflection;

namespace CommandExecutor
{
    [Flags]
    internal enum CommandExecutorOptions
    {
        None = 0,
        IgnoreCase = 1 << 0
    }
    
    public class CommandExecutorConfiguration
    {
        public bool IgnoreCase { get; set; }
        
        internal CommandExecutorOptions ToOptions()
        {
            string[] names = Enum.GetNames<CommandExecutorOptions>();
            PropertyInfo[] infos = this.GetType().GetProperties(BindingFlags.Public);
            
            int options = 0;
            
            for (int i = 0; i < infos.Length; i++)
            {
                PropertyInfo info = infos[i];
                
                if (!((bool?)info.GetValue(this)).Value) continue;
                
                int ind = Array.IndexOf(names, info.Name);
                options |= 1 << ind;
            }
            
            return (CommandExecutorOptions)options;
        }
    }
}