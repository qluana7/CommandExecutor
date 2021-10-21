using System;
using System.Collections.Generic;

namespace CommandExecutor.Attributes
{
    /// <summary>
    /// The attribute that specifying the command method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Initialize object. The command name will set to method name.
        /// </summary>
        public CommandAttribute()
        {
            this.Name = null;
            this.Alias = null;
            this.IsPrivate = false;
        }
        
        /// <summary>
        /// Initialize object with <paramref name="name"/>.
        /// If it set, method name not use as command.
        /// </summary>
        /// <param name="name">The command which used in find command.</param>
        public CommandAttribute(string name): this()
        {
            this.Name = name;
        }
        
        /// <summary>
        /// Initialize object with <paramref name="name"/> and <paramref name="alias"/>.
        /// If it set, method name not use as command.
        /// </summary>
        /// <param name="name">The command which used in find command.</param>
        /// <param name="alias">The alias for command.</param>
        /// <returns></returns>
        public CommandAttribute(string name, string[] alias): this(name)
        {
            this.Alias = alias;
        }
        
        /// <summary>
        /// Initialize object.
        /// Set whether it is private method.
        /// </summary>
        /// <param name="isPrivate">If this option set to true, executor won't register this command.
        /// (but <see cref="ExecutorConfiguration.GetPrivateMethod"/> set to true, executor will register this command.</param>
        public CommandAttribute(bool isPrivate): this()
        {
            this.IsPrivate = isPrivate;
        }
        
        /// <summary>
        /// Initialize object with name.
        /// </summary>
        /// <param name="name">The command which used in find command.</param>
        /// <param name="isPrivate">If this option set to true, executor won't register this command.
        /// (but <see cref="ExecutorConfiguration.GetPrivateMethod"/> set to true, executor will register this command.</param>
        public CommandAttribute(string name, bool isPrivate): this(name)
        {
            this.IsPrivate = isPrivate;
        }
        
        /// <summary>
        /// Initialize object with <paramref name="name"/> and <paramref name="alias"/>
        /// </summary>
        /// <param name="name">The command which used in find command.</param>
        /// /// <param name="alias">The alias for command.</param>
        /// <param name="isPrivate">If this option set to true, executor won't register this command.
        /// (but <see cref="ExecutorConfiguration.GetPrivateMethod"/> set to true, executor will register this command.</param>
        public CommandAttribute(string name, string[] alias, bool isPrivate): this(name, alias)
        {
            this.IsPrivate = isPrivate;
        }
        
        /// <summary>
        /// The name of command. If this is null, name will be method name.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The alias of command.
        /// </summary>
        public string[] Alias { get; }
        
        /// <summary>
        /// If this option set to true, executor won't register this command.
        /// (but <see cref="ExecutorConfiguration.GetPrivateMethod"/> set to true, executor will register this command.
        /// </summary>
        /// <value></value>
        public bool IsPrivate { get; }
    }
}