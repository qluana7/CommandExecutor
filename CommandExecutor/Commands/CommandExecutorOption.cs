using System;
using System.Linq;
using System.Reflection;

namespace CommandExecutor
{
    [Flags]
    public enum ExecutorOptions
    {
        None = 0,
        IgnoreCase = 1 << 0,
        IgnoreExtraArguments = 1 << 1,
        GetPrivateMethod = 1 << 2
    }
    
    public class ExecutorConfiguration
    {
        public bool IgnoreCase { get; set; } = false;
        
        public bool IgnoreExtraArguments { get; set; } = false;
        
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