using System;
using System.Collections.Generic;
using System.Reflection;
using CommandExecutor.Attributes;
using CommandExecutor.Exceptions;

namespace CommandExecutor.Structures
{
    /// <summary>
    /// The class that provides information of Command.
    /// This object can get from FindCommand method.
    /// </summary>
    public class Command
    {
        internal Command(ref Executor client, CommandModule c, MethodInfo m)
        {
            Client = client;
            ModuleClass = c;
            _command = m;
            _attribute = _command.GetCustomAttribute<CommandAttribute>();
            _checks = _command.GetCustomAttributes<CommandCheckAttribute>(true);
        }
        
        /// <summary>
        /// The main client.
        /// </summary>
        public Executor Client { get; }
        
        /// <summary>
        /// The class that command included.
        /// </summary>
        public CommandModule ModuleClass { get; }
        
        /// <summary>
        /// The name of command.
        /// If Name property of CommandAttribute is unset, This set to method name.
        /// </summary>
        public string Name => string.IsNullOrWhiteSpace(_attribute.Name) ?
                              _command.Name : _attribute.Name;
        
        /// <summary>
        /// The alias of command.
        /// If Alias property of CommandAttribute is unset, This property is null.
        /// </summary>
        public string[] Alias => _attribute.Alias;
        
        /// <summary>
        /// Determine command is private.
        /// </summary>
        public bool IsPrivate => _attribute.IsPrivate;
        
        /// <summary>
        /// The custom attributes of command.
        /// </summary>
        public IEnumerable<Attribute> CustomAttributes => _command.GetCustomAttributes();
        
        private readonly MethodInfo _command;
        
        private readonly CommandAttribute _attribute;
        
        private readonly IEnumerable<CommandCheckAttribute> _checks;
        
        /// <summary>
        /// Run command with object.
        /// object array must be contain object that converted as parameter type.
        /// DO NOT USE MANUALLY
        /// </summary>
        /// <param name="param">The object array that is command parameters</param>
        public void Execute(object[] param)
        {
            if (ModuleClass.CheckExecute(this) == false) throw new CommandCheckFailException(this, $"CheckExecute returns false in {ModuleClass.GetType().Name}");
            
            ModuleClass.BeforeExecute();
            
            foreach (var c in _checks)
                if (!c.Check(this))
                    throw new CommandCheckFailException(this, $"Check returns false in {c.GetType().Name}");
            
            _command.Invoke(ModuleClass, param);
            
            ModuleClass.AfterExecute();
        }
        
        /// <summary>
        /// Run command with string which will convert as parameter type array
        /// </summary>
        /// <param name="param">The string for method parameter. parameter will split with space.</param>
        public void Execute(string param) =>
            Execute(Client.ConvertParameter(_command, param?.Split(' ')));
    }
}