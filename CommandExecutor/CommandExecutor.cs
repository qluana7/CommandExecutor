using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandExecutor
{
    public class CommandExecutor
    {
        public CommandExecutor(CommandExecutorConfiguration configuration)
        {
            this._commands = new List<ICommandModule>();
            this.Options = configuration.ToOptions();
        }
        
        private readonly List<ICommandModule> _commands;
        
        internal CommandExecutorOptions Options { get; set; }
        
        public ICommandModule[] Commands => _commands.ToArray();
        
        public void RegisterCommands<T>() where T : ICommandModule, new()
            => this._commands.Add(new T());
        
        public void UnregisterCommands<T>() where T : ICommandModule, new()
        {
            ICommandModule mod = this._commands.FirstOrDefault(l => l is T);
            if (mod == null) throw new NullReferenceException("There's no registered class matching with give class");
            this._commands.Remove(mod);
        }
        
        public void Execute(string content)
        {
            string[] c = content.Split(' ');
            Execute(c[0], c.Skip(1).ToArray());
        }
        
        internal void Execute(string command, string[] value)
        {
            MethodInfo method;
            
            for (int i = 0; i < this._commands.Count; i++)
            {
                MethodInfo info = _commands[i].GetType().GetMethods().First(l => l.Name.ToLower() == command.ToLower());
                if (info == null) continue;
                method = info;
                break;
            }
        }
    }
}
