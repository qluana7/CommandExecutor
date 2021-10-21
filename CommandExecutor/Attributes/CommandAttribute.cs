using System;
using System.Collections.Generic;

namespace CommandExecutor.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute()
        {
            this.Name = null;
            this.Alias = null;
            this.IsPrivate = false;
        }
        
        public CommandAttribute(string name): this()
        {
            this.Name = name;
        }
        
        public CommandAttribute(string name, string[] alias): this(name)
        {
            this.Alias = alias;
        }
        
        public CommandAttribute(bool isPrivate): this()
        {
            this.IsPrivate = isPrivate;
        }
        
        public CommandAttribute(string name, bool isPrivate): this(name)
        {
            this.IsPrivate = isPrivate;
        }
        
        public CommandAttribute(string name, string[] alias, bool isPrivate): this(name, alias)
        {
            this.IsPrivate = isPrivate;
        }
        
        public string Name { get; }
        
        public string[] Alias { get; }
        
        public bool IsPrivate { get; }
    }
}