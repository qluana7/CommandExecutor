using System;

namespace CommandExecutor.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DescriptionAttribute : Attribute
    {
        public DescriptionAttribute(string desc)
        {
            this.Description = desc;
        }
        
        public string Description { get; }
    }
}