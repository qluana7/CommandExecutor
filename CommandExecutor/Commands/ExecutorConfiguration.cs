using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandExecutor.Attributes;
using CommandExecutor.EventArgs;
using CommandExecutor.Exceptions;
using CommandExecutor.Structures;

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
        
        /// <summary>
        /// The exception event.
        /// Can set config for throwing exception 
        /// </summary>
        public ExceptionManager ExceptionConfiguration { get; set; } = ExceptionManager.Default;
    }
    
    /// <summary>
    /// Managing exceptions.
    /// </summary>
    public class ExceptionManager
    {
        /// <summary>
        /// Initialize object with empty object
        /// </summary>
        public ExceptionManager(): this(new Dictionary<Type, CommandThrowType>()) { }
        
        /// <summary>
        /// Initialize object with custom dictionary.
        /// Alll keys must be exception type
        /// </summary>
        /// <param name="dict">The object that has information of processing exception.</param>
        /// <exception cref="ArgumentException"/>
        public ExceptionManager(Dictionary<Type, CommandThrowType> dict)
        {
            if (dict.Keys.Any(l => !l.IsSubclassOf(typeof(Exception))))
                throw new ArgumentException("All keys must be exception type.");
            
            exceptions = dict;
        }
        
        /// <summary>
        /// Default value of <see cref="ExceptionManager"/>
        /// </summary>
        /// <returns>
        /// <see cref="CommandNotFoundException"/> = Event or Exception
        /// <br/>
        /// <see cref="ArgumentException"/> = Event or Exception
        /// </returns>
        public static ExceptionManager Default =>
            new ExceptionManager() {
                exceptions = new Dictionary<Type, CommandThrowType>() {
                    // [typeof(CommandCheckFailException)] = CommandThrowType.Exception,
                    [typeof(CommandNotFoundException)]  = CommandThrowType.Event_or_Exception,
                    // [typeof(ExecuteException)]          = CommandThrowType.Exception,
                    // [typeof(RegisterException)]         = CommandThrowType.Exception,
                    [typeof(ArgumentException)]         = CommandThrowType.Event_or_Exception,
                    // [typeof(InvalidCastException)]      = CommandThrowType.Exception
                },
                DefaultType = CommandThrowType.Exception
            };
        
        /// <summary>
        /// The default processing type when exception that not registered in manager threw
        /// </summary>
        public CommandThrowType DefaultType { get; set; }
        
        private Dictionary<Type, CommandThrowType> exceptions;
        
        internal Executor _client;
        
        /// <summary>
        /// Remove all registered exceptions.
        /// </summary>
        public void Clear() => exceptions.Clear();
        
        /// <summary>
        /// Register new exception.
        /// </summary>
        /// <param name="type">Type of exception</param>
        /// <param name="ttype">Processing type</param>
        /// <exception cref="ArgumentException"/>
        public void Add(Type type, CommandThrowType ttype)
        {
            if (!type.IsSubclassOf(typeof(Exception)))
                throw new ArgumentException("Given type is not exception.");
            
            exceptions.Add(type, ttype);
        }
        
        /// <summary>
        /// Set exists exception to new processing type
        /// </summary>
        /// <param name="type">Type of exception</param>
        /// <param name="ttype">Processing type</param>
        /// <exception cref="KeyNotFoundException"/>
        public void Set(Type type, CommandThrowType ttype)
        {
            if (exceptions.ContainsKey(type))
                exceptions[type] = ttype;
            else
                throw new KeyNotFoundException();
        }
        
        /// <summary>
        /// <see cref="Set(Type, CommandThrowType)"/> generic version.
        /// </summary>
        /// <param name="ttype">Processing type</param>
        /// <typeparam name="T">Type of exception</typeparam>
        /// <exception cref="KeyNotFoundException"/>
        public void Set<T>(CommandThrowType ttype) where T : Exception
            => Set(typeof(T), ttype);
        
        /// <summary>
        /// Try set value.
        /// If set successfully, return <see langword="true"/>; otherwise, <see langword="false"/>
        /// </summary>
        /// <param name="type">Type of exception</param>
        /// <param name="ttype">Processing type</param>
        public bool TrySet(Type type, CommandThrowType ttype)
        {
            if (exceptions.ContainsKey(type))
            {
                exceptions[type] = ttype;
                return true;
            }
            else return false;
        }
        
        /// <summary>
        /// Get processing type
        /// </summary>
        /// <param name="type">Type of exception</param>
        /// <exception cref="KeyNotFoundException"/>
        public CommandThrowType Get(Type type)
        {
            if (exceptions.ContainsKey(type))
                return exceptions[type];
            else
                throw new KeyNotFoundException();
        }
        
        /// <summary>
        /// <see cref="Get(Type)"/> generic version
        /// </summary>
        /// <typeparam name="T">Type of exception</typeparam>
        /// <exception cref="KeyNotFoundException"/>
        public CommandThrowType Get<T>() where T : Exception
            => Get(typeof(T));
        
        /// <summary>
        /// Try get value.
        /// If get sucessfully, return <see langword="true"/>; otherwise, <see langword="false"/>
        /// </summary>
        /// <param name="type">Type of exception</param>
        /// <param name="output">Processing type of exception. If task failed, it sets to <see cref="CommandThrowType.None"/></param>
        /// <returns></returns>
        public bool TryGet(Type type, out CommandThrowType output)
        {
            if (exceptions.ContainsKey(type))
            {
                output = exceptions[type];
                return true;
            }
            else
            {
                output = CommandThrowType.None;
                return false;
            }
        }
        
        /// <summary>
        /// <see langword="true"/> if exception registered; otherwise, <see langword="false"/>
        /// </summary>
        /// <param name="type">Type of exception</param>
        public bool Contains(Type type) => exceptions.ContainsKey(type);
        
        /// <summary>
        /// <see cref="Contains(Type)"/> generic version
        /// </summary>
        /// <typeparam name="T">Type of exception.</typeparam>
        public bool Contains<T>() where T : Exception => exceptions.ContainsKey(typeof(T));
        
        /// <summary>
        /// <see cref="ExceptionManager"/> to <see cref="Dictionary{Type, CommandThrowingType}"/>
        /// </summary>
        public Dictionary<Type, CommandThrowType> ToDictionary() => new Dictionary<Type, CommandThrowType>(exceptions);
    }
    
    /// <summary>
    /// The processing option when exception occurred.
    /// </summary>
    public enum CommandThrowType
    {
        /// <summary>
        /// Do nothing.
        /// </summary>
        None = 0,
        
        /// <summary>
        /// Only raise event.
        /// If event is <see langword="null"/>, exception ignored
        /// </summary>
        Event = 1 << 0,
        
        /// <summary>
        /// Only throw exception.
        /// </summary>
        Exception = 1 << 1,
        
        /// <summary>
        /// Raise event and throw exception.
        /// </summary>
        Event_and_Exception = Event | Exception,
        
        /// <summary>
        /// Raise event and throw exception if event is none.
        /// </summary>
        Event_or_Exception = 1 << 2
    }
}